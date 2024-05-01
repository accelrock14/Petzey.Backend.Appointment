namespace Petzey.Backend.Appointment.Domain
{
    public class ReportTest
    {
        public int ReportTestID { get; set; }
        public int TestID { get; set; }
        public virtual Test Test { get; set; }
    }

}
