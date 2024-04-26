using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Backend.Appointment.Domain.Entities
{
    public class Rating
    {
        public int Competence { get; set; }
        public int Outcome { get; set; }
        public int Booking { get; set; }
        public string Recommendation { get; set; }
        public string Comments { get; set; }
    }
}
