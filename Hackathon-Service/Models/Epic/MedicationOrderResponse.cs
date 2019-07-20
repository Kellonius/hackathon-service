using System;
using System.Collections.Generic;

namespace Hackathon_Service.Models.Epic
{

    public class MedicationOrderResponse
    {
        public string resourceType { get; set; }
        public string type { get; set; }
        public decimal total { get; set; }
        public List<Link> link { get; set; }
        public List<Entry> entry { get; set; }
    }
}