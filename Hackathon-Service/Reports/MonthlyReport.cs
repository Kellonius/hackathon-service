namespace Hackathon_Service.Reports
{
    public class MonthlyReport
    {
        public string Month { get; set; }
        public string Year { get; set; }
        public int PickedUpPrescriptions { get; set; }
        public int UnPickedUpPrescriptions { get; set; }
    }
}