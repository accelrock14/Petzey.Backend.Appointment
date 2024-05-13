using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Backend.Appointment.Domain.Entities
{
    public class DoctorAvgRating
    {
        public double DoctorRating {  get; set; }
        public string DoctorId {  get; set; }

        public int NumberOfRatings {  get; set; }
    }
}
