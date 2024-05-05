namespace Petzey.Backend.Appointment.Domain
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public string Npi { get; set; }
        public string Speciality { get; set; }
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