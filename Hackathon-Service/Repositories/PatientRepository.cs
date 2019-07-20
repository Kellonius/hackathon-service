﻿using Hackathon_DataAccess;
using Hackathon_Service.Models.Users.Requests;
using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hackathon_Service.Models.Users.Responses;

namespace Hackathon_Service.Repositories
{
    public class PatientRepository
    {
        private UserRepository userRepository;

        public PatientRepository()
        {
            userRepository = new UserRepository();
        }
        public Patient checkIfPatientExists(int userId)
        {
            using (var context = new HackathonEntities())
            {
                return context.Patients.FirstOrDefault(x => x.UserId.Equals(userId));
            }
        }

        public void createNewUserPatient(PatientCreationRequest request)
        {
            userRepository.createNewUser(request);
            var user = userRepository.getUserInfo(request.email);
            createNewPatient(request, user.id);
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
                var patient = context.Patients.FirstOrDefault(x => x.UserId == userId);
                context.Dispose();
                return patient;
            }
        }

        public PatientDataResponse getAllPatientData(string userEmail)
        {
            var user = userRepository.getUserInfo(userEmail);
            var patientInfo = getPatientInfo(user.id);
            var response = new PatientDataResponse()
            {
                id = user.id,
                firstName = user.first_name,
                lastName = user.last_name,
                email = user.email,
                DOB = patientInfo.DOB,
                Gender = patientInfo.Gender
            };
            return response;
        }
    }
}