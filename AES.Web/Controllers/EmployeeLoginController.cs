using AES.Web.Models;
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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(EmployeeLoginModel user)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please enter all your information.");
                return View(user);
            }

            ModelState.AddModelError("", "Invalid Login.");
            return View(user);
        }

    }
}