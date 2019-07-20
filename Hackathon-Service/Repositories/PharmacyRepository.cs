using Hackathon_DataAccess;
using Hackathon_Service.Models.Users.Requests;
using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hackathon_Service.Models.Users.Responses;

namespace Hackathon_Service.Repositories
{
    public class PharmacyRepository
    {
        private UserRepository userRepository;

        public PharmacyRepository()
        {
            userRepository = new UserRepository();
        }
        //public Patient checkIfPatientExists(int userId)
        //{
        //    using (var context = new HackathonEntities())
        //    {
        //        return context.Patients.FirstOrDefault(x => x.UserId.Equals(userId));
        //    }
        //}

        public void createNewUserPharmacy(PharmacyCreationRequest request)
        {
            userRepository.createNewUser(request);
            var user = userRepository.getUserInfo(request.email);
            createNewPharmacy(request, user.id);
        }

        public void createNewPharmacy(PharmacyCreationRequest request, int userId)
        {
            using (var context = new HackathonEntities())
            {
                var pharmacy = new Pharmacy()
                {
                    UserId = userId,
                    Location = request.Location,
                    Name = request.Name
                };
                context.Pharmacies.Add(pharmacy);
                context.SaveChanges();
                context.Dispose();
            }
        }

        //public Patient getPatientInfo(int userId)
        //{
        //    using (var context = new HackathonEntities())
        //    {
        //        var patient = context.Patients.FirstOrDefault(x => x.UserId == userId);
        //        context.Dispose();
        //        return patient;
        //    }
        //}

        //public PatientDataResponse getAllPatientData(string userEmail)
        //{
        //    var user = userRepository.getUserInfo(userEmail);
        //    var patientInfo = getPatientInfo(user.id);
        //    var response = new PatientDataResponse()
        //    {
        //        id = user.id,
        //        firstName = user.first_name,
        //        lastName = user.last_name,
        //        email = user.email,
        //        DOB = patientInfo.DOB,
        //        Gender = patientInfo.Gender
        //    };
        //    return response;
        //}
    }
}