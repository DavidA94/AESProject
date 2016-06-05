using AES.Shared;
using AES.Shared.Contracts;
using AES.Web.Authorization;
using AES.Web.Models;
using AES.Web.SecurityService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AES.Web.Controllers
{
    public class EmployeeLoginController : Controller
    {
        // GET: EmployeeLogin
        public ActionResult Login()
        {
            if (Request.IsAuthenticated)
            {
                EmployeeUserManager.Logout();
                return RedirectToActionPermanent("Login");
            }

            ViewBag.returnUrl = HttpContext.Request.QueryString["ReturnUrl"];
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(EmployeeLoginModel user, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please enter all your information.");
                return View(user);
            }

            var login = EmployeeUserManager.LoginUser(user);

            if(login == LoginResult.MUST_RESET) {
                TempData["resetEmail"] = user.Email;
                return RedirectToActionPermanent("ResetPassword");
            }
            else if (login == LoginResult.GOOD)
            {
                // If they tried to access somewhere else first, redirect to there
                if(returnUrl != null)
                {
                    return Redirect(returnUrl);
                }

                // Otherwise, redirect based on their role
                switch(EmployeeUserManager.GetEmployeeRole())
                {
                    case EmployeeRole.HqHiringSpecialist:
                        return RedirectToAction("DashboardHS", "HiringSpecialist");
                    case EmployeeRole.HqQStaffingExpert:
                        return RedirectToAction("Dashboard", "Staffing");
                    case EmployeeRole.StoreManager:
                        return RedirectToAction("Requests", "Manager");
                    case EmployeeRole.HiringManager:
                        return RedirectToAction("DashboardHM", "HiringManager");
                    case EmployeeRole.ITSpecialist:
                        return RedirectToAction("Dashboard", "IT");
                    default:
                        return RedirectToAction("Login", "EmployeeLogin");
                }
            }

            ModelState.AddModelError("", "Invalid Login.");
            return View(user);
        }

        public ActionResult ResetPassword()
        {
            if(Request.IsAuthenticated)
            {
                EmployeeUserManager.Logout();
                TempData["resetEmail"] = TempData["resetEmail"];
                return RedirectToActionPermanent("ResetPassword");
            }

            var e = new EmployeeLoginModel() { Email = TempData["resetEmail"].ToString() };

            return View(e);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(EmployeeLoginModel e)
        {
            // Don't care about the password field
            ModelState["Password"].Errors.Clear();

            if (!ModelState.IsValid)
            {
                return View(e);
            }

            using (var s = new SecuritySvcClient())
            {
                if(s.UpdateUserPassword(new EmployeeCredentialsContract() { Email = e.Email, Password = e.OldPassword }, e.NewPassword))
                {
                    e.Password = e.NewPassword;
                    return Login(e, null);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid information entered");
                    return View(e);
                }
            }
        }

        public ActionResult ForgotPassword()
        {
            if(Request.IsAuthenticated)
            {
                EmployeeUserManager.Logout();
                return RedirectToActionPermanent("ForgotPassword");
            }

            ViewBag.SuccessfulReset = false;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(EmployeeLoginModel e)
        {
            if (string.IsNullOrWhiteSpace(e.Email))
            {
                return View();
            }

            using (var s = new SecuritySvcClient())
            {
                if(s.ForgotPassword(new EmployeeCredentialsContract() { Email = e.Email }))
                {
                    ViewBag.SuccessfulReset = true;
                    return View();
                }
                else
                {
                    ModelState.AddModelError("", "Invalid email entered");
                    return View();
                }
            }
        }
    }
}