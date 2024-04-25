using Petzey.Backend.Appointment.Domain;
using Petzey.Backend.Appointment.Domain.DTO;
using Petzey.Backend.Appointment.Domain.Entities;
using Petzey.Backend.Appointment.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Backend.Appointment.Data.Repository
{
    public class AppointmentRepository : IAppointmentRepository, IReportRepository
    {
        PetzeyDbContext db = new PetzeyDbContext();
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
