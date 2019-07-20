namespace Hackathon_Service.Models.Epic
{
    public class Repeat
    {
        public decimal frequency { get; set; }
        public decimal period { get; set; }
        public string periodUnits { get; set; }
        public BoundsPeriod boundsPeriod { get; set; }
    }
}