using System.Collections.Generic;

namespace Hackathon_Service.Models.Epic
{

    public class CarePlanEntry
    {
        public string fullUrl { get; set; }
        public List<Link> link { get; set; }
        public Search search { get; set; }
        public CarePlan resource { get; set; }
    }
}