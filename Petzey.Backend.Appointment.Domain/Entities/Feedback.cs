using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Backend.Appointment.Domain.Entities
{
    public class Feedback
    {

        public int FeedbackID { get; set; }

        public virtual  List<Question> Questions{  get; set; }
        public string Recommendation {  get; set; }
        public string Comments { get; set; }
        public int AppointmentId { get; set; }


    }
}
