using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Hackathon_DataAccess;
using Hackathon_Service.Models;
using Hackathon_Service.Models.Users.Requests;
using Hackathon_Service.Repositories;
using Hackathon_Service.Models.Epic;
using System.Collections.Generic;
using Hackathon_Service.Models.Users.Responses;

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

        /// <summary>
        /// Pass in an existing user Id to make that user a Medical Professional.
        /// </summary>
        /// <param name="userId"></param>
        [Route("CreateMedicalProfessionalWithExistingUserId")]
        public void createMedicalProfessionalWithExistingUserId(int userId)
        {
            medicalProfessionalRepository.CreateMedicalProfessional(userId);
        }

        /// <summary>
        /// As a medical professional, pass a PatientCreationRequest to create a new user/patient and tie that patient to you as an MP.
        /// PatientCreationRequest
        /// </summary>
        /// <param name="patientRequest"></param>
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

        /// <summary>
        /// Get all data on a Medical Professional by passing in the user email.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMedicalProfessionalData")]
        public MedicalProfessionalDataResponse GetMedicalProfessionalData(string userEmail)
        {
            return medicalProfessionalRepository.getAllMedicalProfessionalData(userEmail);
        }

        /// <summary>
        /// Assign an existing patient to an MP by passing in the MPId and the PatientId
        /// </summary>
        /// <param name="medicalProfessionalId"></param>
        /// <param name="patientId"></param>
        [Route("AssignPatientToMp")]
        public void assignPatientToMp(int medicalProfessionalId, int patientId)
        {
            medicalProfessionalRepository.assignPatientToMp(medicalProfessionalId, patientId);
        }

        /// <summary>
        /// Get all patients associated to a medical professional by passing in that MP's email address.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPatientsForMP")]
        public List<PatientDataResponse> getPatientsForMP(string userEmail)
        {
            var medicalProfessionalData = medicalProfessionalRepository.getAllMedicalProfessionalData(userEmail);
            var results = medicalProfessionalRepository.getPatientsForMP(medicalProfessionalData.MPId);
            return results;
        }

        /// <summary>
        /// Get a list of all existing patients that aren't associated to your medical professional by passing the MP email address
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetExistingNewPatientsForMP")]
        public List<PatientDataResponse> getExistingNewPatientsForMP(string userEmail)
        {
            var medicalProfessionalData = medicalProfessionalRepository.getAllMedicalProfessionalData(userEmail);
            var results = medicalProfessionalRepository.getExistingNewPatientsForMP(medicalProfessionalData.MPId);
            return results;
        }

        /// <summary>
        /// Assign a medication to a specified patient assigned to your MP by passing in a script request.
        /// </summary>
        /// <param name="request"></param>
        [Route("AssignMedication")]
        public void assignMedication(ScriptRequest request)
        {
            var response = new HttpResponseMessage();
            try
            {
                medicalProfessionalRepository.assignMedication(request);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(response);
            }
        }

        /// <summary>
        /// Give Medical Professional ability to fill medication if needed.
        /// </summary>
        /// <param name="scriptId"></param>
        [Route("FillMedication")]
        public void fillMedication(int scriptId)
        {
            medicalProfessionalRepository.fillMedication(scriptId);
        }

    }
}