using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Beta;
using Petzey.Backend.Appointment.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Petzey.Backend.Appointment.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AuthController : ApiController
    {


       /* public async Task<IHttpActionResult> GetUser()
        {

            var clientId = ConfigurationManager.AppSettings["clientId"];
            var clientSecret = ConfigurationManager.AppSettings["clientSecret"];
            var tenantId = ConfigurationManager.AppSettings["tenantId"];
            var scopes = new[] { "https://graph.microsoft.com/.default" };
            // Create an instance of ClientSecretCredential
            var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);

            // Initialize the GraphServiceClient with the ClientSecretCredential
            var graphService = new GraphServiceClient(clientSecretCredential, scopes);

            try
            {
                // Retrieve user information using Microsoft Graph API
                var users = await graphService.Users.GetAsync();
                // Process the retrieved user information
                var data = users.Value.ToArray();

                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                for (int i = 0; i < data.Length; i++)
                {
                    keyValuePairs[data[i].Id] = data[i].DisplayName+" "+data[i].Surname;
                }
                return Ok(keyValuePairs);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
*/

       /* public async Task<IHttpActionResult> GetUserByID(string ID)
        {

            var clientId = ConfigurationManager.AppSettings["clientId"];
            var clientSecret = ConfigurationManager.AppSettings["clientSecret"];
            var tenantId = ConfigurationManager.AppSettings["tenantId"];
            var scopes = new[] { "https://graph.microsoft.com/.default" };
            // Create an instance of ClientSecretCredential
            var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);

            // Initialize the GraphServiceClient with the ClientSecretCredential
            var graphService = new GraphServiceClient(clientSecretCredential, scopes);

            try
            {
                // Retrieve user information using Microsoft Graph API
                var users = await graphService.Users.GetAsync();
                // Process the retrieved user information
                var data = users.Value.ToArray();

                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                for (int i = 0; i < data.Length; i++)
                {
                    keyValuePairs[data[i].Id] = data[i].DisplayName + " " + data[i].Surname;
                }

                if(keyValuePairs.ContainsKey(ID))
                return Ok(keyValuePairs[ID]);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
*/



        public async Task<IHttpActionResult> GetAllUser()
        {
            var clientId = ConfigurationManager.AppSettings["clientId"];
            var clientSecret = ConfigurationManager.AppSettings["clientSecret"];
            var tenantId = ConfigurationManager.AppSettings["tenantId"];
            var scopes = new[] { "https://graph.microsoft.com/.default" };

            // Create an instance of ClientSecretCredential
            var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);

            // Initialize the GraphServiceClient with the ClientSecretCredential
            var graphService = new GraphServiceClient(clientSecretCredential, scopes);

            try
            {
                var users = await graphService.Users.GetAsync();
                var data = users.Value.ToArray();
                List<User> usersList = new List<User>();
                user_class user_Class = new user_class();

                foreach (var userData in data)
                {
                    User user = new User
                    {
                        Id = userData.Id,
                        Name = userData.DisplayName,
                        City = userData.City,
                        State = userData.State,
                        Country = userData.Country,
                        Email = userData.Identities.FirstOrDefault()?.IssuerAssignedId?.ToString(),
                        Role = user_Class.GetRoleFromAdditionalData(userData.AdditionalData),
                        Npi = user_Class.GetNpiFromAdditionalData(userData.AdditionalData),
                        Phone = user_Class.GetPhoneFromAdditionalData(userData.AdditionalData),
                        Speciality = user_Class.GetSpecialityFromAdditionalData(userData.AdditionalData)
                    };

                    usersList.Add(user);
                }

                return Ok(usersList);
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the retrieval process
                return InternalServerError(ex);
            }
        }

    }
}
