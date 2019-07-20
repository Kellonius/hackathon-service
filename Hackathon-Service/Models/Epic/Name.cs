using System.Collections.Generic;

namespace Hackathon_Service.Models.Epic
{
    public class Name
    {
        public string use { get; set; }
        public string text { get; set; }
        public List<string> family { get; set; }
        public List<string> given { get; set; }
    }
}