using AES.SecuritySvc.Contracts;
using AES.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Linq;
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

            var valid = s.ValidateUser(
                new ApplicantInfoContract(
                    user.FirstName, user.LastName, user.SSN, user.DOB,
                    new DateTime(1970, 1, 1, 17, 0, 0), new DateTime(1970, 1, 1, 20, 0, 0)
                    )
                );

            s.Close();

            if (valid != null)
            {
                var identity = new ClaimsIdentity(
                    new[]
                    {
                        // adding following 2 claim just for supporting default antiforgery provider
                        new Claim(ClaimTypes.NameIdentifier, valid.FirstName),
                        new Claim(ClaimTypes.GivenName, valid.LastName),
                        new Claim(ClaimTypes.DateOfBirth, valid.DOB.ToString()),
                        new Claim(ClaimTypes.SerialNumber, valid.UserID.ToString()),
                        // new Claim(ClaimTypes.PrimarySid, valid.SSN),
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

        public static ApplicantLoginModel GetUser()
        {
            var claims = ((ClaimsIdentity)HttpContext.Current.User.Identity).Claims;
            /*
            foreach(var claim in claims)
            {
                claim.s
            }*/


            var user = new ApplicantLoginModel() { 
                FirstName = claims.First(n => n.Type == ClaimTypes.NameIdentifier).Value,
                LastName = claims.First(n => n.Type == ClaimTypes.GivenName).Value,
                DOB = DateTime.Parse(claims.First(n => n.Type == ClaimTypes.DateOfBirth).Value)
            };

            return user;
        }
    }
}
