using Hackathon_DataAccess;
using Hackathon_Service.Models.Users.Requests;
using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hackathon_Service.Repositories
{
    public class MedicalProfessionalRepository
    {
        private UserRepository userRepository;
        private PatientRepository patientRepository;
        public MedicalProfessionalRepository()
        {
            userRepository = new UserRepository();
            patientRepository = new PatientRepository();
        }

        public void createNewUserMedicalProfessional(MedicalProfessionalRequest request)
        {
            userRepository.createNewUser(request);
            var user = userRepository.getUserInfo(request.email);
            createNewMedicalProfessional(request, user.id);
        }

        public void CreateMedicalProfessional(int userId)
        {
            using (var context = new HackathonEntities())
            {
                var medicalProf = new MedicalProfessional()
                {
                    UserId = userId
                };
                context.MedicalProfessionals.Add(medicalProf);
                context.SaveChanges();
                context.Dispose();
            }
        }

        public void createNewMedicalProfessional(MedicalProfessionalRequest request, int userId)
        {
            using (var context = new HackathonEntities())
            {
                var medicalProfessional = new MedicalProfessional()
                {
                    UserId = userId,
                    Address = request.Address,
                    Email = request.email,
                    Phone = request.Phone
                };
                context.MedicalProfessionals.Add(medicalProfessional);
                context.SaveChanges();
                context.Dispose();
            }
        }

        public void CreateNewPatientAndMPRecord(PatientCreationRequest patientRequest)
        {
            using (var context = new HackathonEntities())
            {
                userRepository.createNewUser(patientRequest);
                var user = userRepository.getUserInfo(patientRequest.email);
                patientRepository.createNewPatient(patientRequest, user.id);
                var patient = patientRepository.getPatientInfo(user.id);
                var mpToPatient = new MpToPatient()
                {
                    MPId = patientRequest.MPId,
                    PatientId = patient.PatientId
                };
                context.MpToPatients.Add(mpToPatient);
                context.SaveChanges();
                context.Dispose();
            }
        }

    }
}