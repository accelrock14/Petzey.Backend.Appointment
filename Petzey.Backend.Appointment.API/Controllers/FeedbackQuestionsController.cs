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
using Petzey.Backend.Appointment.Domain.Interfaces;

namespace Petzey.Backend.Appointment.API.Controllers
{
    public class FeedbackQuestionsController : ApiController
    {
        IAppointmentRepository repo;
        public FeedbackQuestionsController(IAppointmentRepository repo)
        {
            this.repo = repo;  
        }

        // GET: api/FeedbackQuestions
        public List<FeedbackQuestion> GetFeedbackQuestions()
        {
            return repo.getfeedbackquestion();
        }

        // GET: api/FeedbackQuestions/5
        [ResponseType(typeof(FeedbackQuestion))]
        public IHttpActionResult GetFeedbackQuestion(int id)
        {
            FeedbackQuestion feedbackQuestion =repo.getfeedbackquestionbyid(id);
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
            try
            {
                repo.updatefeedbackquestion(id, feedbackQuestion);
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

            repo.Addfeedbackquestion(feedbackQuestion);

            return CreatedAtRoute("DefaultApi", new { id = feedbackQuestion.FeedbackQuestionId }, feedbackQuestion);
        }

        // DELETE: api/FeedbackQuestions/5
        [ResponseType(typeof(FeedbackQuestion))]
        public IHttpActionResult DeleteFeedbackQuestion(int id)
        {
            FeedbackQuestion feedbackQuestion = repo.getfeedbackquestionbyid(id);
            if (feedbackQuestion == null)
            {
                return NotFound();
            }

            repo.deletefeedbackquestion(id);

            return Ok(feedbackQuestion);
        }

       

        private bool FeedbackQuestionExists(int id)
        {
            return repo.checkfeedbackquestion(id);
        }
    }
}