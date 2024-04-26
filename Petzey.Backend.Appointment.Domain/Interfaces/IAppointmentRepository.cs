﻿using Petzey.Backend.Appointment.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Backend.Appointment.Domain.Interfaces
{
    public interface IAppointmentRepository
    {
        // GET METHODS
        // APPOINTMENTS
        List<AppointmentCardDto> FilterDateStatus(FilterParamsDto filterParams);
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
    }
}
