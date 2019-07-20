using System;
using System.Collections.Generic;

namespace Hackathon_Service.Models
{
    public class ScriptRequest
    {
        public int MedicationId { get; set; }
        public string MedicationTime { get; set; }
        public string MedicationRoute { get; set; }
        public string Dosage { get; set; }
    }

}