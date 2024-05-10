using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Backend.Appointment.Domain.Entities
{
    public class Cancellation
    {
        [Key]
        public int CancellationId { get; set; }
        public int AppointmentID { get; set; }
        public string Reason_for_cancellation { get; set; }
    }
}
