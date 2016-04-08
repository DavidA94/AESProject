using AES.Web.Authorization;
using AES.Web.Models;
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
            
            if (ApplicantUserManager.LoginUser(user))
            {
                return RedirectToAction("UserProfile", "Application");
            }

            ModelState.AddModelError("", "Invalid Login.");
            return View(user);
        }

        [AESAuthorize]
        public ActionResult Welcome()
        {
            return View(ApplicantUserManager.GetUser());
        }
    }
}