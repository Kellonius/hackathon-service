using System.Collections.Generic;

namespace Hackathon_Service.Reports
{
    public class YearlyReport
    {
        public string Year { get; set; }
        public List<MonthlyReport> MonthlyReports { get; set; }
    }
}