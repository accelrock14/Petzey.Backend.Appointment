using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Backend.Appointment.Domain.DTO
{
    internal class AppointmentDto
    {
    }

    class AppointmentCardDto
    {
        int AppointmentID { get; set; }
        int DoctorID { get; set; }
        int PetID { get; set; }
        DateTime ScheduleDate { get; set; }
    }

    class PetReportHistoryDto
    {
        int ReportID { get; set; }  //latest report
        int HeartRate { get; set; }
        int Temperature {  get; set; }
        //list of tests
        //list of symptoms
        //list of priscriptions
    }
}
