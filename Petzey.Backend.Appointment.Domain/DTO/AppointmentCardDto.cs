using System;

namespace Petzey.Backend.Appointment.Domain.DTO
{
    public class AppointmentCardDto
    {
        public int AppointmentID { get; set; }
        public int DoctorID { get; set; }
        public int PetID { get; set; }
        public string PetName { get; set; }
        public int PetAge { get; set; }
        public string PetGender { get; set; }
        public string OwnerName { get; set; }
        // PET PHOTO
        public string DoctorName { get; set; }
        public string VetSpecialization { get; set; }
        public string DoctorPhoto { get; set; }
        public DateTime ScheduleDate { get; set; }
    }
}
