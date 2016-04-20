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

            ApplicantInfoContract[] CallingApplicants = appSvc.GetApplicantsAwaitingCalls(DateTime.Now);

            List<HiringSpecialistModel>  ConvertedContract = ConvertContractToModel(CallingApplicants);

           

            return View(ConvertedContract);
        }

        private List<HiringSpecialistModel> ConvertContractToModel(IEnumerable<ApplicantInfoContract> Info)
        {
            var RetInfo = new List<HiringSpecialistModel>();

            foreach (var AppInfo in Info)
            {
                RetInfo.Add(new HiringSpecialistModel()
                {
                    FirstName = AppInfo.FirstName,
                    LastName = AppInfo.LastName,
                    ETA = AppInfo.UserInfo.EndCallTime
                });
            }

            return RetInfo;
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