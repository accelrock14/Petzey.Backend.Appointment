using Azure.Identity;
using Microsoft.Graph.Beta;
using Microsoft.Graph.Beta.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _tenantId;
        private readonly string[] _scopes;

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
        Task<User[]> GetUsersAsync();
    }
    public class GraphServiceClientImplementation : IGraphServiceClient
    {
        private readonly IGraphServiceClientFactory _graphServiceClientFactory;

        public GraphServiceClientImplementation(IGraphServiceClientFactory graphServiceClientFactory)
        {
            _graphServiceClientFactory = graphServiceClientFactory;
        }

        public async Task<User[]> GetUsersAsync()
        {
            var graphService = _graphServiceClientFactory.CreateGraphServiceClient();
            var users = await graphService.Users.GetAsync();
            return users.Value.ToArray();
        }
    }

}
