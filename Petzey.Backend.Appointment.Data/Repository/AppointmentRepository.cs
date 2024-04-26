﻿using Petzey.Backend.Appointment.Domain;
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
    public class AppointmentRepository : IAppointmentRepository
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

        public void AddReport(Report report)
        {
            db.Reports.Add(report);
            db.SaveChanges();
        }

        public void EditReport(Report report)
        {
            foreach (PrescribedMedicine medicine in report.Prescription.PrescribedMedicines)
            {
                db.Entry(medicine.Medicine).State = System.Data.Entity.EntityState.Modified;
                db.Entry(medicine).State = System.Data.Entity.EntityState.Modified;
            }
            db.Entry(report).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

        public List<Medicine> GetAllMedicines()
        {
            return db.Medicines.ToList();
        }

        public IEnumerable<Symptom> GetAllSymptoms()
        {
            return db.Symptoms.Distinct();
        }

        public IEnumerable<Test> GetAllTests()
        {
            return db.Tests.Distinct();
        }

        public List<AppointmentDetail> GetRecentAppointmentsByPetID(int PetID)
        {
            return db.AppointmentDetails.Where(a => a.PetID == PetID && a.Status == Status.Closed).OrderByDescending(a => a.ScheduleDate).Take(10).ToList();

        }

        public Report GetReportByID(int id)
        {
            return db.Reports.Find(id);
        }

        public List<Prescription> GetHistoryOfPrescriptionsByPetID(int PetID)
        {
            return db.AppointmentDetails.Where(a => a.PetID == PetID && a.Status == Status.Closed).Select(a => a.Report.Prescription).ToList();

        }

        public AppointmentDetail MostRecentAppointmentByPetID(int PetID)
        {
            return db.AppointmentDetails.Where(a => a.PetID == PetID && a.Status == Status.Closed).OrderByDescending(a => a.ScheduleDate).FirstOrDefault();

        }
    }
}
