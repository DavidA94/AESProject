﻿using AES.Web.OpeningService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AES.Web.Models;
using AES.Web.ApplicationService;
using AES.Shared;
using System.Net;
using Microsoft.AspNet.Identity;
using AES.Web.Authorization;

namespace AES.Web.Controllers
{
    public class JobOpeningsController : Controller
    {
        // GET: JobOpenings
        public ActionResult AvailableJobs()
        {
            // Anytime we hit this, we want the user logged out
            ApplicantUserManager.Logout();

            // Open the service
            using (var JobOpenings = new OpeningSvcClient())
            {
                // Get the query string, or use 1 by default
                string storeID = Request.QueryString["store"] ?? "1";

                // Get all the openings for the given store
                JobOpeningContract[] getOpening = JobOpenings.GetApprovedOpenings(Convert.ToInt32(storeID));

                // Create a new list of openings
                List<JobOpeningsViewModel> OpeningList = new List<JobOpeningsViewModel>();

                // Loop through what we got and add them to the list
                foreach (var o in getOpening)
                {
                    OpeningList.Add(new JobOpeningsViewModel()
                    {
                        Title = o.title,
                        ShortDesc = o.ShortDescription,
                        JobID = o.JobID,
                        LongDesc = o.LongDescription,
                        StoreID = o.StoreID
                    });
                }

                return View(OpeningList);
            }
        }

        [HttpPost]
        public ActionResult AvailableJobs(IEnumerable<JobOpeningsViewModel> model)
        {
            // Get the jobs that were selected
            var selectedJobs = model.Where(c => c.JobCheckbox == true).Select(m => new Tuple<int, int>(m.JobID, m.StoreID)).ToArray();

            // If there were none, then error back to the user
            if (selectedJobs.Length == 0)
            {
                ModelState.AddModelError("", "Please select at least one job.");
                return View(model);
            }

            // Remember the jobs in a session variables
            Session["SelectedJobs"] = selectedJobs;

            // Redirect to the login page
            return RedirectToAction("Login", "ApplicantLogin");
        }
    }
}