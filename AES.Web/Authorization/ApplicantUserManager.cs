using AES.Shared.Contracts;
using AES.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;

namespace AES.Web.Authorization
{
    class ApplicantUserManager
    {
        // http://stackoverflow.com/questions/31584506/how-to-implement-custom-authentication-in-asp-net-mvc-5

        public static bool LoginUser(ApplicantLoginModel user)
        {
            var s = new SecurityService.SecuritySvcClient();

            var valid = s.ValidateUser(new ApplicantInfoContract(user.FirstName, user.LastName, user.SSN, user.DOB));

            s.Close();

            if (valid != null)
            {
                var identity = new ClaimsIdentity(
                    new[]
                    {
                        // adding following 2 claim just for supporting default antiforgery provider
                        new Claim(ClaimTypes.Name, valid.FirstName),
                        new Claim(ClaimTypes.GivenName, valid.LastName),
                        new Claim(ClaimTypes.DateOfBirth, valid.DOB.ToString()),
                        new Claim(ClaimTypes.SerialNumber, valid.UserID.ToString()),
                        // new Claim(ClaimTypes.PrimarySid, valid.SSN),
                        new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),

                        new Claim(ClaimTypes.NameIdentifier, valid.UserID.ToString()),
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

        public static ApplicantLoginModel GetUser()
        {
            var claims = ((ClaimsIdentity)HttpContext.Current.User.Identity).Claims;

            var user = new ApplicantLoginModel() { 
                FirstName = claims.First(n => n.Type == ClaimTypes.Name).Value,
                LastName = claims.First(n => n.Type == ClaimTypes.GivenName).Value,
                DOB = DateTime.Parse(claims.First(n => n.Type == ClaimTypes.DateOfBirth).Value)
            };

            return user;
        }

        public static int GetUserID()
        {
            try {
                return Convert.ToInt32(((ClaimsIdentity)HttpContext.Current.User.Identity).Claims.First(id => id.Type == ClaimTypes.NameIdentifier).Value);
            }
            catch(Exception ex)
            {
                return -1;
            }
        }

        public static void Logout()
        {
            HttpContext.Current.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }
    }
}
