using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AES.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Get the directory we're starting in
            DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

            // Loop until we find the folder that holds AES.Web
            while (dir.GetDirectories().FirstOrDefault(d => d.Name == "AES.Web") == null)
            {
                dir = dir.Parent;
            }

            // Set the DataDirectory
            AppDomain.CurrentDomain.SetData("DataDirectory", dir.FullName);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //Database.SetInitializer<AESDbContext>(null);
        }
    }
}
