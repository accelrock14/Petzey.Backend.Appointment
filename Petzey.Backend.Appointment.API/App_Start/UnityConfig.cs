using Petzey.Backend.Appointment.Domain.Interfaces;
using System.Web.Http;
using Unity;
using Unity.WebApi;
using Petzey.Backend.Appointment.Data.Repository;

namespace Petzey.Backend.Appointment.API
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<IAppointmentRepository, AppointmentRepository>();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}