using Azure.Identity;
using Microsoft.Graph;
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


        public async Task<IHttpActionResult> GetUser()
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
                    keyValuePairs[data[i].Id] = data[i].DisplayName;
                }
                return Ok(keyValuePairs);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

    }
}
