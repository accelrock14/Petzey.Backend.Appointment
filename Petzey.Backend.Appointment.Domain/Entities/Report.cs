using System.Collections.Generic;

namespace Petzey.Backend.Appointment.Domain
{
    public class Report
    {
        public int ReportID { get; set; }
        public virtual Prescription Prescription { get; set; }
        public virtual List<ReportSymptom> Symptoms { get; set; } = new List<ReportSymptom>();
        public virtual List<ReportTest> Tests { get; set; } = new List<ReportTest>();
        public virtual List<RecommendedDoctor> RecommendedDoctors { get; set; }
        public string Comment {  get; set; }
    }
}
