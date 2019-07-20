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
        private UserRepository userRespository;
        private PatientRepository patientRepository;
        public MedicalProfessionalRepository()
        {
            userRespository = new UserRepository();
            patientRepository = new PatientRepository();
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

        public void CreatePatient(PatientCreationRequest patientRequest)
        {
            using (var context = new HackathonEntities())
            {
                userRespository.createNewUser(patientRequest);
                var user = userRespository.getUserInfo(patientRequest.email);
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