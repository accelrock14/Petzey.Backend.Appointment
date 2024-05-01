using System;
using System.Collections.Generic;

namespace Petzey.Backend.Appointment.Domain.DTO
{
    public class PetReportHistoryDto
    {
        public int HeartRate { get; set; }
        public float Temperature {  get; set; }
        public float OxygenLevel { get; set; }
        public DateTime ScheduleDate { get; set; }
        public List<ReportTest> Tests { get; set; }
        public List<ReportSymptom> Symptoms { get; set;}
        public List<Prescription> Prescriptions { get; set;}
    }
}
