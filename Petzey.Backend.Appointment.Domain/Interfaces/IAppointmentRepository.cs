using Petzey.Backend.Appointment.Domain.DTO;
using Petzey.Backend.Appointment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Petzey.Backend.Appointment.Domain.Interfaces
{
    public interface IAppointmentRepository
    {
        //BOOK APPOINTMENT
        IQueryable<AppointmentDetail> GetAppointmentDetails();

        AppointmentDetail GetAppointmentDetail(int id);
        List<AppointmentDetail> GetAppointmentsOfDoctor(string docId);

        bool PutAppointmentDetail(int id, AppointmentDetail appointmentDetail);

        bool PostAppointmentDetail(AppointmentDetail appointmentDetail);

        bool DeleteAppointmentDetail(int id);

        bool AppointmentDetailExists(int id);

        IQueryable<GeneralPetIssue> GetAllGeneralPetIssues();

        bool PostGeneralPetIssue(GeneralPetIssue generalPetIssue);

        List<AppointmentDetail> GetAppointmentsOfDocOnDate(string doctorId, DateTime date);

        bool PatchAppointmentStatus(int id, Status status);

        List<bool> GetScheduledTimeSlotsBasedOnDocIDandDate(string doctorId, DateTime date);


        List<AppointmentCardDto> GetAllClosedAppointmentsByVetID(string VetID);
        List<AppointmentCardDto> GetAllClosedAppointmentsByPetID(int PetID);


        // GET METHODS
        // APPOINTMENTS 
        List<AppointmentCardDto> GetAllAppointmentsWithFilters(FilterParamsDto filterParams);
        List<AppointmentCardDto> GetAppointmentsByOwnerIdWithFilters(FilterParamsDto filterParams, string ownerid);
        List<AppointmentCardDto> GetAppointmentsByVetIdWithFilters(FilterParamsDto filterParams, string vetid);
        //change!
        List<AppointmentCardDto> GetAllAppointments();
        List<AppointmentCardDto> GetAppointmentsByOwnerId(string ownerid);
        List<AppointmentCardDto> GetAppointmentsByVetId(string vetid);
        AppointmentStatusCountsDto AppointmentStatusCounts(IDFiltersDto ids); 
        List<AppointmentCardDto> AppointmentByPetIdAndDate(int petId, DateTime date);
        List<AppointmentCardDto> AppointmentByPetId(int petId);
        List<AppointmentDetail> GetRecentAppointmentsByPetID(int petID);
        //REPORT
        Report GetReportByID(int id);
        IEnumerable<Symptom> GetAllSymptoms();
        IEnumerable<Test> GetAllTests();
        List<Medicine> GetAllMedicines();
        List<PetAppointmentHistoryDto> GetAppointmentHistoryByPetID(int PetID);
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

        List<Feedback> getAllFeedbacks();
        Feedback getFeedbackByAppointmrntId(int id);
        bool Addfeedback(Feedback feedback);

        List<FeedbackQuestion> getfeedbackquestion();
         FeedbackQuestion getfeedbackquestionbyid(int id);
        void updatefeedbackquestion(int id, FeedbackQuestion feedbackQuestion);
         void deletefeedbackquestion(int id);
         void Addfeedbackquestion(FeedbackQuestion feedbackQuestion);
        bool checkfeedbackquestion(int id);
        List<int> GetAllPetIDByVetId(string vetid);
        bool PostCancellationReason(Cancellation cancellation);
        Cancellation GetCancellationReason(int id);
    }
}








