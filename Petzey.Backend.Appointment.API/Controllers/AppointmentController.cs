using Petzey.Backend.Appointment.Data;
using Petzey.Backend.Appointment.Domain.Entities;
using Petzey.Backend.Appointment.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Text;
using Petzey.Backend.Appointment.Domain.Interfaces;
using Petzey.Backend.Appointment.Data.Repository;
using System.Web.Http.Cors;
using Petzey.Backend.Appointment.Domain.DTO;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Petzey.Backend.Appointment.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AppointmentController : ApiController
    {

        //private PetzeyDbContext db = new PetzeyDbContext();
        //IAppointmentRepository repo = new AppointmentRepository();
        IAppointmentRepository repo = null;
        public AppointmentController(IAppointmentRepository repo)
        {
            this.repo = repo;
        }

        // GET: api/Appointment
        public IQueryable<AppointmentDetail> GetAppointmentDetails()
        {
            try
            {
                return repo.GetAppointmentDetails();
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return (IQueryable<AppointmentDetail>)InternalServerError();

            }
        }

        // get all appointments of a doctor
        [HttpGet]
        [Route("api/AppointmentDetails/ofdoctor")]
        public IHttpActionResult GetAppointmentsOfDoctor(string docId)
        {
            try
            {
                // call repo
                return Ok(repo.GetAppointmentsOfDoctor(docId));
                
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();
            }
        }

        // GET: api/Appointment/5
        [ResponseType(typeof(AppointmentDetail))]
        public IHttpActionResult GetAppointmentDetail(int id)
        {
            try
            {
                AppointmentDetail appointmentDetail = repo.GetAppointmentDetail(id);  //db.AppointmentDetails.Find(id);
            if (appointmentDetail == null)
            {
                
                return NotFound();
            }

            return Ok(appointmentDetail);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }
        }

        // PUT: api/Appointment/5
        [ResponseType(typeof(void))]
        // sample url https://localhost:44327/api/Appointment/1
        public IHttpActionResult PutAppointmentDetail(int id, AppointmentDetail appointmentDetail)
        {
            try
            {
                if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int slot = appointmentDetail.ScheduleTimeSlot;

            int hoursToAdd = 9 + (slot * 30 / 60);
            int minutesToAdd = (slot * 30) % 60;

            // if it is the lunch break theen
            if (slot >= 8)
            {
                hoursToAdd += 1;
            }

            appointmentDetail.ScheduleDate = appointmentDetail.ScheduleDate.Date.AddHours(hoursToAdd).AddMinutes(minutesToAdd);

            if (slot < 0 || slot > 17)
            {
                return BadRequest("Invalid time slot.");
            }



            if (id != appointmentDetail.AppointmentID)
            {
                return BadRequest();
            }

            //db.Entry(appointmentDetail).State = EntityState.Modified;

            //try
            //{
            //    db.SaveChanges();
            //}
            //catch (DbUpdateConcurrencyException ex)
            //{
            //    if (!AppointmentDetailExists(id))
            //    {
            //        Logger.Error(ex, "Error while saving...");
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            if( ! repo.PutAppointmentDetail(id, appointmentDetail))
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }
        }

        // POST: api/Appointment
        [ResponseType(typeof(AppointmentDetail))]
        public IHttpActionResult PostAppointmentDetail(AppointmentDetail appointmentDetail)
        {
            try
            {
                if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int slot = appointmentDetail.ScheduleTimeSlot;

            int hoursToAdd = 9 + (slot * 30 / 60);
            int minutesToAdd = (slot * 30) % 60;

            // if it is the lunch break theen
            if (slot >= 8)
            {
                hoursToAdd += 1;
            }

            appointmentDetail.ScheduleDate = appointmentDetail.ScheduleDate.Date.AddHours(hoursToAdd).AddMinutes(minutesToAdd);

            if (slot < 0 || slot > 17)
            {
                return BadRequest("Invalid time slot.");
            }

            if ( ! repo.PostAppointmentDetail(appointmentDetail))
            {
                return BadRequest();
            }

            return CreatedAtRoute("DefaultApi", new { id = appointmentDetail.AppointmentID }, appointmentDetail);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }

        }

        // DELETE: api/Appointment/5
        [ResponseType(typeof(AppointmentDetail))]
        public IHttpActionResult DeleteAppointmentDetail(int id)
        {
            try
            {
                AppointmentDetail appointmentDetail = repo.GetAppointmentDetail(id);   // db.AppointmentDetails.Find(id);
            if (appointmentDetail == null)
            {
                return NotFound();
            }

            if (!repo.DeleteAppointmentDetail(id))
            {
                return NotFound();
            }

            return Ok(appointmentDetail);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }
        }

        

       /* private bool AppointmentDetailExists(int id)
        {
            try
            {

                return repo.AppointmentDetailExists(id);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }

        }*/

        // end point to fetch all list of general pet issues
        // GET: api/AppointmentDetails
        [HttpGet]

        [Route("api/AppointmentDetails/GeneralPetIssues")]
        // sample url https://localhost:44327/api/AppointmentDetails/GeneralPetIssues

        public IQueryable<GeneralPetIssue> GetAllGeneralPetIssues()
        {
            try
            {
                return repo.GetAllGeneralPetIssues();
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return (IQueryable<GeneralPetIssue>)InternalServerError();

            }
        }

        // end point for adding a new petissue
        [HttpPost]
        [Route("api/AppointmentDetails/GeneralPetIssues")]
        // sample url https://localhost:44327/api/AppointmentDetails/GeneralPetIssues
        public IHttpActionResult PostGeneralPetIssue(GeneralPetIssue generalPetIssue)
        {
            try
            {
                if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!repo.PostGeneralPetIssue(generalPetIssue))
            {
                return BadRequest();
            }

            

            //return CreatedAtRoute("DefaultApi", new { id = petIssue.PetIssueID }, petIssue);
            return Ok(generalPetIssue);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }
        }

        [HttpGet]
        [Route("api/AppointmentDetails/docondate/{doctorId}/{date:datetime}")]
        // sample url https://localhost:44327/api/AppointmentDetails/docondate/1/2024-04-26
        public List<AppointmentDetail> GetAppointmentsOfDocOnDate(string doctorId, DateTime date)
        {
            return repo.GetAppointmentsOfDocOnDate(doctorId, date);
        }


        // PATCH: api/AppointmentDetails/5/status
        [HttpPatch]
        [Route("api/AppointmentDetails/{id}/status")]
        [ResponseType(typeof(void))]
        // sample url https://localhost:44327/api/AppointmentDetails/5/status
        // in body send 0,1,2,3 for the required status.
        public IHttpActionResult PatchAppointmentStatus(int id, [FromBody] Status status)
        {
            try
            {
                if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appointment = repo.GetAppointmentDetail(id);
            if (appointment == null)
            {
                return NotFound();
            }

            appointment.Status = status;

            if (!repo.PatchAppointmentStatus(id, status))
            {
                return InternalServerError();
            }

            return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }
        }

        //get doctor schedule based on date (first get today's schedule)
        //should return the list of booleans

        [HttpGet]
        [Route("api/AppointmentDetails/schedules/{doctorId}/{date:datetime}")]
        // sample url https://localhost:44327/api/AppointmentDetails/schedules/1/2024-04-26
        public IHttpActionResult GetScheduledTimeSlotsBasedOnDocIDandDate(string doctorId, DateTime date)
        {
            try
            {
                List<bool> schedules = repo.GetScheduledTimeSlotsBasedOnDocIDandDate(doctorId, date);

                return Ok(schedules);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }

        }


        [HttpGet]
        [Route("api/AppointmentDetails/allappointmentsbyvetid/{vetID}")]
        public async Task<IHttpActionResult> GetAllClosedAppointmentByVetID(string vetID)
        {
            try
            {
                List<AppointmentCardDto> appointmentsList = repo.GetAllClosedAppointmentsByVetID(vetID);

                var docIds = new List<int>();
                foreach (var appointment in appointmentsList)
                {
                    if (int.TryParse(appointment.DoctorID, out int doctorId))
                    {
                        docIds.Add(doctorId);
                    }
                }
                var petIds = new List<int>();
                foreach (var appointment in appointmentsList)
                {
                    petIds.Add(appointment.PetID);
                }
                using (var httpClient = new HttpClient())
                {
                    // Create request content with the JSON string and specify the content type
                    var docIdsJson = JsonConvert.SerializeObject(docIds);
                    var requestContent = new StringContent(docIdsJson, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync("https://petzyvetapi20240505160604.azurewebsites.net/api/vets/VetDetails", requestContent);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        // Deserialize the JSON response to a list of CardVetDetailsDto objects
                        var cardVetDetailsList = JsonConvert.DeserializeObject<List<CardVetDetailsDto>>(responseContent);

                        foreach (var appointment in appointmentsList)
                        {
                            // If vet details are found, update the appointment object
                            var doctorDetails = cardVetDetailsList.FirstOrDefault(d => (d.VetId).ToString() == appointment.DoctorID);
                            if (doctorDetails != null)
                            {
                                appointment.VetSpecialization = doctorDetails.Specialization;
                                appointment.DoctorName = doctorDetails.Name;
                                appointment.DoctorPhoto = doctorDetails.Photo;
                            }
                        }
                    }
                }

                // Fetch pet details
                using (var httpClient = new HttpClient())
                {
                    // Convert the list of doctor IDs to a JSON string
                    var petIdsJson = JsonConvert.SerializeObject(petIds);
                    var requestContent = new StringContent(petIdsJson, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync("https://petzeypetwebapi20240505153103.azurewebsites.net/api/pets/getPetsByIDs", requestContent);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var cardPetDetailsList = JsonConvert.DeserializeObject<List<CardPetDetailsDto>>(responseContent);
                        //convert the above list to dictionary for fastly find petdetails by its ID
                        var petDetailsDictionary = cardPetDetailsList.ToDictionary(p => p.PetID);

                        foreach (var appointment in appointmentsList)
                        {
                            // If vet details are found, update the appointment object
                            if (petDetailsDictionary.TryGetValue(appointment.PetID, out var petDetails))
                            {
                                appointment.PetName = petDetails.PetName;
                                appointment.PetGender = petDetails.PetGender;
                                appointment.PetPhoto = petDetails.petImage;
                                appointment.PetAge = petDetails.PetAge;
                                appointment.OwnerID = petDetails.OwnerID;
                            }
                        }
                    }
                }

                // Fetch owner details
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync("https://petzeybackendappointmentapi20240505153736.azurewebsites.net/api/getalluseridsandname");

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var ownerData = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);

                        foreach (var appointment in appointmentsList)
                        {
                            if (appointment.OwnerID != null && ownerData.ContainsKey(appointment.OwnerID))
                            {
                                appointment.OwnerName = ownerData[appointment.OwnerID];
                            }
                            else
                            {
                                appointment.OwnerName = "Unknown Owner";
                            }
                        }
                    }
                }


                return Ok(appointmentsList);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }

        }


        [HttpGet]
        [Route("api/AppointmentDetails/allappointmentsbypetid/{petID}")]
        public async Task<IHttpActionResult> GetAllClosedAppointmentByPetID(int petID)
        {
            try
            {
                List<AppointmentCardDto> appointmentsList = repo.GetAllClosedAppointmentsByPetID(petID);

                var docIds = new List<int>();
                foreach (var appointment in appointmentsList)
                {
                    if (int.TryParse(appointment.DoctorID, out int doctorId))
                    {
                        docIds.Add(doctorId);
                    }
                }
                var petIds = new List<int>();
                foreach (var appointment in appointmentsList)
                {
                    petIds.Add(appointment.PetID);
                }
                using (var httpClient = new HttpClient())
                {
                    // Create request content with the JSON string and specify the content type
                    var docIdsJson = JsonConvert.SerializeObject(docIds);
                    var requestContent = new StringContent(docIdsJson, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync("https://petzyvetapi20240505160604.azurewebsites.net/api/vets/VetDetails", requestContent);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        // Deserialize the JSON response to a list of CardVetDetailsDto objects
                        var cardVetDetailsList = JsonConvert.DeserializeObject<List<CardVetDetailsDto>>(responseContent);

                        foreach (var appointment in appointmentsList)
                        {
                            // If vet details are found, update the appointment object
                            var doctorDetails = cardVetDetailsList.FirstOrDefault(d => (d.VetId).ToString() == appointment.DoctorID);
                            if (doctorDetails != null)
                            {
                                appointment.VetSpecialization = doctorDetails.Specialization;
                                appointment.DoctorName = doctorDetails.Name;
                                appointment.DoctorPhoto = doctorDetails.Photo;
                            }
                        }
                    }
                }

                // Fetch pet details
                using (var httpClient = new HttpClient())
                {
                    // Convert the list of doctor IDs to a JSON string
                    var petIdsJson = JsonConvert.SerializeObject(petIds);
                    var requestContent = new StringContent(petIdsJson, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync("https://petzeypetwebapi20240505153103.azurewebsites.net/api/pets/getPetsByIDs", requestContent);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var cardPetDetailsList = JsonConvert.DeserializeObject<List<CardPetDetailsDto>>(responseContent);
                        //convert the above list to dictionary for fastly find petdetails by its ID
                        var petDetailsDictionary = cardPetDetailsList.ToDictionary(p => p.PetID);

                        foreach (var appointment in appointmentsList)
                        {
                            // If vet details are found, update the appointment object
                            if (petDetailsDictionary.TryGetValue(appointment.PetID, out var petDetails))
                            {
                                appointment.PetName = petDetails.PetName;
                                appointment.PetGender = petDetails.PetGender;
                                appointment.PetPhoto = petDetails.petImage;
                                appointment.PetAge = petDetails.PetAge;
                                appointment.OwnerID = petDetails.OwnerID;
                            }
                        }
                    }
                }

                // Fetch owner details
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync("https://petzeybackendappointmentapi20240505153736.azurewebsites.net/api/getalluseridsandname");

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var ownerData = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);

                        foreach (var appointment in appointmentsList)
                        {
                            if (appointment.OwnerID != null && ownerData.ContainsKey(appointment.OwnerID))
                            {
                                appointment.OwnerName = ownerData[appointment.OwnerID];
                            }
                            else
                            {
                                appointment.OwnerName = "Unknown Owner";
                            }
                        }
                    }
                }



                return Ok(appointmentsList);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }

        }
        [HttpGet]
        [Route("api/PetIdByDocId/{vetID}")]
        public IHttpActionResult GetAllPetIdByPetID(string vetID)
        {
            try
            {
                List<int> pestIds = repo.GetAllPetIDByVetId(vetID);

                return Ok(pestIds);
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }
        }


    }
}
