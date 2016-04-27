using AES.Web.OpeningService;
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
        // How to set up a cookie 

        // GET: JobOpenings
        public ActionResult AvailableJobs()
        {
            // Anytime we hit this, we want the user logged out
            ApplicantUserManager.Logout();

            // Open the service
            IOpeningSvc JobOpenings = new OpeningSvcClient();

            // Get the query string, or use 1 by default
            string storeID = Request.QueryString["StoreID"] ?? "1";

            // Get all the openings for the given store
            var getOpening = JobOpenings.GetAllOpenings(Convert.ToInt32(storeID));

            // Create a new list of openings
            List<JobOpeningsViewModel> OpeningList = new List<JobOpeningsViewModel>();

            // Loop through what we got and add them to the list
            foreach (var o in getOpening)
            {
                OpeningList.Add(new JobOpeningsViewModel()
                {
                    Title = o.title, 
                    ShortDesc = o.ShortDescription, 
                    ID = o.OpeningID, 
                    LongDesc = o.LongDescription
                });
            }

            return View(OpeningList);
        }

        [HttpPost]
        public ActionResult AvailableJobs(IEnumerable<JobOpeningsViewModel> model)
        {
            // Get the jobs that were selected
            var selectedJobs = model.Where(c => c.JobCheckbox == true).Select(m => m.ID).ToArray();

            // If there were none, then error back to the user
            if (selectedJobs.Length == 0)
            {
                ModelState.AddModelError("", "Please select at least one job.");
                return View(model);
            }

            // Remember the jobs in a sessino variables
            Session["SelectedJobs"] = selectedJobs;

            // Redirect to the login page
            return RedirectToAction("Login", "ApplicantLogin");
        }
    }
}