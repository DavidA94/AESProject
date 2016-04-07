using AES.Entities.Contexts;
using System;
using System.Data.Entity;
using System.IO;
using System.Threading;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AES.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            while (dir.Name != "AESProject")
            {
                dir = dir.Parent;
            }
            AppDomain.CurrentDomain.SetData("DataDirectory", dir.FullName);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //Database.SetInitializer<AESDbContext>(null);
        }
    }
}
