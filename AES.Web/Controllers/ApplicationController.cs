using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AES.Web.Models;
using AES.Web.Authorization;

namespace AES.Web.Controllers
{
    public class ApplicationController : Controller
    {
        // GET: Application
        public ActionResult Profile()
        {
            var x = FillProfileData();
            return View(x);

            //return View((ProfileViewModel)TempData["NewProfile"] ?? new ProfileViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Profile(ProfileViewModel info)
        {
            return ValidateRedirect(info, "NewProfile", "Availability");
        }

       
        // Temporary hardcoded model
        public ProfileViewModel FillProfileData()
        {
            ProfileViewModel tempProfile = new ProfileViewModel();

            // Profile Section
            tempProfile.FirstName = "Hatim";
            tempProfile.LastName = "Painter";
            tempProfile.SSN = "123-12-1234";
            tempProfile.DOB = new DateTime(2012, 12, 21);

            // Availability Section
            tempProfile.AvailabilityViewModel.MondayStartTime = new TimeSpan(12, 12, 12);
            tempProfile.AvailabilityViewModel.MondayEndTime = new TimeSpan(12, 12, 12);

            //Work History Section
            tempProfile.WorkHistoryViewModel.Employer = "Intel";

            //Education Section
            tempProfile.EducationViewModel.InstitutionName = "OIT";

            //References Section
            tempProfile.ReferencesViewModel.Company = "Some Company";
            
            return tempProfile;
        }

        public ActionResult Availability()
        {
            return View((AvailabilityViewModel)TempData["NewAvailability"]??new AvailabilityViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Availability(AvailabilityViewModel info)
        {
            return ValidateRedirect(info, "NewAvailability", "WorkHistory");
        }

        public ActionResult WorkHistory()
        {
            return View((WorkHistoryViewModel)TempData["NewWorkHistory"]);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult WorkHistory(WorkHistoryViewModel info)
        {
            return ValidateRedirect(info, "NewWorkHistory", "Education");
        }

        public ActionResult Education()
        {
            return View((EducationViewModel)TempData["NewEducation"] ?? new EducationViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Education(EducationViewModel info)
        {
            return ValidateRedirect(info, "NewEducation", "References");
        }

        public ActionResult References()
        {
            return View((ReferencesViewModel)TempData["NewReferences"] ?? new ReferencesViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult References(ReferencesViewModel info)
        {
            return ValidateRedirect(info, "NewReferences", "NextSection");
        }

        public ActionResult NextSection()
        {
            return View((ProfileViewModel)TempData["NewProfile"] ?? new ProfileViewModel());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info">current view model</param>
        /// <param name="tempData">Index for temp data</param>
        /// <param name="nextView">string for next page</param>
        /// <returns>An action result which redirects</returns>
        private ActionResult ValidateRedirect(ApplicationViewModel info, string tempData, string nextView)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please enter all your information.");
                return View(info);
            }

            TempData[tempData] = info;

            return RedirectToAction(nextView, "Application");
        }
    }
}