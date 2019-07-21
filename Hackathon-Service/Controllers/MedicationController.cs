using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web.Http;
using System.Web.Http.Cors;
using Hackathon_DataAccess;
using Hackathon_Service.Models;
using Hackathon_Service.Models.Medication;
using Hackathon_Service.Reports;
using Hackathon_Service.Repositories;

namespace Hackathon_Service.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("Medication")]
    public class MedicationController : ApiController
    {
        private List<string> Months = new List<string>()
        {
            "January", "February", "March", "April", "May", "June", "July", "August", "September", "October",
            "November", "December"
        };

        private MedicationRepository _medicationRepository;

        public MedicationController()
        {
            _medicationRepository = new MedicationRepository();
        }

        /// <summary>
        /// Get all medications by patient by passing in a MedicationDataRequest
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetMedications")]
        public List<MedicationModel> GetMedications(MedicationDataRequest request)
        {
            return _medicationRepository.GetMedicationByPatient(request.patientId).Select(m => new MedicationModel(m))
                .ToList();
        }

        [HttpGet]
        [Route("GetMedicationsWithDosages")]
        public List<MedicationDosage> GetMedicationsWithDosages(int patientId)
        {
            return _medicationRepository.GetMedicationDosagesByPatient(patientId).ToList();
        }

        /// <summary>
        /// Get the specific prescriptions for a patient by passing the medications and patient info in the MedicaitonPrescriptionRequest
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetMedicationPrescriptions")]
        public List<ScriptModel> GetMedicationPrescriptions(MedicationPrescriptionRequest request)
        {
            return _medicationRepository.GetScriptsByPatientAndMedication(request)
                .Select(m => new ScriptModel(m)).ToList();
        }

        /// <summary>
        /// Get all incoming prescriptions for givent patient.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetIncomingPrescriptions")]
        public List<PatientScript> GetIncomingPrescriptions()
        {
            return _medicationRepository.GetIncomingPrescriptions();
        }

        /// <summary>
        /// Add an over the counter medication to your list of scripts.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddMedication")]
        public IHttpActionResult AddMedication(MedicationAddRequest request)
        {
            try
            {
                _medicationRepository.AddMedication(request);
                return Ok("success");
            }
            catch
            {
                return BadRequest("failed");
            }
                
        }

        /// <summary>
        /// Get all outgoing prescriptions for a patient.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetOutgoingPrescriptions")]
        public List<PatientScript> GetOutgoingPrescriptions()
        {
            return _medicationRepository.GetOutgoingPrescriptions();
        }

        /// <summary>
        /// Mark your prescription as filled.
        /// </summary>
        /// <param name="prescriptionIds"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("MarkPrescriptionsAsFilled")]
        public bool MarkPrescriptionsAsFilled(List<int> prescriptionIds)
        {
            try
            {
                _medicationRepository.MarkPrescriptionsAsFilled(prescriptionIds);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Mark an individual prescription as picked up.
        /// </summary>
        /// <param name="prescriptionIds"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("MarkPrescriptionsAsPickedUp")]
        public bool MarkPrescriptionsAsPickedUp(List<int> prescriptionIds)
        {
            try
            {
                _medicationRepository.MarkPrescriptionsAsPickedUp(prescriptionIds);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Mark a prescription as being unpicked after a month of time has gone by for reporting purposes.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("UnpickedUpPrescriptionsByMonth")]
        public List<MonthlyReport> UnpickedUpPrescriptionsByMonth()
        {
            var scripts = _medicationRepository.GetPrescriptions().Where(s => s.DateIssued.HasValue)
                .OrderBy(s => s.DateIssued).ToList();
            var monthlyReports = new List<MonthlyReport>();

            foreach (var script in scripts)
            {
                var monthInt = script.DateIssued.Value.Month;
                var month = Months[monthInt - 1];
                var year = script.DateIssued.Value.Year;

                if (monthlyReports.Any(m => m.Month == month && m.Year == year.ToString()))
                {
                    continue;
                }

                var scriptsForMonth = scripts.Where(s =>
                    s.DateIssued >= new DateTime(year, monthInt, 1) &&
                    s.DateIssued < new DateTime(year, monthInt, 1).AddMonths(1));

                monthlyReports.Add(new MonthlyReport()
                {
                    Month = month,
                    Year = year.ToString(),
                    PickedUpPrescriptions = scriptsForMonth.Count(s => s.DatePickedUp != null),
                    UnPickedUpPrescriptions = scriptsForMonth.Count(s => s.DatePickedUp == null)
                });
            }

            return monthlyReports;
        }
    }
}