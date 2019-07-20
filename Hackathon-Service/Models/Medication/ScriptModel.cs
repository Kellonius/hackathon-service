using System;
using System.Collections.Generic;
using Hackathon_DataAccess;

namespace Hackathon_Service.Models
{
    public class ScriptModel
    {
        public int? MedicationId { get; set; }
        public string MedicationTime { get; set; }
        public string MedicationRoute { get; set; }
        public string Dosage { get; set; }
        public DateTime? DateIssued { get; set; }
        public DateTime? DateFilled { get; set; }
        public DateTime? DatePickedUp { get; set; }

        public ScriptModel(Script script)
        {
            MedicationId = script.MedicationId;
            MedicationTime = script.MedicationTime;
            MedicationRoute = script.MedicationRoute;
            Dosage = script.Dosage;
            DateIssued = script.DateIssued;
            DateFilled = script.DateFilled;
            DatePickedUp = script.DatePickedUp;
        }
    }

}