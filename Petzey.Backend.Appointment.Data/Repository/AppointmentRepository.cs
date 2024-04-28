using Petzey.Backend.Appointment.Domain;
using Petzey.Backend.Appointment.Domain.DTO;
using Petzey.Backend.Appointment.Domain.Entities;
using Petzey.Backend.Appointment.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;

namespace Petzey.Backend.Appointment.Data.Repository
{
    public class AppointmentRepository : IAppointmentRepository, IReportRepository
    {
        PetzeyDbContext db = new PetzeyDbContext();

        
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        //-------------------------------------------------

        public IQueryable<AppointmentDetail> GetAppointmentDetails()
        {
            return db.AppointmentDetails;
        }

        public AppointmentDetail GetAppointmentDetail(int id)
        {
            AppointmentDetail appointmentDetail = db.AppointmentDetails.Find(id);
            if (appointmentDetail == null)
            {
                Logger.Info("id does not exist...");
                Logger.Error("id does not exists ... error");
                return null;
            }

            return appointmentDetail;
        }

        public bool PutAppointmentDetail(int id, AppointmentDetail appointmentDetail)
        {


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
                return false;
            }



            if (id != appointmentDetail.AppointmentID)
            {
                return false;
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
                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;
        }

        public bool PostAppointmentDetail(AppointmentDetail appointmentDetail)
        {

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
                return false;
            }



            db.AppointmentDetails.Add(appointmentDetail);
            db.SaveChanges();

            return true;
        }

        public bool DeleteAppointmentDetail(int id)
        {

            AppointmentDetail appointmentDetail = db.AppointmentDetails.Find(id);
            if (appointmentDetail == null)
            {
                return false;
            }

            db.AppointmentDetails.Remove(appointmentDetail);
            db.SaveChanges();

            return true;
        }

        public bool AppointmentDetailExists(int id)
        {
            return db.AppointmentDetails.Count(e => e.AppointmentID == id) > 0;
        }

        public IQueryable<GeneralPetIssue> GetAllGeneralPetIssues()
        {
            return db.GeneralPetIssues;
        }

        public bool PostGeneralPetIssue(GeneralPetIssue generalPetIssue)
        {

            db.GeneralPetIssues.Add(generalPetIssue);
            db.SaveChanges();

            //return CreatedAtRoute("DefaultApi", new { id = petIssue.PetIssueID }, petIssue);
            return true;
        }

        public List<AppointmentDetail> GetAppointmentsOfDocOnDate(int doctorId, DateTime date)
        {

            var dateOnly = date.Date;

            return db.AppointmentDetails
                .Where(a => a.DoctorID == doctorId && DbFunctions.TruncateTime(a.ScheduleDate) == dateOnly)
                .ToList();
        }

        public bool PatchAppointmentStatus(int id, Status status)
        {

            var appointment = db.AppointmentDetails.Find(id);
            if (appointment == null)
            {
                return false;
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

                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;
        }

        public List<bool> GetScheduledTimeSlotsBasedOnDocIDandDate(int doctorId, DateTime date)
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

            return schedules;
        }

        
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||



        public AppointmentStatusCountsDto AppointmentStatusCounts()
        {
            AppointmentStatusCountsDto dto = new AppointmentStatusCountsDto();
            var allAppointments = db.AppointmentDetails.ToList(); 
            dto.Total = allAppointments.Count;
            dto.Closed = allAppointments.Count(a => a.Status == Domain.Entities.Status.Closed);
            dto.Pending = allAppointments.Count(a => a.Status == Domain.Entities.Status.Pending);
            dto.Cancelled = allAppointments.Count(a => a.Status == Domain.Entities.Status.Cancelled);
            dto.Confirmed = allAppointments.Count(a => a.Status == Domain.Entities.Status.Confirmed);

            return dto;
        }

        public List<AppointmentCardDto> FilterDateStatus(FilterParamsDto filterParams)
        {
            IQueryable<AppointmentDetail> query = db.AppointmentDetails;

            // Check if both Status and ScheduleDate are null or default values
            if ((filterParams.Status == null) &&
                (filterParams.ScheduleDate == null || filterParams.ScheduleDate == default(DateTime)))
            {
                // Return all appointments
                return query
                    .Select(appointment => new AppointmentCardDto
                    {
                        AppointmentID = appointment.AppointmentID,
                        DoctorID = appointment.DoctorID,
                        PetID = appointment.PetID,
                        ScheduleDate = appointment.ScheduleDate
                    })
                    .ToList();
            }

            // Filter by ScheduleDate if provided
            if (filterParams.ScheduleDate != null && filterParams.ScheduleDate != default(DateTime))
            {
                query = query.Where(appointment => DbFunctions.TruncateTime(appointment.ScheduleDate) == DbFunctions.TruncateTime(filterParams.ScheduleDate));
            }

            // Filter by Status if provided
            if (filterParams.Status != null)
            {
                query = query.Where(appointment => appointment.Status == filterParams.Status);
            }

            // Execute the query and map to AppointmentCardDto
            List<AppointmentCardDto> filteredAppointments = query
                .Select(appointment => new AppointmentCardDto
                {
                    AppointmentID = appointment.AppointmentID,
                    DoctorID = appointment.DoctorID,
                    PetID = appointment.PetID,
                    ScheduleDate = appointment.ScheduleDate
                })
                .ToList();

            return filteredAppointments;
        }
        public List<AppointmentCardDto> AppointmentByPetIdAndDate(int petId, DateTime date)
        {
            IQueryable<AppointmentDetail> query = db.AppointmentDetails.Where(appointment => appointment.PetID == petId);

            // If date is provided, filter appointments by date
            if (date != null)
            {
                query = query.Where(appointment => DbFunctions.TruncateTime(appointment.ScheduleDate) == DbFunctions.TruncateTime(date));
            }

            // Fetch appointments
            var appointments = query
                .Select(appointment => new AppointmentCardDto
                {
                    AppointmentID = appointment.AppointmentID,
                    DoctorID = appointment.DoctorID,
                    PetID = appointment.PetID,
                    ScheduleDate = appointment.ScheduleDate
                })
                .ToList();

            return appointments;
        }
        public List<AppointmentCardDto> AppointmentByPetId(int petId)
        {
            IQueryable<AppointmentDetail> query = db.AppointmentDetails.Where(appointment => appointment.PetID == petId);
            var appointments = query
                .Select(appointment => new AppointmentCardDto
                {
                    AppointmentID = appointment.AppointmentID,
                    DoctorID = appointment.DoctorID,
                    PetID = appointment.PetID,
                    ScheduleDate = appointment.ScheduleDate
                })
                .ToList();

            return appointments;
        }

        // --------------------------------------------------------------------------------
        // Report Repository Methods
        // --------------------------------------------------------------------------------

        // Get Report details by ID
        public Report GetReportByID(int id) 
        { 
            throw new NotImplementedException(); 
        }

        // Update the detials of a report
        public void UpdateReport(Report report)
        {
            throw new NotImplementedException();
        }

        // Get list of all symptoms
        public List<Symptom> GetSymptoms()
        {
            throw new NotImplementedException();
        }

        // Get list of all tests
        public List<Test> GetTests()
        {
            throw new NotImplementedException();
        }

    }
}
