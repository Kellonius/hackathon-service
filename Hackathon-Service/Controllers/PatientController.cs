using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Hackathon_DataAccess;
using Hackathon_Service.Models;
using Hackathon_Service.Models.Users;
using Hackathon_Service.Models.Users.Responses;
using Hackathon_Service.Repositories;

namespace Hackathon_Service.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("Patient")]
    public class PatientController : ApiController
    {
        private UserRepository userRepository;
        private PatientRepository patientRepository;

        public PatientController()
        {
            userRepository = new UserRepository();
            patientRepository = new PatientRepository();
        }

        /// <summary>
        /// Return patient data by passing in a patient id.
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPatientDataById")]
        public PatientDataResponse GetPatientDataById(int patientId)
        {
            return patientRepository.GetPatientDataById(patientId);
        }

        /// <summary>
        /// Get patient data by passing in the PatientDataRequest.
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetPatientData")]
        public PatientDataResponse GetPatientData(PatientDataRequest patient)
        {
            return patientRepository.getAllPatientData(patient.userEmail);
        }

        /// <summary>
        /// Update script to note that the patient has picked up the prescription as DateTime.Now when passing in the ScriptId
        /// </summary>
        /// <param name="scriptId"></param>
        [Route("PatientPickedUpMedication")]
        public void PatientPickedUpMedication(int scriptId)
        {
            patientRepository.patientPickedUpMedication(scriptId);
        }

        /// <summary>
        /// Search patients based on a set of terms.
        /// </summary>
        /// <param name="terms"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("SearchPatients")]
        public List<PatientDataResponse> SearchPatients(string terms)
        {
            return patientRepository.SearchPatients(terms);
        }

        /// <summary>
        /// Update a specific patient's details by passing in a PatientUpdateRequest
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdatePatient")]
        public PatientDataResponse UpdatePatient(PatientUpdateRequest request)
        {
            return patientRepository.updatePatient(request);
        }
    }
}