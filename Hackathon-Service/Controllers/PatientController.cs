using Hackathon_Service.Models.Users.Requests;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http;
using System.Web.Http.Cors;
using Hackathon_DataAccess;
using Hackathon_Service.Models.Users.Responses;
using Hackathon_Service.Repositories;
using Hackathon_Service.Repositories.Interfaces;
using Hackathon_Service.Models.Epic;

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

        [HttpGet]
        [Route("GetPatientData")]
        public PatientDataResponse GetPatientData(string userEmail)
        {
            return patientRepository.getAllPatientData(userEmail);
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