using AES.SecuritySvc;
using AES.SecuritySvc.Contracts;
using AES.Web.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace AES.Web.UserManagers
{
    class ApplicantUserManager
    {
        // http://stackoverflow.com/questions/31584506/how-to-implement-custom-authentication-in-asp-net-mvc-5

        public bool LoginUser(ApplicantLoginModel user)
        {
            Security s = new Security();

            var valid = s.ValidateUser(new UserInfoContract(user.FirstName, user.LastName, user.SSN, user.DOB));

            if (valid != null)
            {
                var identity = new ClaimsIdentity(
                    new[]
                    {
                        // adding following 2 claim just for supporting default antiforgery provider
                        new Claim(ClaimTypes.NameIdentifier, valid.UserID.ToString()),
                        new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),

                        new Claim(ClaimTypes.Name, valid.UserID.ToString()),
                        // new Claim(ClaimTypes.Role, "Applicant")
                    },
                    DefaultAuthenticationTypes.ApplicationCookie
                );

                // Actually sign the user in
                HttpContext.Current.GetOwinContext().Authentication.SignIn(new AuthenticationProperties { IsPersistent = false }, identity);

                return true;
            }

            return false;
        }
    }
}
