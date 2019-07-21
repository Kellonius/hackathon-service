using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hackathon_Service.Models.Users.Requests
{
    public class PatientDeniedMedicationRequest
    {
        public int ScriptId { get; set; }
        public string Reason { get; set; }
    }
}