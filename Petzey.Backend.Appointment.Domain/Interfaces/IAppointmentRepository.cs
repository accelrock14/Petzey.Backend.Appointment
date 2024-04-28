using Petzey.Backend.Appointment.Domain.DTO;
using Petzey.Backend.Appointment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Backend.Appointment.Domain.Interfaces
{
    public interface IAppointmentRepository
    {
       List<AppointmentCardDto> FilterDateStatus(FilterParamsDto filterParams);
       AppointmentStatusCountsDto AppointmentStatusCounts();
       List<AppointmentCardDto> AppointmentByPetIdAndDate(int petId, DateTime date);
       List<AppointmentCardDto> AppointmentByPetId(int petId);

        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

        


        IQueryable<AppointmentDetail> GetAppointmentDetails();


        AppointmentDetail GetAppointmentDetail(int id);



        bool PutAppointmentDetail(int id, AppointmentDetail appointmentDetail);


        bool PostAppointmentDetail(AppointmentDetail appointmentDetail);


        bool DeleteAppointmentDetail(int id);

        


        bool AppointmentDetailExists(int id);




        IQueryable<GeneralPetIssue> GetAllGeneralPetIssues();



        bool PostGeneralPetIssue(GeneralPetIssue generalPetIssue);


        List<AppointmentDetail> GetAppointmentsOfDocOnDate(int doctorId, DateTime date);



        bool PatchAppointmentStatus(int id, Status status);


        List<bool> GetScheduledTimeSlotsBasedOnDocIDandDate(int doctorId, DateTime date);


    }
}








//||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
//Reference Purpose Storage of the appointment controller 


/* 
 *
 *
        private PetzeyDbContext db = new PetzeyDbContext();
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

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
                Logger.Info("id does not exist...");
                Logger.Error("id does not exists ... error");
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

            db.Entry(appointmentDetail).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!AppointmentDetailExists(id))
                {
                    Logger.Error(ex, "Error while saving...");
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

        [Route("api/AppointmentDetails/GeneralPetIssues")]
        // sample url https://localhost:44327/api/AppointmentDetails/GeneralPetIssues

        public IQueryable<GeneralPetIssue> GetAllGeneralPetIssues()
        {
            return db.GeneralPetIssues;
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

            db.GeneralPetIssues.Add(generalPetIssue);
            db.SaveChanges();

            //return CreatedAtRoute("DefaultApi", new { id = petIssue.PetIssueID }, petIssue);
            return Ok(generalPetIssue);
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
            catch (DbUpdateConcurrencyException ex)
            {
                if (!AppointmentDetailExists(id))
                {
                    Logger.Error(ex, "Error in saving in db inside patch appointment");
                    
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
                //schedules[i] = false;
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

 *
 */