﻿using Petzey.Backend.Appointment.Domain;
using Petzey.Backend.Appointment.Domain.DTO;
using Petzey.Backend.Appointment.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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
            //inject repo using unity
            repo = _repo;
        }

        public ReportController()
        {
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
                NotFound();
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


       

        // Get appointment history of a pet
        // Pass the PetID of the pet
        [HttpGet]
        [Route("api/appointment/appointmenthistory/{PetID}")]
        public IHttpActionResult GetAppointmentHistoryOfThePet(int PetID)
        {
            try
            {
            if (PetID <= 0)
            {
                return BadRequest("Bad Request");
            }


            return Ok(repo.GetAppointmentHistoryByPetID(PetID));

            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }

        }



        // Get the prescribed medicine data
        // Pass the PrescribedMedicineID
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

        // Edit the details in report (vitals, symptoms, tests, recommencded doctors and comment)
        // Pass the reportID
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


        // Update the details of a prescribed medicine
        // Pass the id of the prescribed medicine
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

        [HttpDelete]
        [Route("api/appointment/doctor/{recommendedDoctorID}")]
        public IHttpActionResult DeleteDoctorRecommendation(int recommendedDoctorID)
        {
            try
            {
                repo.RemoveDoctorRecommendation(recommendedDoctorID);
                return Ok("deleted successfully");
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }
        }

        [HttpPut]
        [Route("api/appointment/doctor")]
        public IHttpActionResult PutDoctorRecommendation([FromBody] RecommendedDoctor recommendedDoctor)
        {
            try
            {
                if (recommendedDoctor == null)
                {
                    return BadRequest("Missing data to put");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid report details");
                }

                repo.UpdateDoctorRecommendation(recommendedDoctor);

                return Ok(recommendedDoctor);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }
        }

        [HttpPost]
        [Route("api/appointment/doctor/{id}")]
        public IHttpActionResult PostDoctorRecommendation(int id, RecommendedDoctor recommendedDoctor)
        {
            try
            {
                if (recommendedDoctor == null)
                {
                    return BadRequest("Missing data to patch");
                }
                repo.AddDoctorRecommendation(id, recommendedDoctor);
                return Created("location", recommendedDoctor.ID);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }
        }
    }
}
