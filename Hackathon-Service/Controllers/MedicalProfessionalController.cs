using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Hackathon_DataAccess;
using Hackathon_Service.Models;
using Hackathon_Service.Models.Users.Requests;
using Hackathon_Service.Repositories;
using Hackathon_Service.Repositories.Interfaces;
using Hackathon_Service.Models.Epic;
using System.Collections.Generic;

namespace Hackathon_Service.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("MedicalProfessionals")]
    public class MedicalProfessionalController : ApiController
    {
        private UserRepository userRespository;
        private MedicalProfessionalRepository medicalProfessionalRepository;

        public MedicalProfessionalController()
        {
            userRespository = new UserRepository();
            medicalProfessionalRepository = new MedicalProfessionalRepository();
        }

        [Route("CreateMedicalProfessionalWithExistingUserId")]
        public void createMedicalProfessionalWithExistingUserId(int userId)
        {
            medicalProfessionalRepository.CreateMedicalProfessional(userId);
        }


        [Route("CreatePatientAndTieToMP")]
        public void createPatientAndTieToMP(PatientCreationRequest patientRequest)
        {
            var response = new HttpResponseMessage();
            try
            {

                using (var context = new HackathonEntities())
                {
                    var exists = userRespository.checkIfUserExists(patientRequest.email);

                    if (exists != null && exists.email.Length > 0)
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                            "An account with this email already exists.");
                        throw new HttpResponseException(response);
                    }
                    else
                    {
                        medicalProfessionalRepository.CreateNewPatientAndMPRecord(patientRequest);
                    }
                }
            }
            catch (Exception e)
            {
                throw new HttpResponseException(response);
            }
        }

        [HttpGet]
        [Route("GetMedicalProfessionalData")]
        public MedicalProfessionalDataResponse GetMedicalProfessionalData(string userEmail)
        {
            return medicalProfessionalRepository.getAllMedicalProfessionalData(userEmail);
        }

        [Route("AssignPatientToMp")]
        public void assignPatientToMp(int medicalProfessionalId, int patientId)
        {
            medicalProfessionalRepository.assignPatientToMp(medicalProfessionalId, patientId);
        }

        [HttpGet]
        [Route("GetPatientsForMP")]
        public List<PatientDataResponse> getPatientsForMP(int medicalProfessionalId)
        {
            var results = medicalProfessionalRepository.getPatientsForMP(medicalProfessionalId);
            return results;
        }

        [Route("AssignMedication")]
        public void assignMedication(int medicalProfessionalId, int patientId, int pharmacyId, ScriptRequest request)
        {
            var response = new HttpResponseMessage();
            try
            {
                using (var context = new HackathonEntities())
                {

                    var script = new Script()
                    {
                        MPId = medicalProfessionalId,
                        PatientId = patientId,
                        MedicationId = request.MedicationId,
                        PharmId = pharmacyId,
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
            catch (Exception e)
            {
                throw new HttpResponseException(response);
            }
        }


    }
}