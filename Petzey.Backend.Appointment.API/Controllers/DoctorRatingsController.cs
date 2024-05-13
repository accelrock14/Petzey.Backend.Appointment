using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Petzey.Backend.Appointment.Data;
using Petzey.Backend.Appointment.Domain.Entities;

namespace Petzey.Backend.Appointment.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DoctorRatingsController : ApiController
    {
        private PetzeyDbContext db = new PetzeyDbContext();

        // GET: api/DoctorRatings
        public IHttpActionResult GetDoctorRatings()
        {
           var doc=db.DoctorRatings;
            return Ok(doc);
        }

        // GET: api/DoctorRatings/5
        [ResponseType(typeof(DoctorRating))]
        public IHttpActionResult GetDoctorRating(int id)
        {
            DoctorRating doctorRating = db.DoctorRatings.Find(id);
            if (doctorRating == null)
            {
                return NotFound();
            }

            return Ok(doctorRating);
        }

        // PUT: api/DoctorRatings/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDoctorRating(int id, DoctorRating doctorRating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != doctorRating.DoctorRatingId)
            {
                return BadRequest();
            }

            db.Entry(doctorRating).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorRatingExists(id))
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

        // POST: api/DoctorRatings
        [ResponseType(typeof(DoctorRating))]
        public IHttpActionResult PostDoctorRating(DoctorRating doctorRating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DoctorRatings.Add(doctorRating);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = doctorRating.DoctorRatingId }, doctorRating);
        }

        // DELETE: api/DoctorRatings/5
        [ResponseType(typeof(DoctorRating))]
        public IHttpActionResult DeleteDoctorRating(int id)
        {
            DoctorRating doctorRating = db.DoctorRatings.Find(id);
            if (doctorRating == null)
            {
                return NotFound();
            }

            db.DoctorRatings.Remove(doctorRating);
            db.SaveChanges();

            return Ok(doctorRating);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DoctorRatingExists(int id)
        {
            return db.DoctorRatings.Count(e => e.DoctorRatingId == id) > 0;
        }
    }
}