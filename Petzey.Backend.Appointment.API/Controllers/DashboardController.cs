using Petzey.Backend.Appointment.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Petzey.Backend.Appointment.API.Controllers
{
    public class DashboardController : ApiController
    {
        //Just for CHECKING:
        PetzeyDbContext db = new PetzeyDbContext();
        [HttpGet]
        [Route("api/appointments")]
        public IHttpActionResult GetAllAppointments()
        {
            var appointments = db.AppointmentDetails.ToList();
            if (appointments == null || appointments.Count == 0)
                return NotFound();
            return Ok(appointments);
        }
    }
}
