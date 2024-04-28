using Petzey.Backend.Appointment.Domain.Entities;
using Petzey.Backend.Appointment.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Petzey.Backend.Appointment.API.Controllers
{
    public class FeedbackController : ApiController
    {

        IAppointmentRepository repo;

        public FeedbackController(IAppointmentRepository _repo)
        {
            repo = _repo;
        }


        [ResponseType(typeof(Feedback))]
        public IHttpActionResult GetFeedback(int id)
        {
            Feedback feedback = repo.getFeedbackByAppointmrntId(id);
            if (feedback == null)
            {
                return NotFound();
            }

            return Ok(feedback);
        }


        [ResponseType(typeof(Feedback))]
        public IHttpActionResult PostFeedback(Feedback feedback)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            repo.Addfeedback(feedback);

            return CreatedAtRoute("DefaultApi", new { id = feedback.FeedbackID }, feedback);
        }
    }
}
