using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hackathon_Service.Models.Users.Requests
{
    public class PatientCreationRequest : UserCreationRequest
    {
        public int PatientId { get; set; }
        public int UserId { get; set; }
        public DateTime? DOB { get; set; }
        public string Gender { get; set; }
        public bool AtRisk { get; set; }
        public string SocialSecurity { get; set; }
        public int MPId { get; set; }
    }
}