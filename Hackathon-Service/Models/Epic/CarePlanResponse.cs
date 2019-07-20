using System.Collections.Generic;

namespace Hackathon_Service.Models.Epic
{
    public class CarePlanResponse
    {
        public string resourceType { get; set; }
        public string type { get; set; }
        public int total { get; set; }
        public List<Link> link { get; set; }
        public List<CarePlanEntry> entry { get; set; }
    }
}