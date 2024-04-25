using Petzey.Backend.Appointment.Domain.Entities;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace Petzey.Backend.Appointment.Domain
{
    public class AppointmentDetail
    {
        [Key]
        public int AppointmentID { get; set; }
        public int DoctorID { get; set; }
        public int PetID { get; set; }
        public int OwnerID { get; set; }
        public DateTime ScheduleDate { get; set; }
        public DateTime BookingDate { get; set; }
        public string ReasonForVisit { get; set; }
        public Status Status { get; set; }
        public virtual Report Report { get; set; }
        public virtual List<PetIssue> PetIssues { get; set; } 
    }
}
