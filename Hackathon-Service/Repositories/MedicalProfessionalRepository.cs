﻿using Hackathon_DataAccess;
using Hackathon_Service.Models.Users.Requests;
using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hackathon_Service.Models.Users.Responses;
using Hackathon_Service.Models;

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
                    var scripts = patientRepository.getPatientScripts(patientId);
                    var result = new PatientDataResponse()
                    {
                        id = user.id,
                        PatientId = patient.PatientId,
                        firstName = user.first_name,
                        lastName = user.last_name,
                        email = user.email,
                        AtRisk = patient.AtRisk.Value ? "Yes" : "No",
                        DOB = patient.DOB.Value.ToString("MM-dd-yyyy"),
                        Gender = patient.Gender,
                        Scripts = scripts
                    };
                    results.Add(result);
                };
                return results;
            }
        }

        public List<PatientDataResponse> getExistingNewPatientsForMP(int medicalProfessionalId)
        {

            using (var context = new HackathonEntities())
            {
                var currentPatientIdList = context.MpToPatients.Where(x => x.MPId == medicalProfessionalId).Select(x => x.PatientId).ToList();
                var newPatientIdList = context.Patients.Where(x => !currentPatientIdList.Contains(x.PatientId)).Select(x => x.PatientId).ToList();
                var results = new List<PatientDataResponse>();
                foreach (var patientId in newPatientIdList)
                {
                    var patient = patientRepository.getPatientDataFromId(patientId);
                    var user = userRepository.getUserInfoFromId(patient.UserId);
                    var result = new PatientDataResponse()
                    {
                        id = user.id,
                        PatientId = patient.PatientId,
                        firstName = user.first_name,
                        lastName = user.last_name,
                        email = user.email,
                        AtRisk = patient.AtRisk.Value ? "Yes" : "No",
                        DOB = patient.DOB.Value.ToString("MM-dd-yyyy"),
                        Gender = patient.Gender,
                    };
                    results.Add(result);
                };
                return results;
            }
        }

        internal void fillMedication(int scriptId)
        {
            using (var context = new HackathonEntities())
            {
                var script = context.Scripts.FirstOrDefault(x => x.ScriptId == scriptId);
                if(script != null)
                {
                    script.DateFilled = DateTime.Now;
                    context.SaveChanges();
                }

            }
        }

        public void assignMedication(ScriptRequest request)
        {
            using (var context = new HackathonEntities())
            {
                var medication = new Medication()
                {
                    GenericName = request.MedicationGenericName,
                    MedicalName = request.MedicationMedicalName
                };
                context.Medications.Add(medication);
                context.SaveChanges();

                medication = context.Medications.FirstOrDefault(x => x.GenericName == request.MedicationGenericName &&
                    x.MedicalName == request.MedicationMedicalName);
                var script = new Script()
                {
                    MPId = request.medicalProfessionalId,
                    PatientId = request.patientId,
                    MedicationId = medication.MedicationId,
                    PharmId = request.pharmacyId,
                    Dosage = request.Dosage,
                    MedicationTime = request.MedicationTime,
                    MedicationRoute = request.MedicationRoute,
                    DateIssued = DateTime.Now

                };
                context.Scripts.Add(script);
                context.SaveChanges();
                context.Dispose();
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