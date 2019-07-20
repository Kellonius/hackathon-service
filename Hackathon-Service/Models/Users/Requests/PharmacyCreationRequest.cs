using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hackathon_Service.Models.Users.Requests
{
    public class PharmacyCreationRequest : UserCreationRequest
    {
        public int PharmacyId { get; set; }
        public int UserId { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }

    }
}