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

namespace Petzey.Backend.Appointment.API.Controllers
{
    public class ReportController : ApiController
    {
        IAppointmentRepository repo;

        public ReportController(IAppointmentRepository _repo)
        {
            repo = _repo;
        }

        public ReportController()
        {
        }

        // Get the report details for a particular appointment
        [HttpGet]
        [Route("api/appointment/report/{id}")]
        public IHttpActionResult GetReport(int id)
        {
            //Report report = db.AppointmentDetails.Find(id).Report;
            Report report = repo.GetReportByID(id);
            return Ok(report);
        }

        // Get the names of all the symptoms present
        [HttpGet]
        [Route("api/appointment/symptom")]
        public IHttpActionResult GetSymptoms()
        {
            IEnumerable<Symptom> symptoms = repo.GetAllSymptoms();
            return Ok(symptoms);
        }

        // Get the names of all the tests available
        [HttpGet]
        [Route("api/appointment/test")]
        public IHttpActionResult GetTests()
        {
            IEnumerable<Test> tests = repo.GetAllTests();
            return Ok(tests);
        }

        // Edit details in a report for an appointment
        [HttpPut]
        [Route("api/appointment/report")]
        public IHttpActionResult PutEditReport([FromBody] Report report)
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


        [HttpGet]
        [Route("api/appointment/medicine/{id}")]
        public IHttpActionResult GetMedicine(int id)
        {
            Medicine medicine = repo.GetMedicineById(id);
            if (medicine == null)
            {
                return NotFound();
            }
            return Ok(medicine);
        }

        [HttpDelete]
        [Route("api/appointment/prescription/{id}")]
        public IHttpActionResult DeleteMedicine(int id)
        {
            repo.RemoveMedicineFromPrescription(id);
            return Ok();
        }

        [HttpPost]
        [Route("api/appointment/prescription/{id}")]
        public IHttpActionResult AddMedicine(int id, PrescribedMedicine medicine)
        {
            if (medicine == null)
            {
                return BadRequest("invalid medicine data");
            }
            repo.AddMedicineToPrescription(id, medicine);
            return Ok(medicine.PrescribedMedicineID);
        }


        // Temp api to post a new report
        [HttpPost]
        [Route("api/appointment/report")]
        public IHttpActionResult PostReport(Report report)
        {
            if (report == null)
            {
                return BadRequest("Missing data to patch");
            }
            repo.AddReport(report);
            return Created("location", report.ReportID);
        }









        //get recent appointment history of in the pet profile
        [HttpGet]
        [Route("api/appointment/recent/{PetID}")]
        public IHttpActionResult GetRecentAppointments(int PetID)
        {
            if (PetID <= 0)
            {
                return BadRequest("Bad Request");
            }
            var recentAppointments = repo.GetRecentAppointmentsByPetID(PetID);
            return Ok(recentAppointments);
        }


        //get all medicines for search bar in prescription
        [HttpGet]
        [Route("api/appointment/medicine")]
        public IHttpActionResult GetAllMedicine()
        {
            var allMedicines = repo.GetAllMedicines();
            return Ok(allMedicines);
        }



        //get report history of a pet
        [HttpGet]
        [Route("api/appointment/reporthistory/{PetID}")]
        public IHttpActionResult GetReportHistoryOfThePet(int PetID)
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
            petReportHistoryDto.Symptoms = mostRecentAppointment.Report.Symptoms;
            petReportHistoryDto.Tests = mostRecentAppointment.Report.Tests;
            petReportHistoryDto.Prescriptions = repo.GetHistoryOfPrescriptionsByPetID(PetID);

            return Ok(petReportHistoryDto);
        }
    }
}




/*
 trial object

            Report r = new Report();
            Prescription p = new Prescription();
            PrescribedMedicine p1 = new PrescribedMedicine();
            PrescribedMedicine p2 = new PrescribedMedicine();
            p1.Medicine = new Medicine();
            p2.Medicine = new Medicine();
            p.PrescribedMedicines = new List<PrescribedMedicine> { p1, p2 };
            r.Prescription = p;
            r.Symptoms = new List<Symptom>();
            r.Tests = new List<Test>();
            r.Symptoms.Add(new Symptom());
            r.Tests.Add(new Test());


// Temp api to post a new report
        [HttpPost]
        [Route("api/appointment/report")]
        public IHttpActionResult PostReport(Report report)
        {
            if (report == null)
            {
                return BadRequest("Missing data to patch");
            }
            db.Reports.Add(report);
            db.SaveChanges();
            return Created("location", report.ReportID);
        }
 */