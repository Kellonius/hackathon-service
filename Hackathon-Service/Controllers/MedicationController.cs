using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using Hackathon_DataAccess;
using Hackathon_Service.Models;
using Hackathon_Service.Repositories;

namespace Hackathon_Service.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("Medication")]
    public class MedicationController : ApiController
    {
        private MedicationRepository _medicationRepository;

        public MedicationController()
        {
            _medicationRepository = new MedicationRepository();
        }

        [HttpGet]
        [Route("GetMedications")]
        public List<MedicationModel> GetMedications(int patientId)
        {
            return _medicationRepository.GetMedicationByPatient(patientId).Select(m => new MedicationModel(m)).ToList();
        }

        [HttpGet]
        [Route("GetMedicationPrescriptions")]
        public List<ScriptModel> GetMedicationPrescriptions(int patientId, int medicationId)
        {
            return _medicationRepository.GetScriptsByPatientAndMedication(patientId, medicationId)
                .Select(m => new ScriptModel(m)).ToList();
        }
    }
}