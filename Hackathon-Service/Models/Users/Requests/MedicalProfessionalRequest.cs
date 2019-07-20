using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hackathon_Service.Models.Users.Requests
{
    public class MedicalProfessionalRequest : UserCreationRequest
    {
        public int MPId { get; set; }
        public int UserId { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}