using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Hackathon_DataAccess;
using Hackathon_Service.Models;
using Hackathon_Service.Models.Users.Requests;
using Hackathon_Service.Repositories;

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

        [Route("CreateMedicalProfessional")]
        public void createMedicalProfessional(int userId)
        {
            medicalProfessionalRepository.CreateMedicalProfessional(userId);
        }


        [Route("CreatePatient")]
        public void createPatient(PatientCreationRequest patientRequest)
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