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
    public class AppointmentRepository : IAppointmentRepository
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



        // get count of appointments in different statuses
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

        // filter appointments by date
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

        // get appointments for pet on a particular date
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

        // get all appointments for a pet
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

        // edit contents of a report
        public void EditReport(Report report)
        {
            foreach (PrescribedMedicine medicine in report.Prescription.PrescribedMedicines)
            {
                db.Entry(medicine.Medicine).State = System.Data.Entity.EntityState.Modified;
                db.Entry(medicine).State = System.Data.Entity.EntityState.Modified;
            }
            Report curReport = db.Reports.Find(report.ReportID);
            curReport.Symptoms.Clear();
            curReport.Tests.Clear();

            foreach(ReportSymptom symptom in report.Symptoms)
            {
                curReport.Symptoms.Add(symptom);
            }
            foreach(ReportTest test in report.Tests)
            {
                curReport.Tests.Add(test);
            }

            db.Entry(report).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

        // get all medicines
        public List<Medicine> GetAllMedicines()
        {
            return db.Medicines.ToList();
        }

        // get all symptoms
        public IEnumerable<Symptom> GetAllSymptoms()
        {
            return db.Symptoms.Distinct();
        }

        // get all tests
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

        public Medicine GetMedicineById(int medicineId)
        {
            return db.Medicines.Find(medicineId);
        }

        public PrescribedMedicine GetPrescribedMedicine(int medicineId)
        {
            return db.PrescribedMedics.Find(medicineId);
        }

        public void AddMedicineToPrescription(int prescriptionId, PrescribedMedicine medicine)
        {
            db.Prescriptions.Find(prescriptionId).PrescribedMedicines.Add(medicine);
            db.SaveChanges();
        }
        public void RemoveMedicineFromPrescription(int prescriptionId)
        {
            db.PrescribedMedics.Remove(db.PrescribedMedics.Find(prescriptionId));
            db.SaveChanges();
        }

        public void AddSymptomToReport(int reportID, ReportSymptom reportSymptom)
        {

            db.Reports.Find(reportID).Symptoms.Add(reportSymptom);
            db.SaveChanges();
        }

        public void DeleteSymptomFromReport(int reportsymptomID){

            db.ReportSymptoms.Remove(db.ReportSymptoms.Find(reportsymptomID));
            db.SaveChanges();

         }


        public void AddTestToReport(int reportID, ReportTest reportTest)
        {

            db.Reports.Find(reportID).Tests.Add(reportTest);
            db.SaveChanges();
        }

        public void DeleteTestFromReport(int reportTestID)
        {

            db.ReportTests.Remove(db.ReportTests.Find(reportTestID));
            db.SaveChanges();
        }

        public void AddDoctorRecommendation(int reportID, RecommendedDoctor recommendedDoctor)
        {
            db.Reports.Find(reportID).RecommendedDoctors.Add(recommendedDoctor);
            db.SaveChanges();
        }

        public void RemoveDoctorRecommendation(int recommendedDoctorID)
        {
            db.RecommendedDoctors.Remove(db.RecommendedDoctors.Find(recommendedDoctorID));
            db.SaveChanges();
        }

    }
}

