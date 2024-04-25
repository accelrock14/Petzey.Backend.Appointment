using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Backend.Appointment.Domain.Entities
{
    public class Rating
    {
        public int RatingID { get; set; }
        public string Question { get; set; }
        public int RatingValue {  get; set; }
    }
}
