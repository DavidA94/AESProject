using AES.Shared.Contracts;
using AES.Web.ApplicationService;
using AES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AES.Web.Controllers
{
    public class HiringSpecialistController : Controller
    {
        // GET: HiringSpecialist
        public ActionResult DashboardHS()
        {
            var x = FillHSData();

            return View(x);

            //IApplicationSvc appSvc = new ApplicationSvcClient();

            //// Get the application (will come back with historical data)
            //ApplicationInfoContract app = appSvc.GetApplication(new ApplicantInfoContract()
            //{
            //    UserID = userID
            //});
        }
           
        public HiringSpecialistModel FillHSData()
        {
            HiringSpecialistModel tempHSData = new HiringSpecialistModel();

            tempHSData.FirstName = "Awesome";
            tempHSData.LastName = "Fun";

            tempHSData.ETA = new TimeSpan(12,12,12);

            return tempHSData;
        }
            
        public ActionResult CallPage()
        {
            return View();
        }

    }
}