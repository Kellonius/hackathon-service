using System.Collections.Generic;

namespace Hackathon_Service.Models.Epic
{
    public class CarePlan
    {
        public string resourceType { get; set; }
        public string id { get; set; }
        public string status { get; set; }
        public Reference subject { get; set; }
        public List<Address> addresses { get; set; }
        public List<Reference> goal { get; set; }
        public List<Activity> activity { get; set; }
        public List<Property> category { get; set; }
    }
}