using AES.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AES.Web.Authorization
{
    class AESAuthorizeAttribute : AuthorizeAttribute
    {
        public string BadRedirectURL { get; set; }
        public EmployeeRole Role { get; set; } 

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if(Role != 0)
            {
                Roles = Role.ToString();
            }

            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (AuthorizeCore(filterContext.HttpContext))
            {
                HttpCachePolicyBase cachePolicy =
                    filterContext.HttpContext.Response.Cache;
                cachePolicy.SetProxyMaxAge(new TimeSpan(0));
            }

            /// This code added to support custom Unauthorized pages.
            else if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                if (BadRedirectURL != null)
                    filterContext.Result = new RedirectResult(BadRedirectURL);
                else
                    // Redirect to Login page.
                    HandleUnauthorizedRequest(filterContext);
            }
            /// End of additional code
            else
            {
                // Redirect to Login page.
                HandleUnauthorizedRequest(filterContext);
            }
        }

    }
}
