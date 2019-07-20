using System.Collections.Generic;
using System.Linq;
using Hackathon_DataAccess;
using Hackathon_Service.Models.Medication;

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
                return context.Scripts.Where(s => s.PatientId == patientId && s.MedicationId == request.medicationId).ToList();
            }
        }

        public List<Script> GetPrescriptions()
        {
            using (var context = new HackathonEntities())
            {
                return context.Scripts.ToList();
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
                    MPId = 6
                };

                context.Scripts.Add(script);
                context.SaveChanges();
                context.Dispose();
            }
        }
    }
}