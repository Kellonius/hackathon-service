using System;
using System.Collections.Generic;

namespace Hackathon_Service.Models
{
    public class ScriptRequest
    {
        public int medicalProfessionalId { get; set; }
        public int patientId { get; set; }
        public int pharmacyId { get; set; }
        public string MedicationGenericName { get; set; }
        public string MedicationMedicalName { get; set; }

        public string MedicationTime { get; set; }
        public string MedicationRoute { get; set; }
        public string Dosage { get; set; }
    }

}