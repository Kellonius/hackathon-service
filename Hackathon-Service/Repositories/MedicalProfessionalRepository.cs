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
        private UserRespository userRespository;
        private PatientRespository patientRepository;
        public MedicalProfessionalRepository()
        {
            userRespository = new UserRespository();
            patientRepository = new PatientRespository();
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