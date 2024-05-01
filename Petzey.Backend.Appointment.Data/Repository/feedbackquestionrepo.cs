using Petzey.Backend.Appointment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Backend.Appointment.Data.Repository
{
   public class feedbackquestionrepo
    {
        PetzeyDbContext db = new PetzeyDbContext();
        public  List<FeedbackQuestion> getfeedbackquestion()
        {
            var questions = db.FeedbackQuestions.ToList();
            return questions;
        }
        public FeedbackQuestion getfeedbackquestionbyid(int id)
        {
            FeedbackQuestion feedbackQuestion = db.FeedbackQuestions.Find(id);
            return feedbackQuestion;
        }
        public void updatefeedbackquestion(int id,FeedbackQuestion feedbackQuestion)
        {
            db.Entry(feedbackQuestion).State = EntityState.Modified;

            
            
                db.SaveChanges();
            
        }
    }
}
