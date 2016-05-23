using AES.Shared;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TwitterBootstrapMVC;

namespace AES.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            DBFileManager.SetDataDirectory();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
