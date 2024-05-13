using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Backend.Appointment.Domain.Entities
{
    public class AppointmentCancellation
    {
        public int AppointmentCancellationID { get; set; }
        public int AppointmentID { get; set; }
        public string ReasonForCancellation { get; set; }
    }
}
