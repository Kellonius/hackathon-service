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

        public List<Script> GetIncomingPrescriptions()
        {
            using (var context = new HackathonEntities())
            {
                return context.Scripts.Where(s => s.DateFilled == null && s.DatePickedUp == null).ToList();
            }
        }

        public List<Script> GetOutgoingPrescriptions()
        {
            using (var context = new HackathonEntities())
            {
                return context.Scripts.Where(s => s.DateFilled != null && s.DatePickedUp == null).ToList();
            }
        }

        public void MarkPrescriptionsAsFilled(List<int> prescriptionIds)
        {
            using (var context = new HackathonEntities())
            {
                context.Scripts.Where(s => prescriptionIds.Contains(s.ScriptId))
                    .ForEach(s => s.DateFilled = DateTime.Now);
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
            }
        }
    }
}