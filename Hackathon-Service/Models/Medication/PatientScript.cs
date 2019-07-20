using Hackathon_DataAccess;

namespace Hackathon_Service.Models.Medication
{
    public class PatientScript
    {
        public int ScriptId { get; set; }
        public string PatientName { get; set; }
        public string MedicationGenericName { get; set; }
        public string MedicationMedicalName { get; set; }
        public string MedicationTime { get; set; }
        public string MedicationRoute { get; set; }
        public string Dosage { get; set; }
        public string PrescribedBy { get; set; }

        public PatientScript(Script script, string firstName, string lastName, string prescribedByName, Hackathon_DataAccess.Medication medication)
        {
            ScriptId = script.ScriptId;
            PatientName = lastName + ", " + firstName;
            MedicationTime = script.MedicationTime;
            MedicationRoute = script.MedicationRoute;
            MedicationGenericName = medication.GenericName;
            MedicationMedicalName = medication.MedicalName;
            Dosage = script.Dosage;
            PrescribedBy = prescribedByName;
        }
    }
}