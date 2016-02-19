using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AES.Web.Startup))]
namespace AES.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
