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
        [HttpPost]
        [Route("api/dashboard/statuscounts/")]
        public IHttpActionResult GetStatusCounts(IDFiltersDto ids)
        {
            try
            {
                var status = repo.AppointmentStatusCounts(ids);
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
        [Route("api/dashboard/appointments/filter/{offset?}/{take?}")]
        public async Task<IHttpActionResult> GetAllAppointmentsWithFilters(FilterParamsDto filters, int offset = 0, int take = 5)
        {
            try
            {
                IEnumerable<AppointmentCardDto> temp = repo.GetAllAppointmentsWithFilters(filters);
                IEnumerable<AppointmentCardDto> appointments = temp.Skip(offset).Take(take);

                // Now if you need appointments as List, you can convert it
                List<AppointmentCardDto> appointmentsList = appointments.ToList();
                foreach (var a in appointmentsList)
                {
                    a.All = temp.Count();
                }

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
                //using (var httpClient = new HttpClient())
                //{
                //    // Create request content with the JSON string and specify the content type
                //    var docIdsJson = JsonConvert.SerializeObject(docIds);
                //    var requestContent = new StringContent(docIdsJson, Encoding.UTF8, "application/json");
                //    var response = await httpClient.PostAsync("https://petzyvetapi20240505160604.azurewebsites.net/api/vets/VetDetails", requestContent);

                //    if (response.IsSuccessStatusCode)
                //    {
                //        var responseContent = await response.Content.ReadAsStringAsync();
                //        // Deserialize the JSON response to a list of CardVetDetailsDto objects
                //        var cardVetDetailsList = JsonConvert.DeserializeObject<List<CardVetDetailsDto>>(responseContent);

                //        foreach (var appointment in appointmentsList)
                //        {
                //            // If vet details are found, update the appointment object
                //            var doctorDetails = cardVetDetailsList.FirstOrDefault(d => (d.VetId).ToString() == appointment.DoctorID);
                //            if (doctorDetails != null)
                //            {
                //                appointment.VetSpecialization = doctorDetails.Specialization;
                //                appointment.DoctorName = doctorDetails.Name;
                //                appointment.DoctorPhoto = doctorDetails.Photo;
                //            }
                //        }
                //    }
                //}

                // Fetch pet details
                //using (var httpClient = new HttpClient())
                //{
                //    // Convert the list of doctor IDs to a JSON string
                //    var petIdsJson = JsonConvert.SerializeObject(petIds);
                //    var requestContent = new StringContent(petIdsJson, Encoding.UTF8, "application/json");
                //    var response = await httpClient.PostAsync("https://petzeypetwebapi20240505153103.azurewebsites.net/api/pets/getPetsByIDs", requestContent);

                //    if (response.IsSuccessStatusCode)
                //    {
                //        var responseContent = await response.Content.ReadAsStringAsync();
                //        var cardPetDetailsList = JsonConvert.DeserializeObject<List<CardPetDetailsDto>>(responseContent);
                //        //convert the above list to dictionary for fastly find petdetails by its ID
                //        var petDetailsDictionary = cardPetDetailsList.ToDictionary(p => p.PetID);

                //        foreach (var appointment in appointmentsList)
                //        {
                //            // If vet details are found, update the appointment object
                //            if (petDetailsDictionary.TryGetValue(appointment.PetID, out var petDetails))
                //            {
                //                appointment.PetName = petDetails.PetName;
                //                appointment.PetGender = petDetails.PetGender;
                //                appointment.PetPhoto = petDetails.petImage;
                //                appointment.PetAge = petDetails.PetAge;
                //                appointment.OwnerID = petDetails.OwnerID;
                //            }
                //        }
                //    }
                //}

                // Fetch owner details
                //using (var httpClient = new HttpClient())
                //{
                //    var response = await httpClient.GetAsync("https://petzeybackendappointmentapi20240505153736.azurewebsites.net/api/getalluseridsandname");

                //    if (response.IsSuccessStatusCode)
                //    {
                //        var responseContent = await response.Content.ReadAsStringAsync();
                //        var ownerData = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);

                //        foreach (var appointment in appointmentsList)
                //        {
                //            if (appointment.OwnerID != null && ownerData.ContainsKey(appointment.OwnerID))
                //            {
                //                appointment.OwnerName = ownerData[appointment.OwnerID];
                //            }
                //            else
                //            {
                //                appointment.OwnerName = "Unknown Owner";
                //            }
                //        }
                //    }
                //}

                // Paginate and return appointments
                return Ok(appointmentsList); // 4 appointments per page    
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
                IEnumerable<AppointmentCardDto> temp = repo.GetAppointmentsByOwnerIdWithFilters(filters, ownerid);
                IEnumerable<AppointmentCardDto> appointments = temp.Skip(offset).Take(4);

                // Now if you need appointments as List, you can convert it
                List<AppointmentCardDto> appointmentsList = appointments.ToList();
                foreach (var a in appointmentsList)
                {
                    a.All = temp.Count();
                }
                //var docIds = new List<int>();
                //foreach (var appointment in appointmentsList)
                //{
                //    if (int.TryParse(appointment.DoctorID, out int doctorId))
                //    {
                //        docIds.Add(doctorId);
                //    }
                //}
                //var petIds = new List<int>();
                //foreach (var appointment in appointmentsList)
                //{
                //    petIds.Add(appointment.PetID);
                //}
                //using (var httpClient = new HttpClient())
                //{
                //    var docIdsJson = JsonConvert.SerializeObject(docIds);
                //    var requestContent = new StringContent(docIdsJson, Encoding.UTF8, "application/json");
                //    var response = await httpClient.PostAsync("https://petzyvetapi20240505160604.azurewebsites.net/api/vets/VetDetails", requestContent);

                //    if (response.IsSuccessStatusCode)
                //    {
                //        var responseContent = await response.Content.ReadAsStringAsync();
                //        // Deserialize the JSON response to a list of CardVetDetailsDto objects
                //        var cardVetDetailsList = JsonConvert.DeserializeObject<List<CardVetDetailsDto>>(responseContent);

                //        // Iterate through each appointment to map corresponding vet details
                //        foreach (var appointment in appointmentsList)
                //        {
                //            var doctorDetails = cardVetDetailsList.FirstOrDefault(d => (d.VetId).ToString() == appointment.DoctorID);
                //            if (doctorDetails != null)
                //            {
                //                appointment.VetSpecialization = doctorDetails.Specialization;
                //                appointment.DoctorName = doctorDetails.Name;
                //                appointment.DoctorPhoto = doctorDetails.Photo;
                //            }
                //        }
                //    }
                //}

                //// Fetch pet details
                //using (var httpClient = new HttpClient())
                //{
                //    var petIdsJson = JsonConvert.SerializeObject(petIds);
                //    var requestContent = new StringContent(petIdsJson, Encoding.UTF8, "application/json");
                //    var response = await httpClient.PostAsync("https://petzeypetwebapi20240505153103.azurewebsites.net/api/pets/getPetsByIDs", requestContent);

                //    if (response.IsSuccessStatusCode)
                //    {
                //        var responseContent = await response.Content.ReadAsStringAsync();
                //        var cardPetDetailsList = JsonConvert.DeserializeObject<List<CardPetDetailsDto>>(responseContent);

                //        var petDetailsDictionary = cardPetDetailsList.ToDictionary(p => p.PetID);

                //        foreach (var appointment in appointmentsList)
                //        {
                //            // If pet details are found, update the appointment object
                //            if (petDetailsDictionary.TryGetValue(appointment.PetID, out var petDetails))
                //            {
                //                appointment.PetName = petDetails.PetName;
                //                appointment.PetGender = petDetails.PetGender;
                //                appointment.PetPhoto = petDetails.petImage;
                //                appointment.PetAge = petDetails.PetAge;
                //                appointment.OwnerID = petDetails.OwnerID;
                //            }
                //        }
                //    }
                //}

                //// Fetch owner details
                //using (var httpClient = new HttpClient())
                //{
                //    var response = await httpClient.GetAsync("https://petzeybackendappointmentapi20240505153736.azurewebsites.net/api/getalluseridsandname");

                //    if (response.IsSuccessStatusCode)
                //    {
                //        var responseContent = await response.Content.ReadAsStringAsync();
                //        var ownerData = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);

                //        foreach (var appointment in appointmentsList)
                //        {
                //            if (appointment.OwnerID != null && ownerData.ContainsKey(appointment.OwnerID))
                //            {
                //                appointment.OwnerName = ownerData[appointment.OwnerID];
                //            }
                //            else
                //            {
                //                appointment.OwnerName = "Unknown Owner";
                //            }
                //        }
                //    }
                //}

                return Ok(appointmentsList); // 4 appointments per page    
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
                IEnumerable<AppointmentCardDto> temp = repo.GetAppointmentsByVetIdWithFilters(filters, vetid);
                IEnumerable<AppointmentCardDto> appointments = temp.Skip(offset).Take(4);

                // Now if you need appointments as List, you can convert it
                List<AppointmentCardDto> appointmentsList = appointments.ToList();
                foreach (var a in appointmentsList)
                {
                    a.All = temp.Count();
                }
                //var docIds = new List<int>();
                //foreach (var appointment in appointmentsList)
                //{
                //    if (int.TryParse(appointment.DoctorID, out int doctorId))
                //    {
                //        docIds.Add(doctorId);
                //    }
                //}
                //var petIds = new List<int>();
                //foreach (var appointment in appointmentsList)
                //{
                //    petIds.Add(appointment.PetID);
                //}
                //Fetch Vet details
                //using (var httpClient = new HttpClient())
                //{
                //    var docIdsJson = JsonConvert.SerializeObject(docIds);
                //    var requestContent = new StringContent(docIdsJson, Encoding.UTF8, "application/json");
                //    var response = await httpClient.PostAsync("https://petzyvetapi20240505160604.azurewebsites.net/api/vets/VetDetails", requestContent);

                //    if (response.IsSuccessStatusCode)
                //    {
                //        var responseContent = await response.Content.ReadAsStringAsync();
                //        var cardVetDetailsList = JsonConvert.DeserializeObject<List<CardVetDetailsDto>>(responseContent);

                //        foreach (var appointment in appointmentsList)
                //        {
                //            var doctorDetails = cardVetDetailsList.FirstOrDefault(d => (d.VetId).ToString() == appointment.DoctorID);
                //            if (doctorDetails != null)
                //            {
                //                appointment.VetSpecialization = doctorDetails.Specialization;
                //                appointment.DoctorName = doctorDetails.Name;
                //                appointment.DoctorPhoto = doctorDetails.Photo;
                //            }
                //        }
                //    }
                //}

                // Fetch pet details
                //using (var httpClient = new HttpClient())
                //{
                //    var petIdsJson = JsonConvert.SerializeObject(petIds);
                //    var requestContent = new StringContent(petIdsJson, Encoding.UTF8, "application/json");
                //    var response = await httpClient.PostAsync("https://petzeypetwebapi20240505153103.azurewebsites.net/api/pets/getPetsByIDs", requestContent);

                //    if (response.IsSuccessStatusCode)
                //    {
                //        var responseContent = await response.Content.ReadAsStringAsync();
                //        var cardPetDetailsList = JsonConvert.DeserializeObject<List<CardPetDetailsDto>>(responseContent);

                //        var petDetailsDictionary = cardPetDetailsList.ToDictionary(p => p.PetID);

                //        foreach (var appointment in appointmentsList)
                //        {
                //            if (petDetailsDictionary.TryGetValue(appointment.PetID, out var petDetails))
                //            {
                //                appointment.PetName = petDetails.PetName;
                //                appointment.PetGender = petDetails.PetGender;
                //                appointment.PetPhoto = petDetails.petImage;
                //                appointment.PetAge = petDetails.PetAge;
                //                appointment.OwnerID = petDetails.OwnerID;
                //            }
                //        }
                //    }
                //}

                // Fetch owner details
                //using (var httpClient = new HttpClient())
                //{
                //    var response = await httpClient.GetAsync("https://petzeybackendappointmentapi20240505153736.azurewebsites.net/api/getalluseridsandname");

                //    if (response.IsSuccessStatusCode)
                //    {
                //        var responseContent = await response.Content.ReadAsStringAsync();
                //        // Deserialize the JSON response to a Dictionary<string, string>
                //        var ownerData = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);

                //        // Find the owner name based on ownerId
                //        foreach (var appointment in appointmentsList)
                //        {
                //            if (appointment.OwnerID != null && ownerData.ContainsKey(appointment.OwnerID))
                //            {
                //                appointment.OwnerName = ownerData[appointment.OwnerID];
                //            }
                //            else
                //            {
                //                appointment.OwnerName = "Unknown Owner";
                //            }
                //        }
                //    }
                //}

                return Ok(appointmentsList); // 4 appointments per page    
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                return InternalServerError();
            }
        }
        [HttpPost]
        [Route("api/dashboard/upcoming/")]
        public IHttpActionResult GetUpcomingAppointments(IDFiltersDto ids)
        {
            try
            {
                var appointments = repo.UpcomingAppointments(ids);
                if (appointments == null)
                    return NotFound();

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
