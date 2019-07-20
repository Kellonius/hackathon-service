using System.Collections.Generic;

namespace Hackathon_Service.Models.Epic
{
    public class Medication
    {
        public string resourceType { get; set; }
        public string id { get; set; }
        public Product product { get; set; }
        public Property code { get; set; }
    }
}