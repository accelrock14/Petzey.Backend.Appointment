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
using System.Text;
using NLog;
using Petzey.Backend.Appointment.Domain.Interfaces;
using Petzey.Backend.Appointment.Data.Repository;

namespace Petzey.Backend.Appointment.API.Controllers
{
    public class AppointmentController : ApiController
    {

        //private PetzeyDbContext db = new PetzeyDbContext();
        //IAppointmentRepository repo = new AppointmentRepository();
        IAppointmentRepository repo = null;
        public AppointmentController(IAppointmentRepository repo)
        {
            this.repo = repo;
        }

        // GET: api/Appointment
        public IQueryable<AppointmentDetail> GetAppointmentDetails()
        {
            return repo.GetAppointmentDetails();
        }

        // GET: api/Appointment/5
        [ResponseType(typeof(AppointmentDetail))]
        public IHttpActionResult GetAppointmentDetail(int id)
        {
            AppointmentDetail appointmentDetail = repo.GetAppointmentDetail(id);  //db.AppointmentDetails.Find(id);
            if (appointmentDetail == null)
            {
                
                return NotFound();
            }

            return Ok(appointmentDetail);
        }

        // PUT: api/Appointment/5
        [ResponseType(typeof(void))]
        // sample url https://localhost:44327/api/Appointment/1
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

            //db.Entry(appointmentDetail).State = EntityState.Modified;

            //try
            //{
            //    db.SaveChanges();
            //}
            //catch (DbUpdateConcurrencyException ex)
            //{
            //    if (!AppointmentDetailExists(id))
            //    {
            //        Logger.Error(ex, "Error while saving...");
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            if( ! repo.PutAppointmentDetail(id, appointmentDetail))
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Appointment
        [ResponseType(typeof(AppointmentDetail))]
        public IHttpActionResult PostAppointmentDetail(AppointmentDetail appointmentDetail)
        {
            if(!ModelState.IsValid)
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

            if ( ! repo.PostAppointmentDetail(appointmentDetail))
            {
                return BadRequest();
            }

            



            return CreatedAtRoute("DefaultApi", new { id = appointmentDetail.AppointmentID }, appointmentDetail);

        }

        // DELETE: api/Appointment/5
        [ResponseType(typeof(AppointmentDetail))]
        public IHttpActionResult DeleteAppointmentDetail(int id)
        {
            AppointmentDetail appointmentDetail = repo.GetAppointmentDetail(id);   // db.AppointmentDetails.Find(id);
            if (appointmentDetail == null)
            {
                return NotFound();
            }

            if (!repo.DeleteAppointmentDetail(id))
            {
                return NotFound();
            }

            return Ok(appointmentDetail);
        }

        

        private bool AppointmentDetailExists(int id)
        {
            return repo.AppointmentDetailExists(id);
        }

        // end point to fetch all list of petissues
        // GET: api/AppointmentDetails
        [HttpGet]

        [Route("api/AppointmentDetails/GeneralPetIssues")]
        // sample url https://localhost:44327/api/AppointmentDetails/GeneralPetIssues

        public IQueryable<GeneralPetIssue> GetAllGeneralPetIssues()
        {
            return repo.GetAllGeneralPetIssues();
        }

        // end point for adding a new petissue
        [HttpPost]
        [Route("api/AppointmentDetails/GeneralPetIssues")]
        // sample url https://localhost:44327/api/AppointmentDetails/GeneralPetIssues
        public IHttpActionResult PostGeneralPetIssue(GeneralPetIssue generalPetIssue)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!repo.PostGeneralPetIssue(generalPetIssue))
            {
                return BadRequest();
            }

            

            //return CreatedAtRoute("DefaultApi", new { id = petIssue.PetIssueID }, petIssue);
            return Ok(generalPetIssue);
        }

        [HttpGet]
        [Route("api/AppointmentDetails/docondate/{doctorId}/{date:datetime}")]
        // sample url https://localhost:44327/api/AppointmentDetails/docondate/1/2024-04-26
        public List<AppointmentDetail> GetAppointmentsOfDocOnDate(int doctorId, DateTime date)
        {
            
            
            return repo.GetAppointmentsOfDocOnDate(doctorId, date);
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

            var appointment = repo.GetAppointmentDetail(id);
            if (appointment == null)
            {
                return NotFound();
            }

            appointment.Status = status;

            if (!repo.PatchAppointmentStatus(id, status))
            {
                return InternalServerError();
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
            List<bool> schedules = repo.GetScheduledTimeSlotsBasedOnDocIDandDate(doctorId,date);

            return Ok(schedules);

        }

    }
}
