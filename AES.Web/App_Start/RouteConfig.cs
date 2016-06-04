using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AES.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // As stupid as it is, less specific to more specific. Order is important!

            routes.MapRoute(
                name: "Login",
                url: "Login",
                defaults: new { controller = "ApplicantLogin", action = "Login" }
            );

            routes.MapRoute(
                name: "EmployeeLogin",
                url: "EmployeeLogin",
                defaults: new { controller = "EmployeeLogin", action = "Login" }
            );

            routes.MapRoute(
                name: "ForgotPassword",
                url: "ForgotPassword",
                defaults: new { controller = "EmployeeLogin", action = "ForgotPassword" }
            );

            routes.MapRoute(
                name: "ResetPassword",
                url: "ResetPassword",
                defaults: new { controller = "EmployeeLogin", action = "ResetPassword" }
            );

            routes.MapRoute(
                name: "AvailableJobs",
                url: "AvailableJobs",
                defaults: new { controller = "JobOpenings", action = "AvailableJobs" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "HomePage", id = UrlParameter.Optional }
            );
        }
    }
}
