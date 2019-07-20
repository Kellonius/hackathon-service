using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hackathon_Service.Models.Users.Responses
{
    public class MedicalProfessionalDataResponse : UserResponse
    {
        public int MPId { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}