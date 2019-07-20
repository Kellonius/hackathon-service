using Hackathon_Service.Models.Epic;

namespace Hackathon_Service.Services
{
    public class EpicService
    {
        private HttpService service;
        
        public EpicService()
        {
            service = new HttpService("https://open-ic.epic.com/FHIR/api/FHIR/DSTU2/");
        }

        public EpicRequest GetMedicationOrderByPatient(string patientId)
        {
            return service.Get<EpicRequest>($"MedicationOrder?patient={patientId}");
        }
    }
}