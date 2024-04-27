namespace Petzey.Backend.Appointment.Domain
{
    public class ReportSymptom
    {
        public int ReportSymptomID { get; set; }
        public int SymptomID { get; set; }
        public virtual Symptom Symptom { get; set; }
    }

}
