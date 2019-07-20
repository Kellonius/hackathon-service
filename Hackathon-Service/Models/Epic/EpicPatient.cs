using System;
using System.Collections.Generic;

namespace Hackathon_Service.Models.Epic
{
    public class EpicPatient
    {
        public string resourceType { get; set; }
        public string birthDate { get; set; }
        public bool active { get; set; }
        public string gender { get; set; }
        public bool deceasedBoolean { get; set; }
        public string id { get; set; }
        public List<Reference> careProvider { get; set; }
        public List<Name> name { get; set; }
        public List<Identifier> identifier { get; set; }
        public List<Address> address { get; set; }
        public List<Telecom> telecom { get; set; }
        public Property maritalStatus { get; set; }
        public List<Communication> communication { get; set; }
        public List<Extension> extension { get; set; }
    }
}