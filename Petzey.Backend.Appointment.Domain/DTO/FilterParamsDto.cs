using Petzey.Backend.Appointment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Backend.Appointment.Domain.DTO
{
    public class FilterParamsDto
    {
        public DateTime ScheduleDate { get; set; }
        public Status? Status { get; set; } = null;
        public string DoctorID { get; set; }
    }
}
