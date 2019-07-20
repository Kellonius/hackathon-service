using Hackathon_DataAccess;
using Hackathon_Service.Models.Users.Requests;
using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hackathon_Service.Repositories
{
    public class PatientRespository
    {
        public Patient checkIfPatientExists(int userId)
        {
            using (var context = new HackathonEntities())
            {
                return context.Patients.FirstOrDefault(x => x.UserId.Equals(userId));
            }
        }

        public void createNewPatient(PatientCreationRequest request, int userId)
        {
            using (var context = new HackathonEntities())
            {
                var patient = new Patient()
                {
                    DOB = request.DOB,
                    Gender = request.Gender,
                    SocialSecurity = request.SocialSecurity,
                    UserId = userId
                };
                context.Patients.Add(patient);
                context.SaveChanges();
                context.Dispose();
            }
        }

        public Patient getPatientInfo(int userId)
        {
            using (var context = new HackathonEntities())
            {
                var user = context.Patients.FirstOrDefault(x => x.UserId == userId);
                context.Dispose();
                return user;
            }
        }
    }
}