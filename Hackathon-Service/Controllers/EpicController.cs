using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using Hackathon_Service.Models.Epic;
using Hackathon_Service.Services;
using Newtonsoft.Json;

namespace Hackathon_Service.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("Epic")]
    public class EpicController : ApiController
    {
        private EpicService _epicService;

        public EpicController()
        {
            _epicService = new EpicService();
        }
 
        [HttpGet]
        [Route("Orders")]
        public MedicationOrderResponse MedicationOrder(string patientId)
        {
            return _epicService.GetMedicationOrderByPatient(patientId);
        }
        
        [HttpGet]
        [Route("Patient")]
        public EpicPatient Patient(string patientId)
        {
            return  _epicService.GetPatient(patientId);
        }
        
        [HttpGet]
        [Route("Medication")]
        public Medication Medication(string medicationId)
        {
            return  _epicService.GetMedication(medicationId);
        }
        
        [HttpGet]
        [Route("CarePlan")]
        public CarePlanResponse CarePlan(string patientId)
        {
            return  _epicService.GetCarePlan(patientId);
        }
    }
}