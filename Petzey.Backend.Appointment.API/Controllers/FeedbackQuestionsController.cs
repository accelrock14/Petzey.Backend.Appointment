using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Petzey.Backend.Appointment.Data;
using Petzey.Backend.Appointment.Domain.Entities;

namespace Petzey.Backend.Appointment.API.Controllers
{
    public class FeedbackQuestionsController : ApiController
    {
        private PetzeyDbContext db = new PetzeyDbContext();

        // GET: api/FeedbackQuestions
        public IQueryable<FeedbackQuestion> GetFeedbackQuestions()
        {
            return db.FeedbackQuestions;
        }

        // GET: api/FeedbackQuestions/5
        [ResponseType(typeof(FeedbackQuestion))]
        public IHttpActionResult GetFeedbackQuestion(int id)
        {
            FeedbackQuestion feedbackQuestion = db.FeedbackQuestions.Find(id);
            if (feedbackQuestion == null)
            {
                return NotFound();
            }

            return Ok(feedbackQuestion);
        }

        // PUT: api/FeedbackQuestions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFeedbackQuestion(int id, FeedbackQuestion feedbackQuestion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != feedbackQuestion.FeedbackQuestionId)
            {
                return BadRequest();
            }

            db.Entry(feedbackQuestion).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeedbackQuestionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/FeedbackQuestions
        [ResponseType(typeof(FeedbackQuestion))]
        public IHttpActionResult PostFeedbackQuestion(FeedbackQuestion feedbackQuestion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.FeedbackQuestions.Add(feedbackQuestion);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = feedbackQuestion.FeedbackQuestionId }, feedbackQuestion);
        }

        // DELETE: api/FeedbackQuestions/5
        [ResponseType(typeof(FeedbackQuestion))]
        public IHttpActionResult DeleteFeedbackQuestion(int id)
        {
            FeedbackQuestion feedbackQuestion = db.FeedbackQuestions.Find(id);
            if (feedbackQuestion == null)
            {
                return NotFound();
            }

            db.FeedbackQuestions.Remove(feedbackQuestion);
            db.SaveChanges();

            return Ok(feedbackQuestion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FeedbackQuestionExists(int id)
        {
            return db.FeedbackQuestions.Count(e => e.FeedbackQuestionId == id) > 0;
        }
    }
}