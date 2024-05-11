using System;

namespace Petzey.Backend.Appointment.Domain.DTO
{
    public class PetAppointmentHistoryDto
    {
        public int AppointmentID {  get; set; }
        public string DoctorID {  get; set; }
        public string ReasonOfAppointment {  get; set; }

        public DateTime ScheduleDate {  get; set; }
    }
}
