using AES.Shared;
using AES.Shared.Contracts;
using AES.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace AES.Web.Authorization
{
    public class EmployeeUserManager
    {
        // http://stackoverflow.com/questions/31584506/how-to-implement-custom-authentication-in-asp-net-mvc-5

        public static bool LoginUser(EmployeeLoginModel EmployeeUser)
        {
            var s = new SecurityService.SecuritySvcClient();

            var valid = s.ValidateEmployeeUser(new EmployeeCredentialsContract()
            {
                Email = EmployeeUser.Email,
                Password = EmployeeUser.Password,
            });

            s.Close();

            if (valid != null)
            {
                var identity = new ClaimsIdentity(
                    new[]
                    {
                        // adding following 2 claim just for supporting default antiforgery provider
                        new Claim(ClaimTypes.Email, valid.Email.ToString()),
                        new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),

                        new Claim(ClaimTypes.Name, valid.FirstName),
                        new Claim(ClaimTypes.GivenName, valid.LastName),
                        // new Claim(ClaimTypes.Role, "Applicant")
                        new Claim(ClaimTypes.Role, valid.Role.ToString())
                    },
                    DefaultAuthenticationTypes.ApplicationCookie
                );

                // Actually sign the user in
                HttpContext.Current.GetOwinContext().Authentication.SignIn(new AuthenticationProperties { IsPersistent = false }, identity);

                return true;
            }

            return false;
        }

        public static EmployeeLoginModel GetUser()
        {
            var claims = ((ClaimsIdentity)HttpContext.Current.User.Identity).Claims;

            var EmployeeUser = new EmployeeLoginModel()
            {
                FirstName = claims.First(n => n.Type == ClaimTypes.Name).Value,
                LastName = claims.First(n => n.Type == ClaimTypes.Name).Value,
                Email = claims.First(n => n.Type == ClaimTypes.Email).Value,
                EmployeeRole = (EmployeeRole)Enum.Parse(typeof(EmployeeRole), claims.First(n => n.Type == ClaimTypes.Role).Value)
                //EmployeeRole = claims.First(n => n.Type == ClaimTypes.Role).Value.ToString()
            };

            return EmployeeUser;
        }

        public static EmployeeRole? GetEmployeeRole()
        {
            try
            {
                return (EmployeeRole)Enum.Parse(typeof(EmployeeRole), (((ClaimsIdentity)HttpContext.Current.User.Identity).Claims.First(id => id.Type == ClaimTypes.Role).Value));
                //return (((ClaimsIdentity)HttpContext.Current.User.Identity).Claims.First(id => id.Type == ClaimTypes.Role).Value);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void Logout()
        {
            HttpContext.Current.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }
    }
}