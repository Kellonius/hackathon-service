using System;
using System.Collections.Generic;
using System.Linq;
using Hackathon_DataAccess;
using Hackathon_Service.Models.Medication;
using Microsoft.Ajax.Utilities;

namespace Hackathon_Service.Repositories
{
    public class MedicationRepository
    {
        public List<Medication> GetMedicationByPatient(int patientId)
        {
            using (var context = new HackathonEntities())
            {
                var meds = context.Medications.ToList();
                var returnedMeds = meds.Where(m =>
                    context.Scripts.Where(s => s.PatientId == patientId).Select(s => s.MedicationId)
                        .Contains(m.MedicationId)).ToList();
                return returnedMeds;
            }
        }

        public List<Script> GetScriptsByPatientAndMedication(MedicationPrescriptionRequest request)
        {
            using (var context = new HackathonEntities())
            {
                var patientId = context.Patients.FirstOrDefault(x => x.UserId == request.userId).PatientId;
                return context.Scripts.Where(s => s.PatientId == patientId && s.MedicationId == request.medicationId)
                    .ToList();
            }
        }

        public List<Script> GetPrescriptions()
        {
            using (var context = new HackathonEntities())
            {
                return context.Scripts.ToList();
            }
        }

        private void ReconcilePharmacyEventsToScripts()
        {
            using (var context = new HackathonEntities())
            {
                var scriptIds = context.Scripts.Select(s => s.ScriptId)
                    .Where(s => !context.PharmacyEvents.Any(pe => pe.ScriptId == s)).ToList();
                context.PharmacyEvents.AddRange(scriptIds.Select(s => new PharmacyEvent
                {
                    Id = 0,
                    DateFilled = null,
                    DatePickedUp = null,
                    ScriptId = s
                }));
                context.SaveChanges();
            }
        }

        public List<PatientScript> GetIncomingPrescriptions()
        {
            ReconcilePharmacyEventsToScripts();
            using (var context = new HackathonEntities())
            {
                return (from s in context.Scripts
                        join pe in context.PharmacyEvents on s.ScriptId equals pe.ScriptId
                        where s.MPId != null && s.DateFilled == null && pe.DateFilled == null
                        select new
                        {
                            Script = s,
                            FirstName = s.Patient.user.first_name,
                            LastName = s.Patient.user.last_name,
                            PrescribedBy = "Dr. " + s.MedicalProfessional.user.first_name + " " +
                                           s.MedicalProfessional.user.last_name,
                            Medication = s.Medication
                        }
                    ).ToList().Select(s => new PatientScript(
                        s.Script,
                        s.FirstName,
                        s.LastName,
                        s.PrescribedBy,
                        s.Medication
                    )).ToList();
//                return context.Scripts.Where(s => s.DateFilled == null && s.DatePickedUp == null).ToList();
            }
        }

        public List<PatientScript> GetOutgoingPrescriptions()
        {
            ReconcilePharmacyEventsToScripts();
            using (var context = new HackathonEntities())
            {
                return (from s in context.Scripts
                        join pe in context.PharmacyEvents on s.ScriptId equals pe.ScriptId
                        where s.MPId != null && s.DateFilled != null && s.DatePickedUp == null &&
                              pe.DateFilled != null && pe.DatePickedUp == null
                        select new
                        {
                            Script = s,
                            FirstName = s.Patient.user.first_name,
                            LastName = s.Patient.user.last_name,
                            PrescribedBy = "Dr. " + s.MedicalProfessional.user.first_name + " " +
                                           s.MedicalProfessional.user.last_name,
                            Medication = s.Medication
                        }
                    ).ToList().Select(s => new PatientScript(
                        s.Script,
                        s.FirstName,
                        s.LastName,
                        s.PrescribedBy,
                        s.Medication
                    )).ToList();
//                return (from s in context.Scripts
//                    where s.MPId != null && s.DateFilled != null && s.DatePickedUp == null
//                    select new PatientScript(
//                        s,
//                        s.Patient.user.first_name, s.Patient.user.last_name,
//                        "Dr. " + s.MedicalProfessional.user.first_name + " " + s.MedicalProfessional.user.last_name,
//                        s.Medication
//                    )).ToList();
//                return context.Scripts.Where(s => s.DateFilled != null && s.DatePickedUp == null).ToList();
            }
        }

        public void MarkPrescriptionsAsFilled(List<int> prescriptionIds)
        {
            using (var context = new HackathonEntities())
            {
                context.PharmacyEvents.Where(s => prescriptionIds.Contains(s.ScriptId))
                    .ForEach(s => s.DateFilled = DateTime.Now);
//                context.Scripts.Where(s => prescriptionIds.Contains(s.ScriptId))
//                    .ForEach(s => s.DateFilled = DateTime.Now);
                context.SaveChanges();
            }
        }

        public void MarkPrescriptionsAsPickedUp(List<int> prescriptionIds)
        {
            using (var context = new HackathonEntities())
            {
                context.PharmacyEvents.Where(s => prescriptionIds.Contains(s.ScriptId))
                    .ForEach(s =>
                    {
                        if (s.DateFilled == null)
                        {
                            s.DateFilled = DateTime.Now;
                        }

                        s.DatePickedUp = DateTime.Now;
                    });
//                context.Scripts.Where(s => prescriptionIds.Contains(s.ScriptId))
//                    .ForEach(s =>
//                    {
//                        if (s.DateFilled == null)
//                        {
//                            s.DateFilled = DateTime.Now;
//                        }
//
//                        s.DatePickedUp = DateTime.Now;
//                    });
                context.SaveChanges();
            }
        }

        public void AddMedication(MedicationAddRequest request)
        {
            using (var context = new HackathonEntities())
            {
                var med = new Medication()
                {
                    MedicalName = request.medName,
                    GenericName = request.name
                };
                context.Medications.Add(med);
                context.SaveChanges();

                var patientId = context.Patients.FirstOrDefault(x => x.UserId == request.userId).PatientId;

                var script = new Script()
                {
                    PatientId = patientId,
                    MedicationId = med.MedicationId,
                    Dosage = request.dosage,
                    MedicationTime = request.time,
                    MPId = 6,
                    DateIssued = DateTime.Now,
                    DateFilled = DateTime.Now,
                    DatePickedUp = DateTime.Now
                };

                context.Scripts.Add(script);
                context.SaveChanges();
                context.Dispose();
            }
        }
    }
}