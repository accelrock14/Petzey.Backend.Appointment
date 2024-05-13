using Petzey.Backend.Appointment.Domain.Entities;
using System;

namespace Petzey.Backend.Appointment.Domain.DTO
{
    public class AppointmentCardDto
    {
        public int AppointmentID { get; set; }
        public string DoctorID { get; set; }
        public int PetID { get; set; }
        public string PetName { get; set; }
        public int? PetAge { get; set; }
        public string PetGender { get; set; }
        public string OwnerName { get; set; }
        public string OwnerID { get; set; }
        public string PetPhoto { get; set; }
        public string DoctorName { get; set; }
        public string VetSpecialization { get; set; }
        public string DoctorPhoto { get; set; }
        public DateTime ScheduleDate { get; set; }

        //added status for filtering in UI rather than backend
        public string Status { get; set; }
        // total number of appointments
        public int All { get; set; }
    }
}
