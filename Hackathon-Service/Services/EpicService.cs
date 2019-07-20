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

        public MedicationOrderResponse GetMedicationOrderByPatient(string patientId)
        {
            return service.Get<MedicationOrderResponse>($"MedicationOrder?patient={patientId}");
        }

        public EpicPatient GetPatient(string patientId)
        {
            return service.Get<EpicPatient>($"Patient/{patientId}");
        }

        public Medication GetMedication(string medicationId)
        {
            return service.Get<Medication>($"Medication/{medicationId}");
        }
    }
}