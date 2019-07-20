using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hackathon_Service.Models.Users.Responses
{
    public class PatientDataResponse : UserResponse
    {
        public string Gender { get; set; }
        public DateTime? DOB { get; set; }
    }
}