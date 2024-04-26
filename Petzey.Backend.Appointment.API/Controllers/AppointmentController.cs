using Petzey.Backend.Appointment.Data;
using Petzey.Backend.Appointment.Domain.Entities;
using Petzey.Backend.Appointment.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Petzey.Backend.Appointment.API.Controllers
{
    public class AppointmentController : ApiController
    {
        private PetzeyDbContext db = new PetzeyDbContext();

        // GET: api/Appointment
        public IQueryable<AppointmentDetail> GetAppointmentDetails()
        {
            return db.AppointmentDetails;
        }

        // GET: api/Appointment/5
        [ResponseType(typeof(AppointmentDetail))]
        public IHttpActionResult GetAppointmentDetail(int id)
        {
            AppointmentDetail appointmentDetail = db.AppointmentDetails.Find(id);
            if (appointmentDetail == null)
            {
                return NotFound();
            }

            return Ok(appointmentDetail);
        }

        // PUT: api/Appointment/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAppointmentDetail(int id, AppointmentDetail appointmentDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int slot = appointmentDetail.ScheduleTimeSlot;

            int hoursToAdd = 9 + (slot * 30 / 60);  
            int minutesToAdd = (slot * 30) % 60;    

            // if it is the lunch break theen
            if (slot >= 8)
            {
                hoursToAdd += 1;  
            }

            appointmentDetail.ScheduleDate = appointmentDetail.ScheduleDate.Date.AddHours(hoursToAdd).AddMinutes(minutesToAdd);

            if (slot < 0 || slot > 17)
            {
                return BadRequest("Invalid time slot.");
            }

            

            if (id != appointmentDetail.AppointmentID)
            {
                return BadRequest();
            }

            db.Entry(appointmentDetail).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentDetailExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Appointment
        [ResponseType(typeof(AppointmentDetail))]
        public IHttpActionResult PostAppointmentDetail(AppointmentDetail appointmentDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int slot = appointmentDetail.ScheduleTimeSlot;

            int hoursToAdd = 9 + (slot * 30 / 60);
            int minutesToAdd = (slot * 30) % 60;

            // if it is the lunch break theen
            if (slot >= 8)
            {
                hoursToAdd += 1;
            }

            appointmentDetail.ScheduleDate = appointmentDetail.ScheduleDate.Date.AddHours(hoursToAdd).AddMinutes(minutesToAdd);

            if (slot < 0 || slot > 17)
            {
                return BadRequest("Invalid time slot.");
            }



            db.AppointmentDetails.Add(appointmentDetail);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = appointmentDetail.AppointmentID }, appointmentDetail);
        }

        // DELETE: api/Appointment/5
        [ResponseType(typeof(AppointmentDetail))]
        public IHttpActionResult DeleteAppointmentDetail(int id)
        {
            AppointmentDetail appointmentDetail = db.AppointmentDetails.Find(id);
            if (appointmentDetail == null)
            {
                return NotFound();
            }

            db.AppointmentDetails.Remove(appointmentDetail);
            db.SaveChanges();

            return Ok(appointmentDetail);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AppointmentDetailExists(int id)
        {
            return db.AppointmentDetails.Count(e => e.AppointmentID == id) > 0;
        }

        // end point to fetch all list of petissues
        // GET: api/AppointmentDetails
        [HttpGet]

        [Route("api/AppointmentDetails/PetIssues")]
        // sample url https://localhost:44327/api/AppointmentDetails/PetIssues

        public IQueryable<PetIssue> GetAllPetIssues()
        {
            return db.PetIssues;
        }

        // end point for adding a new petissue
        [HttpPost]
        [Route("api/AppointmentDetails/PetIssues")]
        // sample url https://localhost:44327/api/AppointmentDetails/PetIssues
        public IHttpActionResult PostPetIssue(PetIssue petIssue)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PetIssues.Add(petIssue);
            db.SaveChanges();

            //return CreatedAtRoute("DefaultApi", new { id = petIssue.PetIssueID }, petIssue);
            return Ok(petIssue);
        }

        [HttpGet]
        [Route("api/AppointmentDetails/docondate/{doctorId}/{date:datetime}")]
        // sample url https://localhost:44327/api/AppointmentDetails/docondate/1/2024-04-26
        public List<AppointmentDetail> GetAppointmentsOfDocOnDate(int doctorId, DateTime date)
        {
            // Ensuring the date comparison includes only the date part, not the time part
            var dateOnly = date.Date;

            return db.AppointmentDetails
                .Where(a => a.DoctorID == doctorId && DbFunctions.TruncateTime(a.ScheduleDate) == dateOnly)
                .ToList();
        }


        // PATCH: api/AppointmentDetails/5/status
        [HttpPatch]
        [Route("api/AppointmentDetails/{id}/status")]
        [ResponseType(typeof(void))]
        // sample url https://localhost:44327/api/AppointmentDetails/5/status
        // in body send 0,1,2,3 for the required status.
        public IHttpActionResult PatchAppointmentStatus(int id, [FromBody] Status status)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appointment = db.AppointmentDetails.Find(id);
            if (appointment == null)
            {
                return NotFound();
            }

            appointment.Status = status;

            try
            {
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentDetailExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        //get doctor schedule based on date (first get today's schedule)
        //should return the list of booleans

        [HttpGet]
        [Route("api/AppointmentDetails/schedules/{doctorId}/{date:datetime}")]
        // sample url https://localhost:44327/api/AppointmentDetails/schedules/1/2024-04-26
        public IHttpActionResult GetScheduledTimeSlotsBasedOnDocIDandDate(int doctorId, DateTime date)
        {
            List<bool> schedules = new List<bool>(18);
            for (int i = 0; i < 18; i++)
            {
                schedules.Add(false);
            }

            var dateOnly = date.Date;

            // Ensuring the date comparison includes only the date part, not the time part
            var scheuledTimeSlots = db.AppointmentDetails
               .Where(a => a.DoctorID == doctorId && DbFunctions.TruncateTime(a.ScheduleDate) == dateOnly)
               .Select(a => a.ScheduleTimeSlot);

            foreach (var item in scheuledTimeSlots)
            {
                schedules[item] = true;
            }

            return Ok(schedules);

        }

    }
}
