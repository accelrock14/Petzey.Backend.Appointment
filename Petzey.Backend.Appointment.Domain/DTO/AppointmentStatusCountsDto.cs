using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Backend.Appointment.Domain.DTO
{
    public class AppointmentStatusCountsDto
    {
        public int Pending { get; set; }   
        public int Confirmed { get; set; }
        public int Cancelled { get; set; }
        public int Total { get; set; }
        public int Closed { get; set; }
    }
}
