using Hackathon_DataAccess;
using Hackathon_Service.Models.Users.Requests;
using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hackathon_Service.Models.Users.Responses;
using Hackathon_Service.Models;
using Hackathon_Service.Models.Users;

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
                    AtRisk = request.AtRisk,
                    SocialSecurity = request.SocialSecurity,
                    UserId = userId
                };
                context.Patients.Add(patient);
                context.SaveChanges();
                context.Dispose();
            }
        }

        internal void patientPickedUpMedication(int scriptId)
        {
            using (var context = new HackathonEntities())
            {
                var script = context.Scripts.FirstOrDefault(x => x.ScriptId == scriptId);
                script.DatePickedUp = DateTime.Now;
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

        public PatientDataResponse GetPatientDataById(int patientId)
        {
            using(var context = new HackathonEntities())
            {
                var patient = context.Patients.FirstOrDefault(p => p.PatientId == patientId);
                var user = patient.user;
                var patientScripts = getPatientScripts(patientId);
                return new PatientDataResponse()
                {
                    id = user.id,
                    firstName = user.first_name,
                    lastName = user.last_name,
                    email = user.email,
                    AtRisk = patient.AtRisk.Value ? "Yes" : "No",
                    DOB = patient.DOB.Value.ToString("MM-dd-yyyy"),
                    Gender = patient.Gender,
                    Scripts = patientScripts
                };
            }
        }

        public PatientDataResponse getAllPatientData(string userEmail)
        {
            var user = userRepository.getUserInfo(userEmail);
            var patientInfo = getPatientInfo(user.id);
            var patientScripts = getPatientScripts(patientInfo.PatientId);
            var response = new PatientDataResponse()
            {
                id = user.id,
                firstName = user.first_name,
                lastName = user.last_name,
                email = user.email,
                AtRisk = patientInfo.AtRisk.Value ? "Yes" : "No",
                DOB = patientInfo.DOB.Value.ToString("MM-dd-yyyy"),
                Gender = patientInfo.Gender,
                Scripts = patientScripts
            };
            return response;
        }

        public List<PatientDataResponse> SearchPatients(string terms)
        {
            using (var context = new HackathonEntities())
            {
                var response = new List<PatientDataResponse>();

                var patientInfo = context.Patients.Where(p =>
                    (p.SocialSecurity != null && p.SocialSecurity.ToLower().Contains(terms)) ||
                    (p.SocialSecurity != null && p.SocialSecurity.Replace("-", "").ToLower().Contains(terms)) ||
                    (p.user.first_name != null && p.user.first_name.ToLower().Contains(terms)) ||
                    (p.user.last_name != null && p.user.last_name.ToLower().Contains(terms)) ||
                    (p.user.email != null && p.user.email.ToLower().Contains(terms))
                ).ToList();
                var patientScripts = patientInfo.Select(pi => getPatientScripts(pi.PatientId)).ToList();
                for (var i = 0; i < patientInfo.Count; ++i)
                {
                    var pi = patientInfo[i];
                    var ps = patientScripts[i];
                    var user = pi.user;

                    response.Add(
                        new PatientDataResponse()
                        {
                            id = pi.PatientId,
                            firstName = user.first_name,
                            lastName = user.last_name,
                            email = user.email,
                            AtRisk = pi.AtRisk.Value ? "Yes" : "No",
                            DOB = pi.DOB.Value.ToString("MM-dd-yyyy"),
                            Gender = pi.Gender,
                            Scripts = ps
                        }
                    );
                }

                return response;
            }
        }

        public List<ScriptModel> getPatientScripts(int? patientId)
        {
            using (var context = new HackathonEntities())
            {
                var scriptIds = context.Scripts.Where(x => x.PatientId == patientId).Select(x => x.ScriptId);
                var scripts = new List<ScriptModel>();
                foreach (var id in scriptIds)
                {
                    var script = context.Scripts.FirstOrDefault(x => x.ScriptId == id);
                    var medicalProfessional = context.MedicalProfessionals.FirstOrDefault(x => x.MPId == script.MPId);
                    var medicalProfessionalUser = context.users.FirstOrDefault(x => x.id == medicalProfessional.UserId);
                    var medication = context.Medications.FirstOrDefault(x => x.MedicationId == script.MedicationId);
                    scripts.Add(new ScriptModel(script)
                    {
                        PrescribedBy = "Dr. " + medicalProfessionalUser.first_name + " " +
                                       medicalProfessionalUser.last_name,
                        Phone = medicalProfessional.Phone,
                        Email = medicalProfessional.Email,
                        MedicationGenericName = medication.GenericName,
                        MedicationMedicalName = medication.MedicalName
                    });
                }

                context.Dispose();
                return scripts;
            }
        }

        public Patient getPatientDataFromId(int? patientId)
        {
            using (var context = new HackathonEntities())
            {
                var patient = context.Patients.FirstOrDefault(x => x.PatientId == patientId);
                context.Dispose();
                return patient;
            }
        }

        public PatientDataResponse updatePatient(PatientUpdateRequest request)
        {
            using (var context = new HackathonEntities())
            {
                var user = context.users.FirstOrDefault(x => x.id == request.userId);
                user.first_name = request.firstName;
                user.last_name = request.lastName;
                user.email = request.email;

                var patient = context.Patients.FirstOrDefault(x => x.UserId == request.userId);
                patient.Gender = request.Gender;
                patient.DOB = DateTime.Parse(request.DOB);

                context.SaveChanges();
                context.Dispose();

                return getAllPatientData(request.email);
            }
        }
    }
}