using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Backend.Appointment.Domain.Entities
{
    public class Appointment
    {
        public int AppointmentID { get; set; }
        public int DoctorID { get; set; }
        public int PetID { get; set; }
        public int OwnerID { get; set; }
        public DateTime ScheduleDate { get; set; }
        public DateTime BookingDate { get; set; }
        public string ReasonForVisit { get; set; }
        public Status Status { get; set; }
        public Report Report { get; set; }
        public Feedback Feedback { get; set; }
        public List<PetIssue> Petissues { get; set; } = new List<PetIssue>();
    }
}
