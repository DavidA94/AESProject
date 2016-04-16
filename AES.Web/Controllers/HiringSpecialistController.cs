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
            
        //List<ApplicantInfoContract> testList = new List<ApplicantInfoContract>();

        //testList.Add(new ApplicantInfoContract()
        //{

        //    UserInfo = new UserInfoContract()
        //    {
        //        Nickname = "John Smith";
        //        EndCallTime = new TimeSpan(12,12,12); 
        //    }
        //});

        //List<ApplicantInfoModel> models = new List<ApplicantInfoModel>();

        //foreach (var applicant in testList)
        //{
        //    models.Add(new ApplicantInfoModel()
        //    {
        //        FirstName = applicant.FirstName,
        //        LastName = applicant.FirstName,
        //        ETA = yadadadad
        //    }
        //    );
        //}

        public ActionResult CallPage()
        {
            return View();
        }

    }
}