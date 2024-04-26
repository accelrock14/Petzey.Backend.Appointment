using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Backend.Appointment.Data.Repository
{
 public class FeedbackRepo
    {

        PetzeyDbContext db=new PetzeyDbContext();
        public FeedbackRepo()
        {
            db.Feedbacks.ToList();
        }
    }
}
