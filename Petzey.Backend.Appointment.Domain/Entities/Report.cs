using System.Collections.Generic;

namespace Petzey.Backend.Appointment.Domain
{
    public class Report
    {
        public int ReportID { get; set; }
        public virtual Prescription Prescription { get; set; }
        public virtual List<Symptom> Symptoms { get; set; }
        public virtual List<Test> Tests { get; set; }
        public int HeartRate {  get; set; }
        public float Temperature { get; set; }
        public float OxygenLevel { get; set; }
        public List<int> RecommendedDoctorIDs { get; set; }
        public string Comment {  get; set; }

    }

}
