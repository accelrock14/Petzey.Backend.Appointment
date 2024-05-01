using Petzey.Backend.Appointment.Data;
using Petzey.Backend.Appointment.Domain;
using Petzey.Backend.Appointment.Domain.DTO;
using Petzey.Backend.Appointment.Domain.Entities;
using Petzey.Backend.Appointment.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Petzey.Backend.Appointment.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ReportController : ApiController
    {
        IAppointmentRepository repo;

        public ReportController(IAppointmentRepository _repo)
        {
            repo = _repo;
        }


        // Get the report details for a particular appointment
        // Pass the ReportID
        [HttpGet]
        [Route("api/appointment/report/{reportId}")]
        public IHttpActionResult GetReport(int reportId)
        {
            try
            {
                //Report report = db.AppointmentDetails.Find(id).Report;
                Report report = repo.GetReportByID(reportId);

            if(report == null)
            {
                return NotFound();
            }

            return Ok(report);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }
        }


        // Get the names of all the symptoms present
        [HttpGet]
        [Route("api/appointment/symptoms")]
        public IHttpActionResult GetSymptoms()
        {
            try
            {
                IEnumerable<Symptom> symptoms = repo.GetAllSymptoms();
            return Ok(symptoms);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }
        }


        // Get the names of all the tests available
        [HttpGet]
        [Route("api/appointment/tests")]
        public IHttpActionResult GetTests()
        {
            try
            {
                IEnumerable<Test> tests = repo.GetAllTests();
            return Ok(tests);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }
        }


        // Get the medicine details by id
        // Pass the MedicineID
        [HttpGet]
        [Route("api/appointment/medicine/{medicineId}")]
        public IHttpActionResult GetMedicine(int medicineId)
        {
            try
            {
                Medicine medicine = repo.GetMedicineById(medicineId);
            if (medicine == null)
            {
                return NotFound();
            }
            return Ok(medicine);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }
        }


        // Get recent appointment history of in the pet profile
        // Pass the PetID of the pet
        [HttpGet]
        [Route("api/appointment/recent/{PetID}")]
        public IHttpActionResult GetRecentAppointments(int PetID)
        {
            try
            {
                if (PetID <= 0)
            {
                return BadRequest("Bad Request");
            }
            var recentAppointments = repo.GetRecentAppointmentsByPetID(PetID);

            if(recentAppointments == null)
            {
                return NotFound();
            }

            return Ok(recentAppointments);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }
        }


        // Get all medicines for search bar in prescription
        [HttpGet]
        [Route("api/appointment/medicines")]
        public IHttpActionResult GetAllMedicine()
        {
            try
            {
                var allMedicines = repo.GetAllMedicines();
            return Ok(allMedicines);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }
        }


        // Get report history of a pet
        // Pass the PetID of the pet
        [HttpGet]
        [Route("api/appointment/reporthistory/{PetID}")]
        public IHttpActionResult GetReportHistoryOfThePet(int PetID)
        {
            try
            {
                if (PetID <= 0)
            {
                return BadRequest("Bad Request");
            }
            var mostRecentAppointment = repo.MostRecentAppointmentByPetID(PetID);
            if (mostRecentAppointment == null)
            {
                return NotFound();
            }

            PetReportHistoryDto petReportHistoryDto = new PetReportHistoryDto();
            petReportHistoryDto.HeartRate = mostRecentAppointment.Report.HeartRate;
            petReportHistoryDto.Temperature = mostRecentAppointment.Report.Temperature;
            petReportHistoryDto.OxygenLevel = mostRecentAppointment.Report.OxygenLevel;
            petReportHistoryDto.ScheduleDate=mostRecentAppointment.ScheduleDate;
            petReportHistoryDto.Symptoms = mostRecentAppointment.Report.Symptoms;
            petReportHistoryDto.Tests = mostRecentAppointment.Report.Tests;
            petReportHistoryDto.Prescriptions = repo.GetHistoryOfPrescriptionsByPetID(PetID);

            return Ok(petReportHistoryDto);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }
        }


        [HttpGet]
        [Route("api/appointment/prescription/{id}")]
        public IHttpActionResult GetPrescription(int id)
        {
            PrescribedMedicine prescribedMedicine = repo.GetPrescribed(id);
            return Ok(prescribedMedicine);
        }


        // Add a new Medicine to the list in Prescription
        // Pass the PrescriptionID of the prescription to which you want to add the PrescribedMedicine
        [HttpPost]
        [Route("api/appointment/prescription/{prescriptionId}")]
        public IHttpActionResult AddMedicine(int prescriptionId, PrescribedMedicine prescribedMedicine)
        {
            try
            {
                if (prescribedMedicine == null)
            {
                return BadRequest("invalid medicine data");
            }
            repo.AddMedicineToPrescription(prescriptionId, prescribedMedicine);
            return Created("location",prescribedMedicine.PrescribedMedicineID);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();


            }
        }

/*
        // Add a symptom to the report
        // Pass the ReportId of the report to which you want to add the symptom
        [HttpPost]
        [Route("api/appointment/reportsymptom/{reportID}")]
        public IHttpActionResult AddSymptomToReport(int reportID, ReportSymptom reportSymptom)
        {
            try
            {
                repo.AddSymptomToReport(reportID, reportSymptom);
                return Created("location", reportSymptom.ReportSymptomID);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }
        }


        // Add a test to the report
        // Pass the ReportId of the report to which you want to add the test
        [HttpPost]
        [Route("api/appointment/reporttest/{reportID}")]
        public IHttpActionResult AddTestToReport(int reportID, ReportTest reportTest)
        {
            try
            {
                repo.AddTestToReport(reportID, reportTest);
                return Created("location", reportTest.ReportTestID);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }
        }


        // Add a recommended doctor to the report
        // Pass the ReportId of the report to which you want to add the recommendation
        [HttpPost]
        [Route("api/appointment/recommendation/{reportID}")]
        public IHttpActionResult AddRecommendationToReport(int reportID, RecommendedDoctor recommendedDoctor)
        {
            try
            {
                repo.AddDoctorRecommendation(reportID, recommendedDoctor);
                return Created("location", recommendedDoctor.ID);
            }
      /*      }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }
        }*/


            // Edit details in a report for an appointment
        [HttpPut]
        [Route("api/appointment/report")]
        public IHttpActionResult PutEditReport([FromBody] Report report)
        {
            try
            {
                if (report == null)
            {
                return BadRequest("Missing data to put");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid report details");
            }

            repo.EditReport(report);

            return Ok(report);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }
        }


        // Remove a PrescribedMedicine from the list in Prescription
        // Pass the PrescribedMedicineID of which you want to delete
        [HttpDelete]
        [Route("api/appointment/prescription/{prescribedMedicineId}")]
        public IHttpActionResult DeleteMedicine(int prescribedMedicineId)
        {
            try
            {
                repo.RemoveMedicineFromPrescription(prescribedMedicineId);
            return Ok("deleted successfully");
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }
        }

/*
        // Remove a symptom from the report
        // Pass the reportSymptomID of the symptom you want to remove
        [HttpDelete]
        [Route("api/appointment/reportsymptom/{reportsymptomID}")]
        public IHttpActionResult DeleteSymptomFromReport(int reportSymptomID)
        {
            try
            {
                repo.DeleteSymptomFromReport(reportSymptomID);
            return Ok("deleted successfully");
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }
        }


        // Remove a test from the report
        // Pass the reportTestID of the test you want to remove
        [HttpDelete]
        [Route("api/appointment/reporttest/{reportTestID}")]
        public IHttpActionResult DeleteTestFromReport(int reportTestID)
        {
            try
            {
                repo.DeleteTestFromReport(reportTestID);
            return Ok("deleted successfully");
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }
        }


        // Remove a doctor recommendation from the report
        // Pass the recommendeddoctorID that you want to remove
        [HttpDelete]
        [Route("api/appointment/recommendation/{recommendedDoctorID}")]
        public IHttpActionResult DeleteRecommendationFromReport(int recommendedDoctorID)
        {
            try
            {
                repo.RemoveDoctorRecommendation(recommendedDoctorID);
            return Ok("deleted successfully");
        }*/
          /*  }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }
        }*/


        // Temp api to post a new report
        [HttpPost]
        [Route("api/appointment/report")]
        public IHttpActionResult PostReport(Report report)
        {
            try
            {
                if (report == null)
            {
                return BadRequest("Missing data to patch");
            }
            repo.AddReport(report);
            return Created("location", report.ReportID);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }
        }

        [HttpPatch]
        [Route("api/appointment/report/{id}")]
        public IHttpActionResult PatchReport(int id, [FromBody] Report report)
        {
            if (report == null)
            {
                return BadRequest("Missing data to patch");
            }

            var existingReport = repo.GetReportByID(id);


            if (existingReport == null)
            {

                return BadRequest("canot find the report with this id");

            }

            repo.UpdateReportStatus(existingReport, report);
            return Ok(report);
        }

        [HttpPatch]
        [Route("api/appointment/prescription/{id}")]
        public IHttpActionResult PatchPrescription(int id, [FromBody] PrescribedMedicine prescribedMedicine)
        {
            if (prescribedMedicine == null)
            {
                return BadRequest("Missing data to patch");
            }

            var existingPrescription = repo.GetPrescribed(id);


            if (existingPrescription == null)
            {

                return BadRequest("canot find the prescription with this id");

            }

            repo.UpdateMedicine(existingPrescription, prescribedMedicine);
            return Ok(prescribedMedicine);
        }
    }
}
