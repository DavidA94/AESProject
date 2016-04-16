using AES.Web.ApplicationService;
using AES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AES.Web.Controllers
{
    public class FullApplicationController : Controller
    {
        // GET: FullApplication
        public ActionResult Index()
        {
            return View();
        }

        public FullApplicationModel FillFullApplication(ApplicationInfoContract app)
        {
            return new FullApplicationModel
            {
                Profile = new ProfileViewModel()
                {
                    Address = app.UserInfo.Address,
                    City = app.UserInfo.City,
                }
            };
        }
    }
}