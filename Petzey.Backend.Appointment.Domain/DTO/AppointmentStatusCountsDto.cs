using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Backend.Appointment.Domain.DTO
{
    public class AppointmentStatusCountsDto
    {
        int Pending { get; set; }   
        int Confirmed { get; set; }
        int Cancelled { get; set; }
        int Total { get; set; }
        int Closed { get; set; }
    }
}
