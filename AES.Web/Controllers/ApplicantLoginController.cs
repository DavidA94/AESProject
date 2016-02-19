using AES.SecuritySvc;
using AES.SecuritySvc.Contracts;
using AES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AES.Web.Controllers
{
    public class ApplicantLoginController : Controller
    {
        // GET: ApplicantLogin
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(ApplicantLoginModel user)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please enter all your information.");
                return View(user);
            }

            Security s = new Security();
            var userLogin = new UserInfoContract()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                DOB = user.DOB,
                SSN = user.SSN
            };

            if (s.ValidateUser(userLogin) != null)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid Login.");
            return View(user);
        }
    }
}