using System.ComponentModel.DataAnnotations;

namespace Petzey.Backend.Appointment.Domain
{
    public class RecommendedDoctor
    {
        [Key]
        public int ID { get; set; }
        public string DoctorID { get; set; }
        public string Reason { get; set; }
    }
}
