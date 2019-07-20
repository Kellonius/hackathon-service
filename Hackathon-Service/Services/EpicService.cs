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

        public MedicationOrder GetMedicationOrderByPatient(string patientId)
        {
            return service.Get<MedicationOrder>($"MedicationOrder?patient={patientId}");
        }
    }
}