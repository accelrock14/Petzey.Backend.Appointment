﻿using Petzey.Backend.Appointment.Data;
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
        [HttpGet]
        [Route("api/dashboard/appointments/filter/{offset?}")]
        public IHttpActionResult GetAllAppointmentsWithFilters(FilterParamsDto filters, int offset = 0)
        {
            var appointments = repo.GetAllAppointmentsWithFilters(filters); 
            return Ok(appointments.Skip(offset).Take(20)); //20 appointments per page    
        }
        [HttpGet]
        [Route("api/dashboard/petappointments/filter/{ownerid}/{offset?}")]
        public IHttpActionResult GetPetAppointmentsWithFilters(FilterParamsDto filters, int ownerid, int offset = 0)
        {
            var appointments = repo.GetAppointmentsByOwnerIdWithFilters(filters, ownerid);
            return Ok(appointments.Skip(offset).Take(20)); //20 appointments per page    
        }
        [HttpGet]
        [Route("api/dashboard/vetappointments/filter/{vetid}/{offset?}")]
        public IHttpActionResult GetVetAppointmentsWithFilters(FilterParamsDto filters, int vetid, int offset = 0)
        {
            var appointments = repo.GetAppointmentsByVetIdWithFilters(filters, vetid);
            return Ok(appointments.Skip(offset).Take(20)); //20 appointments per page    
        }

        [HttpGet]
        [Route("api/appointment/petappointments/{petid}/{date}")]
        public IHttpActionResult GetAppointmentsByPetIdAndDate(int petid, DateTime date)
        {
            var appointments = repo.AppointmentByPetIdAndDate(petid, date);
            return Ok(appointments);
        }
        [HttpGet]
        [Route("api/appointment/petappointments/{petid}")]
        public IHttpActionResult GetAppointmentsByPetId(int petid)
        {
            var appointments = repo.AppointmentByPetId(petid);
            return Ok(appointments);
        }
    }
}
