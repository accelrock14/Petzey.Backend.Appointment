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
using Petzey.Backend.Appointment.Domain.Interfaces;
using Petzey.Backend.Appointment.Data.Repository;
using System.Web.Http.Cors;
using Petzey.Backend.Appointment.Domain.DTO;

namespace Petzey.Backend.Appointment.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
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
            try
            {
                return repo.GetAppointmentDetails();
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return (IQueryable<AppointmentDetail>)InternalServerError();

            }
        }

        // get all appointments of a doctor
        [HttpGet]
        [Route("api/AppointmentDetails/ofdoctor")]
        public IHttpActionResult GetAppointmentsOfDoctor(string docId)
        {
            try
            {
                // call repo
                return Ok(repo.GetAppointmentsOfDoctor(docId));
                
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();
            }
        }

        // GET: api/Appointment/5
        [ResponseType(typeof(AppointmentDetail))]
        public IHttpActionResult GetAppointmentDetail(int id)
        {
            try
            {
                AppointmentDetail appointmentDetail = repo.GetAppointmentDetail(id);  //db.AppointmentDetails.Find(id);
            if (appointmentDetail == null)
            {
                
                return NotFound();
            }

            return Ok(appointmentDetail);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }
        }

        // PUT: api/Appointment/5
        [ResponseType(typeof(void))]
        // sample url https://localhost:44327/api/Appointment/1
        public IHttpActionResult PutAppointmentDetail(int id, AppointmentDetail appointmentDetail)
        {
            try
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
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }
        }

        // POST: api/Appointment
        [ResponseType(typeof(AppointmentDetail))]
        public IHttpActionResult PostAppointmentDetail(AppointmentDetail appointmentDetail)
        {
            try
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

            if ( ! repo.PostAppointmentDetail(appointmentDetail))
            {
                return BadRequest();
            }

            



            return CreatedAtRoute("DefaultApi", new { id = appointmentDetail.AppointmentID }, appointmentDetail);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }

        }

        // DELETE: api/Appointment/5
        [ResponseType(typeof(AppointmentDetail))]
        public IHttpActionResult DeleteAppointmentDetail(int id)
        {
            try
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
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }
        }

        

       /* private bool AppointmentDetailExists(int id)
        {
            try
            {

                return repo.AppointmentDetailExists(id);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }

        }*/

        // end point to fetch all list of petissues
        // GET: api/AppointmentDetails
        [HttpGet]

        [Route("api/AppointmentDetails/GeneralPetIssues")]
        // sample url https://localhost:44327/api/AppointmentDetails/GeneralPetIssues

        public IQueryable<GeneralPetIssue> GetAllGeneralPetIssues()
        {
            try
            {
                return repo.GetAllGeneralPetIssues();
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return (IQueryable<GeneralPetIssue>)InternalServerError();

            }
        }

        // end point for adding a new petissue
        [HttpPost]
        [Route("api/AppointmentDetails/GeneralPetIssues")]
        // sample url https://localhost:44327/api/AppointmentDetails/GeneralPetIssues
        public IHttpActionResult PostGeneralPetIssue(GeneralPetIssue generalPetIssue)
        {
            try
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
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }
        }

        [HttpGet]
        [Route("api/AppointmentDetails/docondate/{doctorId}/{date:datetime}")]
        // sample url https://localhost:44327/api/AppointmentDetails/docondate/1/2024-04-26
        public List<AppointmentDetail> GetAppointmentsOfDocOnDate(string doctorId, DateTime date)
        {
            
            
            return repo.GetAppointmentsOfDocOnDate(doctorId, date);
        }

        // POST Cancellation reason
        [HttpPost]
        [Route("api/AppointmentCancellationReason")]
        public IHttpActionResult PostCancellationReason(Cancellation cancellation)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!repo.PostCancellationReason(cancellation))
                {
                    return BadRequest();
                }

                return Ok(cancellation.Reason_for_cancellation);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }
        }
        [HttpGet]
        [Route("api/GetAppointmentCancellationReason/{id}")]
        public IHttpActionResult GetCancellationReason(int id)
        {
            try
            {
                Cancellation cancellation = repo.GetCancellationReason(id);

                return Ok(cancellation);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }

        }

        // PATCH: api/AppointmentDetails/5/status
        [HttpPatch]
        [Route("api/AppointmentDetails/{id}/status")]
        [ResponseType(typeof(void))]
        // sample url https://localhost:44327/api/AppointmentDetails/5/status
        // in body send 0,1,2,3 for the required status.
        public IHttpActionResult PatchAppointmentStatus(int id, [FromBody] Status status)
        {
            try
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
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }
        }

        //get doctor schedule based on date (first get today's schedule)
        //should return the list of booleans

        [HttpGet]
        [Route("api/AppointmentDetails/schedules/{doctorId}/{date:datetime}")]
        // sample url https://localhost:44327/api/AppointmentDetails/schedules/1/2024-04-26
        public IHttpActionResult GetScheduledTimeSlotsBasedOnDocIDandDate(string doctorId, DateTime date)
        {
            try
            {
                List<bool> schedules = repo.GetScheduledTimeSlotsBasedOnDocIDandDate(doctorId, date);

                return Ok(schedules);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }

        }


        [HttpGet]
        [Route("api/AppointmentDetails/allappointmentsbyvetid/{vetID}")]
        public IHttpActionResult GetAllClosedAppointmentByVetID(string vetID)
        {
            try
            {
                List<AppointmentCardDto> resultList=repo.GetAllClosedAppointmentsByVetID(vetID);

                return Ok(resultList);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }

        }


        [HttpGet]
        [Route("api/AppointmentDetails/allappointmentsbypetid/{petID}")]
        public IHttpActionResult GetAllClosedAppointmentByPetID(int petID)
        {
            try
            {
                List<AppointmentCardDto> resultList = repo.GetAllClosedAppointmentsByPetID(petID);

                return Ok(resultList);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }

        }
        [HttpGet]
        [Route("api/PetIdByDocId/{vetID}")]
        public IHttpActionResult GetAllPetIdByPetID(string vetID)
        {
            try
            {
                List<int> pestIds = repo.GetAllPetIDByVetId(vetID);

                return Ok(pestIds);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }
        }


    }
}
