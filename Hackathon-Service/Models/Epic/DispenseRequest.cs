namespace Hackathon_Service.Models.Epic
{
    public class DispenseRequest
    {
        public ValidityPeriod validityPeriod { get; set; }
        public decimal? numberOfRepeatsAllowed { get; set; }
        public ExpectedSupplyDuration expectedSupplyDuration { get; set; }
        public Quantity quantity { get; set; }
    }
}