using System.Collections.Generic;
using System.Linq;
using m = Hackathon_DataAccess.Medication;

namespace Hackathon_Service.Models
{
    public class MedicationModel
    {
        public int MedicationId { get; set; }
        public string GenericName { get; set; }
        public string MedicalName { get; set; }

        public MedicationModel()
        {
        }

        public MedicationModel(m medication)
        {
            MedicationId = medication.MedicationId;
            GenericName = medication.GenericName;
            MedicalName = medication.MedicalName;
        }
    }
}