using System.Collections.Generic;

namespace Hackathon_Service.Models.Epic
{
    public class Address
    {
        public string use { get; set; }
        public List<string> line { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postalCode { get; set; }
        public string country { get; set; }
        public Period period { get; set; }
    }
}