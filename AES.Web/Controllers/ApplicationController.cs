using AES.Shared;
using AES.Shared.Contracts;
using AES.Web.ApplicationService;
using AES.Web.Authorization;
using AES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AES.Web.Controllers
{
    public class ApplicationController : Controller
    {
        // GET: Application
        public ActionResult UserProfile()
        {
            // Get the seleceted jobs from the session
            int[] selectedJobs = (int[])Session["SelectedJobs"];
            int userID = ApplicantUserManager.GetUserID();

            if(userID < 0)
            {
                return RedirectToAction("AvailableJobs", "JobOpenings");
            }

            IApplicationSvc appSvc = new ApplicationSvcClient();

            // If we have jobs, start a new app, otherwise, assume the user is continuing a previous application
            if (selectedJobs != null && selectedJobs.Length > 0)
            {
                // Save the new application
                var response = appSvc.SavePartialApplication(new ApplicationInfoContract()
                {
                    ApplicantID = userID,
                    AppliedJobs = selectedJobs
                });

                // If we didn't get a good application back, go to the job openings page
                if (response != AppSvcResponse.GOOD)
                {
                    return RedirectToAction("AvailableJobs", "JobOpenings");
                }
            }

            // Get the application (will come back with historical data)
            ApplicationInfoContract app = appSvc.GetApplication(new ApplicantInfoContract()
            {
                UserID = userID
            });

            // Get the user that is logged in
            var user = ApplicantUserManager.GetUser();

            // Create the data to go into the profile view
            var profile = new ProfileViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                DOB = user.DOB,
                Nickname = app.UserInfo.Nickname,
                Address = app.UserInfo.Address,
                City = app.UserInfo.City,
                State = app.UserInfo.State,
                Zip = app.UserInfo.Zip == 0 ? (int?)null : app.UserInfo.Zip,
                Phone = app.UserInfo.Phone,
                SalaryExpectation = app.UserInfo.SalaryExpectation == 0 ? (decimal?)null : app.UserInfo.SalaryExpectation
            };

            return View(profile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserProfile(ProfileViewModel info)
        {
            return ValidateRedirect(info, "Availability");
        }



        public ActionResult Availability()
        {
            int userID = ApplicantUserManager.GetUserID();

            if (userID < 0)
            {
                return RedirectToActionPermanent("AvailableJobs", "JobOpenings");
            }

            // Get the application (will come back with historical data)
            IApplicationSvc appSvc = new ApplicationSvcClient();
            ApplicationInfoContract app = appSvc.GetApplication(new ApplicantInfoContract()
            {
                UserID = userID
            });

            if(app == null || app.UserInfo == null)
            {
                return RedirectToAction("AvailableJobs", "JobOpenings");
            }

            if (app.Availability != null) {

                var availability = new AvailabilityViewModel()
                {
                    SundayStart = app.Availability.SundayStart,
                    SundayEnd = app.Availability.SundayEnd,
                    MondayStart = app.Availability.MondayStart,
                    MondayEnd = app.Availability.MondayEnd,
                    TuesdayStart = app.Availability.TuesdayStart,
                    TuesdayEnd = app.Availability.TuesdayEnd,
                    WednesdayStart = app.Availability.WednesdayStart,
                    WednesdayEnd = app.Availability.WednesdayEnd,
                    ThursdayStart = app.Availability.ThursdayStart,
                    ThursdayEnd = app.Availability.ThursdayEnd,
                    FridayStart = app.Availability.FridayStart,
                    FridayEnd = app.Availability.FridayEnd,
                    SaturdayStart = app.Availability.SaturdayStart,
                    SaturdayEnd = app.Availability.SaturdayEnd
                };

                return View(availability);
            }

            return View(new AvailabilityViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Availability(AvailabilityViewModel info)
        {
            return ValidateRedirect(info, "WorkHistory");
        }


        
        public PartialViewResult GetWorkHistoryItem()
        {
            return PartialView("PartialWorkHistory", new WorkHistoryViewModel());
        }

        public ActionResult WorkHistory()
        {
            int userID = ApplicantUserManager.GetUserID();

            if (userID < 0)
            {
                return RedirectToActionPermanent("AvailableJobs", "JobOpenings");
            }

            // Get the application (will come back with historical data)
            IApplicationSvc appSvc = new ApplicationSvcClient();
            ApplicationInfoContract app = appSvc.GetApplication(new ApplicantInfoContract()
            {
                UserID = userID
            });

            if (app == null || app.UserInfo == null)
            {
                return RedirectToAction("AvailableJobs", "JobOpenings");
            }

            if (app.Jobs != null && app.Jobs.Length > 0)
            {
                var pastJobs = new List<WorkHistoryViewModel>();

                foreach (var job in app.Jobs)
                {
                    pastJobs.Add(new WorkHistoryViewModel()
                    {
                        EmployerAddress = job.EmployerAddress,
                        EmployerCity = job.EmployerCity,
                        EmployerName = job.EmployerName,
                        EmployerCountry = job.EmployerCountry,
                        EndingSalary = job.EndingSalary > 0 ? job.EndingSalary : (decimal?)null,
                        EmployerPhone = job.EmployerPhone,
                        ReasonForLeaving = job.ReasonForLeaving,
                        Responsibilities = job.Responsibilities,
                        StartingSalary = job.StartingSalary > 0 ? job.StartingSalary : (decimal?)null,
                        EmployerState = job.EmployerState,
                        SupervisorName = job.SupervisorName,
                        WorkedFrom = job.StartDate != new DateTime(1970, 1, 1) ? job.StartDate : (DateTime?)null,
                        WorkedTo = job.EndDate != new DateTime(1970, 1, 1) ? job.EndDate : (DateTime?)null,
                        EmployerZip = job.EmployerZip < 100 ? (int?)null : job.EmployerZip
                    });
                }

                return View(pastJobs);
            }

            return View(new List<WorkHistoryViewModel> { new WorkHistoryViewModel() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult WorkHistory(IEnumerable<WorkHistoryViewModel> info)
        {
            return ValidateRedirect(info, "Education");
        }



        public ActionResult GetEducationHistoryItem()
        {
            return PartialView("PartialEducationHistory", new EducationViewModel());
        }

        public ActionResult Education()
        {
            int userID = ApplicantUserManager.GetUserID();

            if (userID < 0)
            {
                return RedirectToActionPermanent("AvailableJobs", "JobOpenings");
            }

            // Get the application (will come back with historical data)
            IApplicationSvc appSvc = new ApplicationSvcClient();
            ApplicationInfoContract app = appSvc.GetApplication(new ApplicantInfoContract()
            {
                UserID = userID
            });

            if (app == null || app.UserInfo == null)
            {
                return RedirectToAction("AvailableJobs", "JobOpenings");
            }

            if (app.Educations != null && app.Educations.Length > 0)
            {
                var pastEds = new List<EducationViewModel>();

                foreach (var ed in app.Educations)
                {
                    pastEds.Add(new EducationViewModel()
                    {
                        SchoolAddress = ed.SchoolAddress,

                        SchoolCity = ed.SchoolCity,
                        Degree = ed.Degree,
                        SchoolCountry = ed.SchoolCountry,
                        GraduationDate = ed.Graduated != new DateTime(1970, 1, 1) ? ed.Graduated : (DateTime?)null,
                        SchoolName = ed.SchoolName,
                        Major = ed.Major,
                        SchoolState = ed.SchoolState,
                        YearAttended = ed.YearsAttended > 0 ? ed.YearsAttended : (double?)null,
                        SchoolZip = ed.SchoolZIP > 0 ? ed.SchoolZIP : (int?)null
                    });
                }

                return View(pastEds);
            }

            return View(new List<EducationViewModel> { new EducationViewModel() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Education(IEnumerable<EducationViewModel> info)
        {
            return ValidateRedirect(info, "References");
        }



        public ActionResult GetReferenceItem()
        {
            return PartialView("PartialReference", new ReferencesViewModel());
        }

        public ActionResult References()
        {
            int userID = ApplicantUserManager.GetUserID();

            if (userID < 0)
            {
                return RedirectToActionPermanent("AvailableJobs", "JobOpenings");
            }

            // Get the application (will come back with historical data)
            IApplicationSvc appSvc = new ApplicationSvcClient();
            ApplicationInfoContract app = appSvc.GetApplication(new ApplicantInfoContract()
            {
                UserID = userID
            });

            if (app == null || app.UserInfo == null)
            {
                return RedirectToAction("AvailableJobs", "JobOpenings");
            }

            if (app.References != null && app.References.Length > 0)
            {
                var refs = new List<ReferencesViewModel>();

                foreach (var r in app.References)
                {
                    refs.Add(new ReferencesViewModel()
                    {
                        Company = r.Company,
                        Phone = r.Phone,
                        Name = r.Name,
                        Title = r.Title
                    });
                }

                return View(refs);
            }

            return View(new List<ReferencesViewModel> { new ReferencesViewModel() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult References(IEnumerable<ReferencesViewModel> info)
        {
            return ValidateRedirect(info, "Questionaire");
        }



        public ActionResult Questionaire()
        {
            int userID = ApplicantUserManager.GetUserID();

            if (userID < 0)
            {
                return RedirectToActionPermanent("AvailableJobs", "JobOpenings");
            }

            // Get the application (will come back with historical data)
            IApplicationSvc appSvc = new ApplicationSvcClient();
            ApplicationInfoContract app = appSvc.GetApplication(new ApplicantInfoContract()
            {
                UserID = userID
            });

            if (app == null || app.UserInfo == null)
            {
                return RedirectToAction("AvailableJobs", "JobOpenings");
            }

            if (app.QA != null && app.QA.Length > 0)
            {
                var questions = new List<QuestionnaireViewModel>();

                foreach (var q in app.QA)
                {
                    questions.Add(new QuestionnaireViewModel()
                    {
                        MC_Answers = q.MC_Answers.ToList(),
                        Options = q.Options.ToList(),
                        Question = q.Question,
                        QuestionID = q.QuestionID,
                        RadioOption = q.Type == QuestionType.RADIO ? "i" + q.QuestionID.ToString() + (questions.Count - 1).ToString() : "",
                        ShortAnswer = q.ShortAnswer,
                        Type = q.Type
                    });
                }

                return View(questions);
            }

            return View(new List<QuestionnaireViewModel>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Questionaire(IEnumerable<QuestionnaireViewModel> info)
        {

            // Get any radio button values to be put back into the list
            foreach(var radio in info.Where(r => r.Type == QuestionType.RADIO))
            {
                radio.MC_Answers = new List<bool>() { false, false, false, false };
                if (radio.RadioOption != null)
                {
                    radio.MC_Answers[Convert.ToInt32(radio.RadioOption.Last().ToString())] = true;
                }
            }

            var valid = ValidateRedirect(info, "");

            if(valid is ViewResult)
            {
                return View(info);
            }

            int userID = ApplicantUserManager.GetUserID();

            if (userID < 0)
            {
                return RedirectToActionPermanent("AvailableJobs", "JobOpenings");
            }

            // Call out and get the current application
            IApplicationSvc appSvc = new ApplicationSvcClient();
            var app = appSvc.GetApplication(new ApplicantInfoContract() { UserID = userID });

            // Attempt to save the application
            var result = appSvc.SubmitApplication(new ApplicantInfoContract() { UserID = app.ApplicantID });

            if (result)
            {
                return RedirectToAction("ApplicationSubmitted");
            }

            ModelState.AddModelError("", "There was an issue submitting your application. Please try again");
            return View(info);
        }

        public ActionResult ApplicationSubmitted()
        {
            // Anytime we hit this, we want the user logged out
            ApplicantUserManager.Logout();

            return View();
        }



        /// <summary>
        /// Validates the data and redirects to the next page
        /// </summary>
        /// <param name="info">current view model</param>
        /// <param name="nextView">string for next page</param>
        /// <returns>An action result which redirects</returns>
        private ActionResult ValidateRedirect(IApplicationViewModel info, string nextView)
        {
            var result = ValidateRedirect(new List<IApplicationViewModel> { info }, nextView);
            if(result.GetType() == typeof(ViewResult))
            {
                result = View(((result as ViewResult).Model as IEnumerable<IApplicationViewModel>).FirstOrDefault());
            }
            return result;
        }

        /// <summary>
        /// Validates the data and redirects to the next page
        /// </summary>
        /// <param name="info">current view model</param>
        /// <param name="nextView">string for next page</param>
        /// <returns>An action result which redirects</returns>
        private ActionResult ValidateRedirect(IEnumerable<IApplicationViewModel> info, string nextView)
        {
            if (info == null || info.FirstOrDefault() == null || !ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please enter all your information.");
                return View(info);
            }

            int userID = ApplicantUserManager.GetUserID();

            if (userID < 0)
            {
                return RedirectToActionPermanent("AvailableJobs", "JobOpenings");
            }

            // Call out and get the current application
            IApplicationSvc appSvc = new ApplicationSvcClient();
            var app = appSvc.GetApplication(new ApplicantInfoContract() { UserID = userID });

            // Add the data to the app
            info.First().AddData(info, ref app);

            // Save the application
            appSvc.SavePartialApplication(app);

            // Go to the next page
            return RedirectToAction(nextView, "Application");
        }

    }
}