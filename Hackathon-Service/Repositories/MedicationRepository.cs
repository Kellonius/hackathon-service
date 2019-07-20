using System.Collections.Generic;
using System.Linq;
using Hackathon_DataAccess;

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

        public List<Script> GetScriptsByPatientAndMedication(int patientid, int medicationId)
        {
            using (var context = new HackathonEntities())
            {
                return context.Scripts.Where(s => s.PatientId == patientid && s.MedicationId == medicationId).ToList();
            }
        }
    }
}