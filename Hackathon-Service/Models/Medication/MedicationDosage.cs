namespace Hackathon_Service.Models.Medication
{
    public class MedicationDosage
    {
        public MedicationModel Medication { get; set; }
        public ScriptModel Script { get; set; }
    }
}