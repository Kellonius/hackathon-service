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

        [HttpPost]
        [Route("GetPatientData")]
        public PatientDataResponse GetPatientData(PatientDataRequest patient)
        {
            return patientRepository.getAllPatientData(patient.userEmail);
        }

        [Route("PatientPickedUpMedication")]
        public void PatientPickedUpMedication(int scriptId)
        {
            patientRepository.patientPickedUpMedication(scriptId);
        }

        [HttpGet]
        [Route("SearchPatients")]
        public List<PatientDataResponse> SearchPatients(string terms)
        {
            return patientRepository.SearchPatients(terms);
        }

        [HttpPost]
        [Route("UpdatePatient")]
        public PatientDataResponse UpdatePatient(PatientUpdateRequest request)
        {
            return patientRepository.updatePatient(request);
        }

        //[Route("CreatePatientFromExistingUser")]
        //public void createPatient(int userId)
        //{
        //    var response = new HttpResponseMessage();
        //    try
        //    {

        //        using (var context = new HackathonEntities())
        //        {
                    
        //            patientRepository.(patientRequest);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw new HttpResponseException(response);
        //    }
        //}
    }
}