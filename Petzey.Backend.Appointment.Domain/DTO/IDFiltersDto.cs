using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Backend.Appointment.Domain.DTO
{
    public class IDFiltersDto
    {
        public string DoctorID { get; set; }
        public string OwnerID { get; set; }
    }
}
