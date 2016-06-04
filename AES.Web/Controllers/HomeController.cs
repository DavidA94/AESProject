using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AES.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult HomePage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult HomePage(string BeginButton)
        {
            if (BeginButton == "True")
            {
                return RedirectToAction("Login", "ApplicantLogin");
            }

            return View();
        }



    }
}