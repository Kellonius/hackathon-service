using System.Collections.Generic;

namespace Hackathon_Service.Models.Epic
{
    public class MedicationOrder
    {
        public string resourceType { get; set; }
        public string dateWritten { get; set; }
        public string status { get; set; }
        public string id { get; set; }
        public List<Identifier> identifier { get; set; }
        public Reference patient { get; set; }
        public Reference prescriber { get; set; }
        public Reference medicationReference { get; set; }
        public List<DosageInstruction> dosageInstruction { get; set; }
        public DispenseRequest dispenseRequest { get; set; }
    }
}