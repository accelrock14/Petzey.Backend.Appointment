using Petzey.Backend.Appointment.Data;
using Petzey.Backend.Appointment.Domain.Interfaces;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace Petzey.Backend.Appointment.API
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IRepo, PetzeyDbContext>();
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}