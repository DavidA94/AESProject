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

            IApplicationSvc appSvc = new ApplicationSvcClient();

            UserInfoContract CallingApplicants = appSvc.GetApplicantsAwaitingCalls();

            var HiringSpecialist = new HiringSpecialistModel()
            {
                ETA = CallingApplicants.EndCallTime
            };

            return View(HiringSpecialist);
        }
        
        // Hardcoded HiringSpecialistViewModel
        public List<HiringSpecialistModel> FillHSData()
        {
            return new List<HiringSpecialistModel>()
            {
                new HiringSpecialistModel()
                {
                    ETA = new TimeSpan(12,12,12),
                    FirstName = "John",
                    LastName = "Smith",
                },

                new HiringSpecialistModel()
                {
                    ETA = new TimeSpan(11,11,11),
                    FirstName = "Awesome",
                    LastName = "Fun",
                }
            };
        }

        public ActionResult CallPage()
        {
            return View();
        }

    }
}