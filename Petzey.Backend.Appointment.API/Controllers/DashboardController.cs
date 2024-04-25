using Petzey.Backend.Appointment.Data;
using Petzey.Backend.Appointment.Domain.DTO;
using Petzey.Backend.Appointment.Domain.Interfaces;
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
        IAppointmentRepository repo = null;
        public DashboardController(IAppointmentRepository repository) 
        {
            repo = repository;
        }
        [HttpGet]
        [Route("api/dashboard/statuscounts")]
        public IHttpActionResult GetStatusCounts()
        {
            var status = repo.AppointmentStatusCounts();
            if (status == null)
                return NotFound();

            return Ok(status);
        }
        [HttpPost]
        [Route("api/dashboard/filter/{offset?}")]
        public IHttpActionResult FilterDateStatus([FromBody] FilterParamsDto filters, int offset = 0)
        {
            var appointments = repo.FilterDateStatus(filters);
            return Ok(appointments.Skip(offset).Take(20)); //20 appointments per page    
        }
        [HttpGet]
        [Route("api/dashboard/petappointments/{petid}/{date}")]
        public IHttpActionResult GetAppointmentsByPetIdAndDate(int petid, DateTime date)
        {
            var appointments = repo.AppointmentByPetIdAndDate(petid, date);
            return Ok(appointments);
        }
        [HttpGet]
        [Route("api/dashboard/petappointments/{petid}")]
        public IHttpActionResult GetAppointmentsByPetId(int petid)
        {
            var appointments = repo.AppointmentByPetId(petid);
            return Ok(appointments);
        }
    }
}
