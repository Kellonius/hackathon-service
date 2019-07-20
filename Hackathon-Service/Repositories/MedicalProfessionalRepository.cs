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

        public List<PatientDataResponse> getPatientsForMP(int medicalProfessionalId)
        {
            using (var context = new HackathonEntities())
            {
                var patientIdList = context.MpToPatients.Where(x => x.MPId == medicalProfessionalId).Select(x => x.PatientId).ToList();
                var results = new List<PatientDataResponse>();
                foreach (var patientId in patientIdList)
                {
                    var patient = patientRepository.getPatientDataFromId(patientId);
                    var user = userRepository.getUserInfoFromId(patient.UserId);
                    var result = new PatientDataResponse()
                    {
                        id = user.id,
                        firstName = user.first_name,
                        lastName = user.last_name,
                        email = user.email,
                        DOB = patient.DOB,
                        Gender = patient.Gender
                    };
                    results.Add(result);
                };
                return results;
            }
        }

        public void assignPatientToMp(int medicalProfessionalId, int patientId)
        {
            using (var context = new HackathonEntities())
            {
                var mpToPatient = new MpToPatient()
                {
                    MPId = medicalProfessionalId,
                    PatientId = patientId
                };
                context.MpToPatients.Add(mpToPatient);
                context.SaveChanges();
                context.Dispose();
            }
        }
        public MedicalProfessional getMedicalProfessionalData(int userId)
        {
            using (var context = new HackathonEntities())
            {
                var medicalProfessional = context.MedicalProfessionals.FirstOrDefault(x => x.UserId == userId);
                context.Dispose();
                return medicalProfessional;
            }
        }

        public MedicalProfessionalDataResponse getAllMedicalProfessionalData(string userEmail)
        {
            var user = userRepository.getUserInfo(userEmail);
            var mpInfo = getMedicalProfessionalData(user.id);
            var response = new MedicalProfessionalDataResponse()
            {
                id = user.id,
                firstName = user.first_name,
                lastName = user.last_name,
                email = user.email,
                Address = mpInfo.Address,
                MPId = mpInfo.MPId,
                Phone = mpInfo.Phone
            };
            return response;
        }
    }
}