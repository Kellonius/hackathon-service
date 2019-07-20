using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hackathon_Service.Models.Medication
{
    public class MedicationAddRequest
    {
        public int userId { get; set; }
        public string name { get; set; }
        public string medName { get; set; }
        public string dosage { get; set; }
        public string time { get; set; }
    }
}