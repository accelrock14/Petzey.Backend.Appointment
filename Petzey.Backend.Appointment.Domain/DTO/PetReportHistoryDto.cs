using System.Collections.Generic;

namespace Petzey.Backend.Appointment.Domain.DTO
{
    public class PetReportHistoryDto
    {
        public int ReportID { get; set; }  //latest report
        public int HeartRate { get; set; }
        public float Temperature {  get; set; }
        public float OxygenLevel { get; set; }
        public List<Test> Tests { get; set; }
        public List<Symptom> Symptoms { get; set;}
        public List<Prescription> Prescriptions { get; set;}
    }
}
