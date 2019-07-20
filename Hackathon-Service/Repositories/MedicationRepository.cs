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
        public List<Medication> GetMedicationByPatient(int userId)
        {
            using (var context = new HackathonEntities())
            {
                var meds = context.Medications.ToList();
                var patientId = context.Patients.FirstOrDefault(x => x.UserId == userId).PatientId;
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

        public List<PatientScript> GetIncomingPrescriptions()
        {
            using (var context = new HackathonEntities())
            {
                return (from s in context.Scripts
                        where s.MPId != null && s.DateFilled == null
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
            using (var context = new HackathonEntities())
            {
                return (from s in context.Scripts
                        where s.MPId != null && s.DateFilled != null && s.DatePickedUp == null
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
                context.Scripts.Where(s => prescriptionIds.Contains(s.ScriptId))
                    .ForEach(s => s.DateFilled = DateTime.Now);
                context.SaveChanges();
            }
        }

        public void MarkPrescriptionsAsPickedUp(List<int> prescriptionIds)
        {
            using (var context = new HackathonEntities())
            {
                context.Scripts.Where(s => prescriptionIds.Contains(s.ScriptId))
                    .ForEach(s =>
                    {
                        if (s.DateFilled == null)
                        {
                            s.DateFilled = DateTime.Now;
                        }

                        s.DatePickedUp = DateTime.Now;
                    });
                context.SaveChanges();
            }
        }
    }
}