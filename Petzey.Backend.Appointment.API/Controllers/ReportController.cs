using Petzey.Backend.Appointment.Data;
using Petzey.Backend.Appointment.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData;

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
        public IHttpActionResult PutEditReport([FromBody] Delta<Report> report)
        {
            if (report == null)
            {
                return BadRequest("Missing data to patch");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid report details");
            }

            db.Entry(report).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Ok("report updated");
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