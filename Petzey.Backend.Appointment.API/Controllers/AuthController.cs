using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Beta;
using Petzey.Backend.Appointment.Data.Repository;
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
        public readonly IGraphServiceClient _graphServiceClient;

        public AuthController(IGraphServiceClient graphServiceClient)
        {
            _graphServiceClient = graphServiceClient;
        }

        [HttpGet]
        [Route("api/getalluserobjects")]

        public async Task<IHttpActionResult> GetAllUser()
        {
            try
            {
                var users = await _graphServiceClient.GetUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                return InternalServerError(ex);
            }
        }

    }
}
