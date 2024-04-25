using Petzey.Backend.Appointment.Data;
using Petzey.Backend.Appointment.Domain;
using Petzey.Backend.Appointment.Domain.DTO;
using Petzey.Backend.Appointment.Domain.Entities;
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
        PetzeyDbContext db = new PetzeyDbContext();

        // Get the report details for a particular appointment
        [HttpGet]
        [Route("api/appointment/report/{id}")]
        public IHttpActionResult GetReport(int id)
        {
            //Report report = db.AppointmentDetails.Find(id).Report;
            Report report = db.Reports.Find(id);
            return Ok(report);
        }

        // Get the names of all the symptoms present
        [HttpGet]
        [Route("api/appointment/symptom")]
        public IHttpActionResult GetSymptoms()
        {
            IEnumerable<Symptom> symptoms = db.Symptoms.Distinct();
            return Ok(symptoms);
        }

        // Get the names of all the tests available
        [HttpGet]
        [Route("api/appointment/test")]
        public IHttpActionResult GetTests()
        {
            IEnumerable<Test> tests = db.Tests.Distinct();
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
            foreach(PrescribedMedicine medicine in report.Prescription.PrescribedMedicines)
            {
                db.Entry(medicine.Medicine).State = System.Data.Entity.EntityState.Modified;
                db.Entry(medicine).State = System.Data.Entity.EntityState.Modified;
            }
            db.Entry(report).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Ok(report);
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
            db.Reports.Add(report);
            db.SaveChanges();
            return Created("location", report.ReportID);
        }









        //get recent appointment history of in the pet profile
        [HttpGet]
        [Route("api/appointment/recent/{PetID}")]
        public IHttpActionResult GetRecentAppointments(int PetID)
        {
            if (PetID<=0)
            {
                return BadRequest("Bad Request");
            }
            var recentAppointments=db.AppointmentDetails.Where(a=>a.PetID==PetID && a.Status==Status.Closed).OrderByDescending(a=>a.ScheduleDate).Take(10).ToList();
            return Ok(recentAppointments);
        }


        //get all medicines for search bar in prescription
        [HttpGet]
        [Route("api/appointment/medicine")]
        public IHttpActionResult GetAllMedicine()
        {
            var allMedicines=db.Medicines.ToList();
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
            var mostRecentAppointment = db.AppointmentDetails.Where(a => a.PetID == PetID && a.Status == Status.Closed).OrderByDescending(a => a.ScheduleDate).FirstOrDefault();

            if(mostRecentAppointment == null)
            {
                return NotFound();
            }

            PetReportHistoryDto petReportHistoryDto = new PetReportHistoryDto();
            petReportHistoryDto.HeartRate=mostRecentAppointment.Report.HeartRate;
            petReportHistoryDto.Temperature=mostRecentAppointment.Report.Temperature;
            petReportHistoryDto.OxygenLevel=mostRecentAppointment.Report.OxygenLevel;
            petReportHistoryDto.Symptoms=mostRecentAppointment.Report.Symptoms;
            petReportHistoryDto.Tests=mostRecentAppointment.Report.Tests;
            petReportHistoryDto.Prescriptions=db.AppointmentDetails.Where(a=>a.PetID==PetID && a.Status==Status.Closed).Select(a=>a.Report.Prescription).ToList();

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
 */