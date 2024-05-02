using Petzey.Backend.Appointment.Data;
using Petzey.Backend.Appointment.Domain.DTO;
using Petzey.Backend.Appointment.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Petzey.Backend.Appointment.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
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
            try
            {
                var status = repo.AppointmentStatusCounts();
                if (status == null)
                    return NotFound();

                return Ok(status);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();


            }
        }
        [HttpPost]
        [Route("api/dashboard/appointments/filter/{offset?}")]
        public IHttpActionResult GetAllAppointmentsWithFilters(FilterParamsDto filters, int offset = 0)
        {
            try
            {
                var appointments = repo.GetAllAppointmentsWithFilters(filters);
            foreach(var a in appointments)
            {
                a.PetName = "Marley";
                a.DoctorName = "John Doe";
                a.PetAge = 2;
                a.PetGender = "Male";
                a.OwnerName = "Parth";
                a.VetSpecialization = "NAVLE";
            }
            return Ok(appointments.Skip(offset).Take(3)); //3 appointments per page    
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }
        }
        [HttpPost]
        [Route("api/dashboard/petappointments/filter/{ownerid}/{offset?}")]
        public IHttpActionResult GetPetAppointmentsWithFilters(FilterParamsDto filters, string ownerid, int offset = 0)
        {
            try
            {
                var appointments = repo.GetAppointmentsByOwnerIdWithFilters(filters, ownerid);
            foreach (var a in appointments)
            {
                a.PetName = "Marley";
                a.DoctorName = "John Doe";
                a.PetAge = 2;
                a.PetGender = "Male";
                a.OwnerName = "Parth";
                a.VetSpecialization = "NAVLE";
            }
            return Ok(appointments.Skip(offset).Take(3)); //3 appointments per page    
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();


            }
        }
        [HttpPost]
        [Route("api/dashboard/vetappointments/filter/{vetid}/{offset?}")]
        public IHttpActionResult GetVetAppointmentsWithFilters(FilterParamsDto filters, string vetid, int offset = 0)
        {
            try
            {
                var appointments = repo.GetAppointmentsByVetIdWithFilters(filters, vetid);
            foreach (var a in appointments)
            {
                a.PetName = "Marley";
                a.DoctorName = "John Doe";
                a.PetAge = 2;
                a.PetGender = "Male";
                a.OwnerName = "Parth";
                a.VetSpecialization = "NAVLE";
            }
            return Ok(appointments.Skip(offset).Take(3)); //3 appointments per page    
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();


            }
        }

        [HttpGet]
        [Route("api/appointment/petappointments/{petid}/{date}")]
        public IHttpActionResult GetAppointmentsByPetIdAndDate(int petid, DateTime date)
        {
            try
            {
                var appointments = repo.AppointmentByPetIdAndDate(petid, date);
            foreach (var a in appointments)
            {
                a.PetName = "Marley";
                a.DoctorName = "John Doe";
                a.PetAge = 2;
                a.PetGender = "Male";
                a.OwnerName = "Parth";
                a.VetSpecialization = "NAVLE";
            }
            return Ok(appointments);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();


            }
        }
        [HttpGet]
        [Route("api/appointment/petappointments/{petid}")]
        public IHttpActionResult GetAppointmentsByPetId(int petid)
        {
            try
            {
                var appointments = repo.AppointmentByPetId(petid);
            foreach (var a in appointments)
            {
                a.PetName = "Marley";
                a.DoctorName = "John Doe";
                a.PetAge = 2;
                a.PetGender = "Male";
                a.OwnerName = "Parth";
                a.VetSpecialization = "NAVLE";
            }
            return Ok(appointments);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();


            }
        }
        [HttpGet]
        [Route("api/appointment/all")]
        public IHttpActionResult GetAllAppointments()
        {
            try
            {
                var appointments = repo.GetAllAppointments();
            foreach (var a in appointments)
            {
                a.PetName = "Marley";
                a.DoctorName = "John Doe";
                a.PetAge = 2;
                a.PetGender = "Male";
                a.OwnerName = "Parth";
                a.VetSpecialization = "NAVLE";
            }
            return Ok(appointments);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();


            }
        }
        [HttpGet]
        [Route("api/appointment/vets/{vetid}")]
        public IHttpActionResult GetAllAppointmentsVets(string vetid)
        {
            try
            {
                var appointments = repo.GetAppointmentsByVetId(vetid);
            foreach (var a in appointments)
            {
                a.PetName = "Marley";
                a.DoctorName = "John Doe";
                a.PetAge = 2;
                a.PetGender = "Male";
                a.OwnerName = "Parth";
                a.VetSpecialization = "NAVLE";
            }
            return Ok(appointments);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();


            }
        }
        [HttpGet]
        [Route("api/appointment/pets/{ownerid}")]
        public IHttpActionResult GetAllAppointmentsPets(string ownerid)
        {
            try
            {
                var appointments = repo.GetAppointmentsByOwnerId(ownerid);
            foreach (var a in appointments)
            {
                a.PetName = "Marley";
                a.DoctorName = "John Doe";
                a.PetAge = 2;
                a.PetGender = "Male";
                a.OwnerName = "Parth";
                a.VetSpecialization = "NAVLE";
            }
            return Ok(appointments);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }
        }
    }
}
