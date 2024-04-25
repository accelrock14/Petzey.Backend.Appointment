using System;

namespace Petzey.Backend.Appointment.Domain.DTO
{
    public class AppointmentCardDto
    {
        public int AppointmentID { get; set; }
        public int DoctorID { get; set; }
        public int PetID { get; set; }
        public DateTime ScheduleDate { get; set; }
    }
}
