using System.Collections.Generic;

namespace Hackathon_Service.Models.Epic
{

    public class Entry
    {
        public string fullUrl { get; set; }
        public Search search { get; set; }
        public List<Link> link { get; set; }
        public MedicationOrder resource { get; set; }
    }
}