using Newtonsoft.Json;
using Petzey.Backend.Appointment.Data;
using Petzey.Backend.Appointment.Domain.DTO;
using Petzey.Backend.Appointment.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
        [Route("api/dashboard/statuscounts/{vetid}")]
        public IHttpActionResult GetStatusCounts(string vetid)
        {
            try
            {
                var status = repo.AppointmentStatusCounts(vetid);
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
        public async Task<IHttpActionResult> GetAllAppointmentsWithFilters(FilterParamsDto filters, int offset = 0)
        {
            try
            {
                IEnumerable<AppointmentCardDto> appointments = repo.GetAllAppointmentsWithFilters(filters);

                // Apply Skip() and Take() without converting to List
                appointments = appointments.Skip(offset).Take(3);

                // Now if you need appointments as List, you can convert it
                List<AppointmentCardDto> appointmentsList = appointments.ToList();

                var docIds = new List<int>();
                foreach(var appointment in appointments)
                {
                    docIds.Add(int.Parse(appointment.DoctorID));
                }
                var petIds = new List<int>();
                foreach (var appointment in appointments)
                {
                    petIds.Add(appointment.PetID);
                }
                using (var httpClient = new HttpClient())
                {
                    // Convert docIds to JSON string
                    var docIdsJson = JsonConvert.SerializeObject(docIds);

                    // Create the request content
                    var requestContent = new StringContent(docIdsJson, Encoding.UTF8, "application/json");

                    // Set the required headers if any
                    // httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "YourToken");

                    // Make the POST request
                    var response = await httpClient.PostAsync("https://petzyvetapi20240502220748.azurewebsites.net/api/vets/VetDetails", requestContent);

                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content
                        var responseContent = await response.Content.ReadAsStringAsync();

                        // Deserialize the JSON response to an array of CardVetDetailsDto
                        var cardVetDetailsList = JsonConvert.DeserializeObject<List<CardVetDetailsDto>>(responseContent);

                        // Process the data as needed
                        for (int i = 0; i<Math.Min(3, cardVetDetailsList.Count); i++)
                        {
                            appointmentsList[i].DoctorID = (cardVetDetailsList[i].VetId).ToString();
                            appointmentsList[i].VetSpecialization = cardVetDetailsList[i].Specialization;
                            appointmentsList[i].DoctorName = cardVetDetailsList[i].Name;
                            appointmentsList[i].DoctorPhoto = cardVetDetailsList[i].Photo;
                        }
                    }
                }

                using (var httpClient = new HttpClient())
                {
                    // Convert docIds to JSON string
                    var petIdsJson = JsonConvert.SerializeObject(petIds);

                    // Create the request content
                    var requestContent = new StringContent(petIdsJson, Encoding.UTF8, "application/json");

                    // Set the required headers if any
                    // httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "YourToken");

                    // Make the POST request
                    var response = await httpClient.PostAsync("https://petzeypetswebapi20240503003857.azurewebsites.net/api/pets/getPetsByIDs", requestContent);

                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content
                        var responseContent = await response.Content.ReadAsStringAsync();

                        // Deserialize the JSON response to an array of CardVetDetailsDto
                        var cardPetDetailsList = JsonConvert.DeserializeObject<List<CardPetDetailsDto>>(responseContent);

                        // Process the data as needed
                        for (int i = 0; i < Math.Min(3, cardPetDetailsList.Count); i++)
                        {
                            appointmentsList[i].PetID = cardPetDetailsList[i].PetID;
                            appointmentsList[i].PetName = cardPetDetailsList[i].PetName;
                            appointmentsList[i].PetGender = cardPetDetailsList[i].PetGender;
                            appointmentsList[i].PetPhoto = cardPetDetailsList[i].petImage;
                            appointmentsList[i].PetAge = cardPetDetailsList[i].PetAge;
                            appointmentsList[i].OwnerName = "John Doe";
                        }
                    }
                }
                //ownername to be assigned!!!

                return Ok(appointments); //3 appointments per page    
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();

            }
        }
        [HttpPost]
        [Route("api/dashboard/petappointments/filter/{ownerid}/{offset?}")]
        public async Task<IHttpActionResult> GetPetAppointmentsWithFilters(FilterParamsDto filters, string ownerid, int offset = 0)
        {
            try
            {
                IEnumerable<AppointmentCardDto> appointments = repo.GetAppointmentsByOwnerIdWithFilters(filters, ownerid);

                // Apply Skip() and Take() without converting to List
                appointments = appointments.Skip(offset).Take(3);

                // Now if you need appointments as List, you can convert it
                List<AppointmentCardDto> appointmentsList = appointments.ToList();
                var docIds = new List<int>();
                foreach (var appointment in appointments)
                {
                    docIds.Add(int.Parse(appointment.DoctorID));
                }
                var petIds = new List<int>();
                foreach (var appointment in appointments)
                {
                    petIds.Add(appointment.PetID);
                }
                using (var httpClient = new HttpClient())
                {
                    // Convert docIds to JSON string
                    var docIdsJson = JsonConvert.SerializeObject(docIds);

                    // Create the request content
                    var requestContent = new StringContent(docIdsJson, Encoding.UTF8, "application/json");

                    // Set the required headers if any
                    // httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "YourToken");

                    // Make the POST request
                    var response = await httpClient.PostAsync("https://petzyvetapi20240502220748.azurewebsites.net/api/vets/VetDetails", requestContent);

                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content
                        var responseContent = await response.Content.ReadAsStringAsync();

                        // Deserialize the JSON response to an array of CardVetDetailsDto
                        var cardVetDetailsList = JsonConvert.DeserializeObject<List<CardVetDetailsDto>>(responseContent);

                        // Process the data as needed
                        for (int i = 0; i < Math.Min(3, cardVetDetailsList.Count); i++)
                        {
                            appointmentsList[i].DoctorID = (cardVetDetailsList[i].VetId).ToString();
                            appointmentsList[i].VetSpecialization = cardVetDetailsList[i].Specialization;
                            appointmentsList[i].DoctorName = cardVetDetailsList[i].Name;
                            appointmentsList[i].DoctorPhoto = cardVetDetailsList[i].Photo;
                        }
                    }
                }

                using (var httpClient = new HttpClient())
                {
                    // Convert docIds to JSON string
                    var petIdsJson = JsonConvert.SerializeObject(petIds);

                    // Create the request content
                    var requestContent = new StringContent(petIdsJson, Encoding.UTF8, "application/json");

                    // Set the required headers if any
                    // httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "YourToken");

                    // Make the POST request
                    var response = await httpClient.PostAsync("https://petzeypetswebapi20240503003857.azurewebsites.net/api/pets/getPetsByIDs", requestContent);

                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content
                        var responseContent = await response.Content.ReadAsStringAsync();

                        // Deserialize the JSON response to an array of CardVetDetailsDto
                        var cardPetDetailsList = JsonConvert.DeserializeObject<List<CardPetDetailsDto>>(responseContent);

                        // Process the data as needed
                        for (int i = 0; i < Math.Min(3, cardPetDetailsList.Count); i++)
                        {
                            appointmentsList[i].PetID = cardPetDetailsList[i].PetID;
                            appointmentsList[i].PetName = cardPetDetailsList[i].PetName;
                            appointmentsList[i].PetGender = cardPetDetailsList[i].PetGender;
                            appointmentsList[i].PetPhoto = cardPetDetailsList[i].petImage;
                            appointmentsList[i].PetAge = cardPetDetailsList[i].PetAge;
                            appointmentsList[i].OwnerName = "John Doe";
                        }
                    }
                }
                //ownername to be assigned!!!
                return Ok(appointments); //3 appointments per page    
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();


            }
        }
        [HttpPost]
        [Route("api/dashboard/vetappointments/filter/{vetid}/{offset?}")]
        public async Task<IHttpActionResult> GetVetAppointmentsWithFilters(FilterParamsDto filters, string vetid, int offset = 0)
        {
            try
            {
                IEnumerable<AppointmentCardDto> appointments = repo.GetAppointmentsByVetIdWithFilters(filters, vetid);

                // Apply Skip() and Take() without converting to List
                appointments = appointments.Skip(offset).Take(3);

                // Now if you need appointments as List, you can convert it
                List<AppointmentCardDto> appointmentsList = appointments.ToList();
                var docIds = new List<int>();
                foreach (var appointment in appointments)
                {
                    docIds.Add(int.Parse(appointment.DoctorID));
                }
                var petIds = new List<int>();
                foreach (var appointment in appointments)
                {
                    petIds.Add(appointment.PetID);
                }
                using (var httpClient = new HttpClient())
                {
                    // Convert docIds to JSON string
                    var docIdsJson = JsonConvert.SerializeObject(docIds);

                    // Create the request content
                    var requestContent = new StringContent(docIdsJson, Encoding.UTF8, "application/json");

                    // Set the required headers if any
                    // httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "YourToken");

                    // Make the POST request
                    var response = await httpClient.PostAsync("https://petzyvetapi20240502220748.azurewebsites.net/api/vets/VetDetails", requestContent);

                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content
                        var responseContent = await response.Content.ReadAsStringAsync();

                        // Deserialize the JSON response to an array of CardVetDetailsDto
                        var cardVetDetailsList = JsonConvert.DeserializeObject<List<CardVetDetailsDto>>(responseContent);

                        // Process the data as needed
                        for (int i = 0; i < Math.Min(3, cardVetDetailsList.Count); i++)
                        {
                            appointmentsList[i].DoctorID = (cardVetDetailsList[i].VetId).ToString();
                            appointmentsList[i].VetSpecialization = cardVetDetailsList[i].Specialization;
                            appointmentsList[i].DoctorName = cardVetDetailsList[i].Name;
                            appointmentsList[i].DoctorPhoto = cardVetDetailsList[i].Photo;
                        }
                    }
                }

                using (var httpClient = new HttpClient())
                {
                    // Convert docIds to JSON string
                    var petIdsJson = JsonConvert.SerializeObject(petIds);

                    // Create the request content
                    var requestContent = new StringContent(petIdsJson, Encoding.UTF8, "application/json");

                    // Set the required headers if any
                    // httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "YourToken");

                    // Make the POST request
                    var response = await httpClient.PostAsync("https://petzeypetswebapi20240503003857.azurewebsites.net/api/pets/getPetsByIDs", requestContent);

                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content
                        var responseContent = await response.Content.ReadAsStringAsync();

                        // Deserialize the JSON response to an array of CardVetDetailsDto
                        var cardPetDetailsList = JsonConvert.DeserializeObject<List<CardPetDetailsDto>>(responseContent);

                        // Process the data as needed
                        for (int i = 0; i < Math.Min(3, cardPetDetailsList.Count); i++)
                        {
                            appointmentsList[i].PetID = cardPetDetailsList[i].PetID;
                            appointmentsList[i].PetName = cardPetDetailsList[i].PetName;
                            appointmentsList[i].PetGender = cardPetDetailsList[i].PetGender;
                            appointmentsList[i].PetPhoto = cardPetDetailsList[i].petImage;
                            appointmentsList[i].PetAge = cardPetDetailsList[i].PetAge;
                            appointmentsList[i].OwnerName = "John Doe";
                        }
                    }
                }
                return Ok(appointments); //3 appointments per page    
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
