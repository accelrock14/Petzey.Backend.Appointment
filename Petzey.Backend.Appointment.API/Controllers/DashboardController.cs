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

                // Now if you need appointments as List, you can convert it
                List<AppointmentCardDto> appointmentsList = appointments.ToList();

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
                    // Convert docIds to JSON string
                    var docIdsJson = JsonConvert.SerializeObject(docIds);

                    // Create the request content
                    var requestContent = new StringContent(docIdsJson, Encoding.UTF8, "application/json");

                    // Set the required headers if any
                    // httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "YourToken");

                    // Make the POST request
                    var response = await httpClient.PostAsync("https://petzyvetapi20240505160604.azurewebsites.net/api/vets/VetDetails", requestContent);

                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content
                        var responseContent = await response.Content.ReadAsStringAsync();

                        // Deserialize the JSON response to an array of CardVetDetailsDto
                        var cardVetDetailsList = JsonConvert.DeserializeObject<List<CardVetDetailsDto>>(responseContent);

                        // Process the data as needed
                        int minLength = Math.Min(appointmentsList.Count, cardVetDetailsList.Count);
                        for (int i = 0; i < minLength; i++)
                        { 
                            for(int j = 0; j < minLength; j++)
                            {
                                if (appointmentsList[i].DoctorID == (cardVetDetailsList[j].VetId).ToString())
                                {
                                    appointmentsList[i].VetSpecialization = cardVetDetailsList[j].Specialization;
                                    appointmentsList[i].DoctorName = cardVetDetailsList[j].Name;
                                    appointmentsList[i].DoctorPhoto = cardVetDetailsList[j].Photo;
                                }
                            }
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
                    var response = await httpClient.PostAsync("https://petzeypetwebapi20240505153103.azurewebsites.net/api/pets/getPetsByIDs", requestContent);

                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content
                        var responseContent = await response.Content.ReadAsStringAsync();

                        // Deserialize the JSON response to an array of CardVetDetailsDto
                        var cardPetDetailsList = JsonConvert.DeserializeObject<List<CardPetDetailsDto>>(responseContent);

                        // Process the data as needed
                        int minLength = Math.Min(appointmentsList.Count, cardPetDetailsList.Count);
                        for (int i = 0; i < minLength; i++)
                        {
                            int id = appointmentsList[i].PetID;
                            for (int j = 0; j < minLength; j++)
                            {
                                if (appointmentsList[i].PetID == cardPetDetailsList[j].PetID)
                                {
                                    appointmentsList[i].PetName = cardPetDetailsList[j].PetName;
                                    appointmentsList[i].PetGender = cardPetDetailsList[j].PetGender;
                                    appointmentsList[i].PetPhoto = cardPetDetailsList[j].petImage;
                                    appointmentsList[i].PetAge = cardPetDetailsList[j].PetAge;
                                    appointmentsList[i].OwnerID = cardPetDetailsList[j].OwnerID;
                                }
                            }
                        }
                    }
                }
                //ownername to be assigned!!!


                //hit a get request to https://petzeybackendappointmentapi20240502214622.azurewebsites.net//api/Auth and fetch it's data and store into a variable

                using (var httpClient = new HttpClient())
                {
                    // Make the GET request
                    var response = await httpClient.GetAsync("https://petzeybackendappointmentapi20240505153736.azurewebsites.net//api/Auth");

                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content
                        var responseContent = await response.Content.ReadAsStringAsync();

                        // Deserialize the JSON response to a Dictionary<string, string>
                        var ownerData = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);

                        // Find the owner name based on ownerId
                        foreach (var appointment in appointmentsList)
                        {
                            if (appointment.OwnerID != null && ownerData.ContainsKey(appointment.OwnerID))
                            {
                                appointment.OwnerName = ownerData[appointment.OwnerID];
                            }
                            else
                            {
                                // Handle the case where OwnerID is null or not found in the dictionary
                                // For example, set a default owner name or log a warning
                                appointment.OwnerName = "Unknown Owner";
                            }
                        }
                    }
                }


                return Ok(appointmentsList.Skip(offset).Take(3)); //3 appointments per page    
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

                // Now if you need appointments as List, you can convert it
                List<AppointmentCardDto> appointmentsList = appointments.ToList();
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
                    // Convert docIds to JSON string
                    var docIdsJson = JsonConvert.SerializeObject(docIds);

                    // Create the request content
                    var requestContent = new StringContent(docIdsJson, Encoding.UTF8, "application/json");

                    // Set the required headers if any
                    // httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "YourToken");

                    // Make the POST request
                    var response = await httpClient.PostAsync("https://petzyvetapi20240505160604.azurewebsites.net/api/vets/VetDetails", requestContent);

                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content
                        var responseContent = await response.Content.ReadAsStringAsync();

                        // Deserialize the JSON response to an array of CardVetDetailsDto
                        var cardVetDetailsList = JsonConvert.DeserializeObject<List<CardVetDetailsDto>>(responseContent);

                        // Process the data as needed
                        int minLength = Math.Min(appointmentsList.Count, cardVetDetailsList.Count);
                        for (int i = 0; i < minLength; i++)
                        {
                            for (int j = 0; j < minLength; j++)
                            {
                                if (appointmentsList[i].DoctorID == (cardVetDetailsList[j].VetId).ToString())
                                {
                                    appointmentsList[i].VetSpecialization = cardVetDetailsList[j].Specialization;
                                    appointmentsList[i].DoctorName = cardVetDetailsList[j].Name;
                                    appointmentsList[i].DoctorPhoto = cardVetDetailsList[j].Photo;
                                }
                            }
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
                    var response = await httpClient.PostAsync("https://petzeypetwebapi20240505153103.azurewebsites.net/api/pets/getPetsByIDs", requestContent);

                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content
                        var responseContent = await response.Content.ReadAsStringAsync();

                        // Deserialize the JSON response to an array of CardVetDetailsDto
                        var cardPetDetailsList = JsonConvert.DeserializeObject<List<CardPetDetailsDto>>(responseContent);

                        // Process the data as needed
                        int minLength = Math.Min(appointmentsList.Count, cardPetDetailsList.Count);
                        for (int i = 0; i < minLength; i++)
                        {
                            int id = appointmentsList[i].PetID;
                            for (int j = 0; j < minLength; j++)
                            {
                                if (appointmentsList[i].PetID == cardPetDetailsList[j].PetID)
                                {
                                    appointmentsList[i].PetName = cardPetDetailsList[j].PetName;
                                    appointmentsList[i].PetGender = cardPetDetailsList[j].PetGender;
                                    appointmentsList[i].PetPhoto = cardPetDetailsList[j].petImage;
                                    appointmentsList[i].PetAge = cardPetDetailsList[j].PetAge;
                                    appointmentsList[i].OwnerID = cardPetDetailsList[j].OwnerID;
                                }
                            }
                        }
                    }
                }
                //ownername to be assigned!!!

                using (var httpClient = new HttpClient())
                {
                    // Make the GET request
                    var response = await httpClient.GetAsync("https://petzeybackendappointmentapi20240505153736.azurewebsites.net//api/Auth");

                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content
                        var responseContent = await response.Content.ReadAsStringAsync();

                        // Deserialize the JSON response to a Dictionary<string, string>
                        var ownerData = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);

                        // Find the owner name based on ownerId
                        foreach (var appointment in appointmentsList)
                        {
                            if (appointment.OwnerID != null && ownerData.ContainsKey(appointment.OwnerID))
                            {
                                appointment.OwnerName = ownerData[appointment.OwnerID];
                            }
                            else
                            {
                                // Handle the case where OwnerID is null or not found in the dictionary
                                // For example, set a default owner name or log a warning
                                appointment.OwnerName = "Unknown Owner";
                            }
                        }
                    }
                }



                return Ok(appointmentsList.Skip(offset).Take(3)); //3 appointments per page    
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

                // Now if you need appointments as List, you can convert it
                List<AppointmentCardDto> appointmentsList = appointments.ToList();
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
                    // Convert docIds to JSON string
                    var docIdsJson = JsonConvert.SerializeObject(docIds);

                    // Create the request content
                    var requestContent = new StringContent(docIdsJson, Encoding.UTF8, "application/json");

                    // Set the required headers if any
                    // httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "YourToken");

                    // Make the POST request
                    var response = await httpClient.PostAsync("https://petzyvetapi20240505160604.azurewebsites.net/api/vets/VetDetails", requestContent);

                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content
                        var responseContent = await response.Content.ReadAsStringAsync();

                        // Deserialize the JSON response to an array of CardVetDetailsDto
                        var cardVetDetailsList = JsonConvert.DeserializeObject<List<CardVetDetailsDto>>(responseContent);

                        // Process the data as needed
                        int minLength = Math.Min(appointmentsList.Count, cardVetDetailsList.Count);
                        for (int i = 0; i < minLength; i++)
                        {
                            for (int j = 0; j < minLength; j++)
                            {
                                if (appointmentsList[i].DoctorID == (cardVetDetailsList[j].VetId).ToString())
                                {
                                    appointmentsList[i].VetSpecialization = cardVetDetailsList[j].Specialization;
                                    appointmentsList[i].DoctorName = cardVetDetailsList[j].Name;
                                    appointmentsList[i].DoctorPhoto = cardVetDetailsList[j].Photo;
                                }
                            }
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
                    var response = await httpClient.PostAsync("https://petzeypetwebapi20240505153103.azurewebsites.net/api/pets/getPetsByIDs", requestContent);

                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content
                        var responseContent = await response.Content.ReadAsStringAsync();

                        // Deserialize the JSON response to an array of CardVetDetailsDto
                        var cardPetDetailsList = JsonConvert.DeserializeObject<List<CardPetDetailsDto>>(responseContent);

                        // Process the data as needed
                        int minLength = Math.Min(appointmentsList.Count, cardPetDetailsList.Count);
                        for (int i = 0; i < minLength; i++)
                        {
                            int id = appointmentsList[i].PetID;
                            for(int j=0; j<minLength; j++)
                            {
                                if (appointmentsList[i].PetID == cardPetDetailsList[j].PetID)
                                {
                                    appointmentsList[i].PetName = cardPetDetailsList[j].PetName;
                                    appointmentsList[i].PetGender = cardPetDetailsList[j].PetGender;
                                    appointmentsList[i].PetPhoto = cardPetDetailsList[j].petImage;
                                    appointmentsList[i].PetAge = cardPetDetailsList[j].PetAge;
                                    appointmentsList[i].OwnerID = cardPetDetailsList[j].OwnerID;
                                }
                            }
                        }

                    }
                }

                using (var httpClient = new HttpClient())
                {
                    // Make the GET request
                    var response = await httpClient.GetAsync("https://petzeybackendappointmentapi20240505153736.azurewebsites.net//api/Auth");

                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content
                        var responseContent = await response.Content.ReadAsStringAsync();

                        // Deserialize the JSON response to a Dictionary<string, string>
                        var ownerData = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);

                        // Find the owner name based on ownerId
                        foreach (var appointment in appointmentsList)
                        {
                            if (appointment.OwnerID != null && ownerData.ContainsKey(appointment.OwnerID))
                            {
                                appointment.OwnerName = ownerData[appointment.OwnerID];
                            }
                            else
                            {
                                // Handle the case where OwnerID is null or not found in the dictionary
                                // For example, set a default owner name or log a warning
                                appointment.OwnerName = "Unknown Owner";
                            }
                        }
                    }
                }


                return Ok(appointmentsList.Skip(offset).Take(3)); //3 appointments per page    
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
    }
}
