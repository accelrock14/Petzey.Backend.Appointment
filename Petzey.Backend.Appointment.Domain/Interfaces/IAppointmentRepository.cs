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
using System.Security.Cryptography;

namespace Petzey.Backend.Appointment.Domain.Interfaces
{
    public interface IAppointmentRepository
    {
        //BOOK APPOINTMENT
        IQueryable<AppointmentDetail> GetAppointmentDetails();

        AppointmentDetail GetAppointmentDetail(int id);
        List<AppointmentDetail> GetAppointmentsOfDoctor(int docId);

        bool PutAppointmentDetail(int id, AppointmentDetail appointmentDetail);

        bool PostAppointmentDetail(AppointmentDetail appointmentDetail);

        bool DeleteAppointmentDetail(int id);

        bool AppointmentDetailExists(int id);

        IQueryable<GeneralPetIssue> GetAllGeneralPetIssues();

        bool PostGeneralPetIssue(GeneralPetIssue generalPetIssue);

        List<AppointmentDetail> GetAppointmentsOfDocOnDate(int doctorId, DateTime date);

        bool PatchAppointmentStatus(int id, Status status);

        List<bool> GetScheduledTimeSlotsBasedOnDocIDandDate(int doctorId, DateTime date);


        // GET METHODS
        // APPOINTMENTS 
        List<AppointmentCardDto> GetAllAppointmentsWithFilters(FilterParamsDto filterParams);
        List<AppointmentCardDto> GetAppointmentsByOwnerIdWithFilters(FilterParamsDto filterParams, int ownerid);
        List<AppointmentCardDto> GetAppointmentsByVetIdWithFilters(FilterParamsDto filterParams, int vetid);    
        AppointmentStatusCountsDto AppointmentStatusCounts();
        List<AppointmentCardDto> AppointmentByPetIdAndDate(int petId, DateTime date);
        List<AppointmentCardDto> AppointmentByPetId(int petId);
        List<AppointmentDetail> GetRecentAppointmentsByPetID(int petID);
        AppointmentDetail MostRecentAppointmentByPetID(int PetID);
        //REPORT
        Report GetReportByID(int id);
        IEnumerable<Symptom> GetAllSymptoms();
        IEnumerable<Test> GetAllTests();
        List<Medicine> GetAllMedicines();
        List<Prescription> GetHistoryOfPrescriptionsByPetID(int PetID);
        Medicine GetMedicineById(int id);

        // POST METHODS
        void AddReport(Report report);
        void EditReport(Report report);
        void AddMedicineToPrescription(int prescriptionId, PrescribedMedicine medicine);
        void AddSymptomToReport(int reportID, ReportSymptom reportSymptom);
        void AddTestToReport(int reportID, ReportTest reportTest);
        void AddDoctorRecommendation(int reportID, RecommendedDoctor recommendedDoctor);

        // DELETE METHODS
        void DeleteSymptomFromReport(int reportsymptomID);
        void DeleteTestFromReport(int reportTestID);
        void RemoveMedicineFromPrescription(int prescriptionId);
        void RemoveDoctorRecommendation(int recommendedDoctorID);

        void UpdateMedicine(PrescribedMedicine oldPrescription, PrescribedMedicine newPrescription);
        void UpdateReportStatus(Report oldReport, Report newReport);

        PrescribedMedicine GetPrescribed(int prescriptionID);

        ///////feedback
        ///

        IQueryable<Feedback> getAllFeedbacks();
        Feedback getFeedbackByAppointmrntId(int id);
        bool Addfeedback(Feedback feedback);
    }
}








