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
        public string DoctorID { get; set; }
        public int PetID { get; set; }
        public string OwnerID { get; set; }
        public DateTime ScheduleDate { get; set; }
        public int ScheduleTimeSlot { get; set; }
        public DateTime BookingDate { get; set; }
        public string ReasonForVisit { get; set; }
        public Status Status { get; set; }
        public virtual Report Report { get; set; }
        public virtual List<PetIssue> PetIssues { get; set; }
    }
}


// ScheduleTimeSlot
// 9->9:30          --> 0
// 9:30->10         --> 1
// 10->10:30        --> 2
// 10:30->11        --> 3
// 11->11:30        --> 4
// 11:30->12        --> 5
// 12->12:30        --> 6
// 12:30->1         --> 7
// 1 to 2 is lunch break
// 2->2:30          --> 8
// 2:30->3          --> 9
// 3->3:30          --> 10
// 3:30->4          --> 11
// 4->4:30          --> 12
// 4:30->5          --> 13
// 5->5:30          --> 14
// 5:30->6          --> 15
// 6->6:30          --> 16
// 6:30->7          --> 17