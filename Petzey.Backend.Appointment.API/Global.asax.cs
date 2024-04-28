using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using NLog;

namespace Petzey.Backend.Appointment.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            UnityConfig.RegisterComponents();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            // Nlog related code given by chatgpt
            // Other initialization code like RouteConfig, BundleConfig, etc.
            //GlobalConfiguration.Configure(WebApiConfig.Register);
            // this line is only once so it gave exception so I commented

            // Configure NLog
            LogManager.LoadConfiguration(Server.MapPath("~/NLog.config"));
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            LogManager.GetCurrentClassLogger().Error(exception, "Unhandled exception");
        }

    }
}
