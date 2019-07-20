namespace Hackathon_Service.Models.Epic
{
    public class DosageInstruction
    {
        public string text { get; set; }
        public bool asNeededBoolean { get; set; }
        public Property route { get; set; }
        public Property method { get; set; }
        public Timing timing { get; set; }
        public DoseQuantity doseQuantity { get; set; }
    }
}