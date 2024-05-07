using Azure.Identity;
using Microsoft.Graph.Beta;
using Microsoft.Graph.Beta.Models;
using Petzey.Backend.Appointment.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Backend.Appointment.Data.Repository
{
    public interface IGraphServiceClientFactory
    {
        GraphServiceClient CreateGraphServiceClient();
    }

    public class GraphServiceClientFactory : IGraphServiceClientFactory
    {
        public readonly string _clientId;
        public readonly string _clientSecret;
        public readonly string _tenantId;
        public readonly string[] _scopes;

        public GraphServiceClientFactory()
        {
            _clientId = ConfigurationManager.AppSettings["clientId"];
            _clientSecret = ConfigurationManager.AppSettings["clientSecret"];
            _tenantId = ConfigurationManager.AppSettings["tenantId"];
            _scopes = new[] { "https://graph.microsoft.com/.default" };
        }

        public GraphServiceClient CreateGraphServiceClient()
        {
            var clientSecretCredential = new ClientSecretCredential(_tenantId, _clientId, _clientSecret);
            var graphService = new GraphServiceClient(clientSecretCredential, _scopes);
            return graphService;
        }
    }
    
    public interface IGraphServiceClient
    {
        Task<List<Users>> GetUsersAsync();
    }
    public class GraphServiceClientImplementation : IGraphServiceClient
    {
        private readonly IGraphServiceClientFactory _graphServiceClientFactory;

        public GraphServiceClientImplementation(IGraphServiceClientFactory graphServiceClientFactory)
        {
            _graphServiceClientFactory = graphServiceClientFactory;
        }

        public async Task<List<Users>> GetUsersAsync()
        {
            var graphService = _graphServiceClientFactory.CreateGraphServiceClient();
            var users = await graphService.Users.GetAsync(); 
            var data = users.Value.ToArray();
            List<Users> usersList = new List<Users>();
            user_class user_Class = new user_class();

            foreach (var userData in data)
            {
                Users user = new Users
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

            return usersList;
        }
    }

}
