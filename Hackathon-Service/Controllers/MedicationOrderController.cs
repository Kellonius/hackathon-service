using System;
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
    [RoutePrefix("MedicationOrder")]
    public class MedicationOrderController : ApiController
    {
        private EpicService _epicService;

        public MedicationOrderController()
        {
            _epicService = new EpicService();
        }
 
        [HttpGet]
        [Route("order")]
        public MedicationOrder MedicationOrder(string patientId)
        {
            return _epicService.GetMedicationOrderByPatient(patientId);
        }
    }
}