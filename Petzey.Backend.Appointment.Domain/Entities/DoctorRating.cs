using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Backend.Appointment.Domain.Entities
{
    public class DoctorRating
    {
        public int DoctorRatingId {  get; set; }
        public string DoctorId {  get; set; }
        public int AppointmentId {  get; set; }
        public double AvgRating {  get; set; }
    }
}
