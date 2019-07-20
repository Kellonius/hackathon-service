using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hackathon_Service.Models.Medication
{
    public class MedicationPrescriptionRequest
    {
        public int userId { get; set; }
        public int medicationId { get; set; }
    }
}