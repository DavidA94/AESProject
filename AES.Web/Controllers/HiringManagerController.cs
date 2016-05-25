using AES.Shared;
using AES.Shared.Contracts;
using AES.Web.ApplicationService;
using AES.Web.Authorization;
using AES.Web.JobbingService;
using AES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AES.Web.Controllers
{
    //[AESAuthorize(BadRedirectURL = "/EmployeeLogin", Role = EmployeeRole.HiringManager)]
    public class HiringManagerController : Controller
    {
        // GET: HiringManager
        public ActionResult DashboardHM()
        {
            IApplicationSvc appSvc = new ApplicationSvcClient();

            // ** Get the store ID from the database**********
            //ApplicantInfoContract[] InterviewApplicants = appSvc.GetApplicantsAwaitingInterview(EmployeeUserManager.GetUser().StoreID);
            ApplicantInfoContract[] InterviewApplicants = appSvc.GetApplicantsAwaiting(1, AppStatus.WAITING_INTERVIEW);

            List<HiringManagerModel> ConvertedContract = ConvertContractToModel(InterviewApplicants);

            return View(ConvertedContract);

        }

        [HttpPost]
        public ActionResult DashboardHM(string InterviewList)
        {
            if (InterviewList == "Applicant Interview List")
            {
                return RedirectToAction("InterviewList");
            }

            return View();
        }

        public ActionResult ApplicantInformationList()
        {
            IApplicationSvc appSvc = new ApplicationSvcClient();

            // ** Get the store ID from the database**********
            //ApplicantInfoContract[] InterviewApplicants = appSvc.GetApplicantsAwaitingInterview(EmployeeUserManager.GetUser().StoreID);
            ApplicantInfoContract[] InterviewApplicants = appSvc.GetApplicantsAwaiting(1, AppStatus.WAITING_INTERVIEW);
            List<HiringManagerModel> ConvertedContract = ConvertContractToModel(InterviewApplicants);

            return View(ConvertedContract);
        }

        public ActionResult ApplicantsAwaitingInterview()
        {
            IApplicationSvc appSvc = new ApplicationSvcClient();

            // ** Get the store ID from the database**********
            //ApplicantInfoContract[] InterviewApplicants = appSvc.GetApplicantsAwaitingInterview(EmployeeUserManager.GetUser().StoreID);
            ApplicantInfoContract[] InterviewApplicants = appSvc.GetApplicantsAwaiting(1, AppStatus.WAITING_INTERVIEW);
            List<HiringManagerModel> ConvertedContract = ConvertContractToModel(InterviewApplicants);

            return View(ConvertedContract);

            //var x = FillInterviewList();

            //return View(x);
        }

        public ActionResult HiredApplicants()
        {
            IApplicationSvc appSvc = new ApplicationSvcClient();

            // ** Get the store ID from the database**********
            //ApplicantInfoContract[] InterviewApplicants = appSvc.GetApplicantsAwaitingInterview(EmployeeUserManager.GetUser().StoreID);
            ApplicantInfoContract[] InterviewApplicants = appSvc.GetApplicantsAwaiting(1, AppStatus.APPROVED);
            List<HiringManagerModel> ConvertedContract = ConvertContractToModel(InterviewApplicants);

            return View(ConvertedContract);
        }

        public ActionResult ApplicantsAwaitingDecision()
        {
            IApplicationSvc appSvc = new ApplicationSvcClient();

            // ** Get the store ID from the database**********
            //ApplicantInfoContract[] InterviewApplicants = appSvc.GetApplicantsAwaitingInterview(EmployeeUserManager.GetUser().StoreID);
            ApplicantInfoContract[] InterviewApplicants = appSvc.GetApplicantsAwaiting(1, AppStatus.INTERVIEW_COMPLETE);
            List<HiringManagerModel> ConvertedContract = ConvertContractToModel(InterviewApplicants);

            return View(ConvertedContract);
        }

        public ActionResult ApplicantsNotAccepted()
        {
            IApplicationSvc appSvc = new ApplicationSvcClient();

            // ** Get the store ID from the database**********
            //ApplicantInfoContract[] InterviewApplicants = appSvc.GetApplicantsAwaitingInterview(EmployeeUserManager.GetUser().StoreID);
            ApplicantInfoContract[] InterviewApplicants = appSvc.GetApplicantsAwaiting(1, AppStatus.DENIED);
            List<HiringManagerModel> ConvertedContract = ConvertContractToModel(InterviewApplicants);

            return View(ConvertedContract);
        }

        public ActionResult RequestPositions()
        {
            using (var jobSvc = new JobbingSvcClient())
            {
                var positions = jobSvc.GetJobs();

                List<JobModel> ConvertedContract = ConvertContractToModel(positions);

                return View(ConvertedContract);
            }

        }

        public ActionResult InterviewList()
        {
            IApplicationSvc appSvc = new ApplicationSvcClient();

            // ** Get the store ID from the database**********
            ApplicantInfoContract[] InterviewApplicants = appSvc.GetApplicantsAwaiting(EmployeeUserManager.GetUser().StoreID, AppStatus.WAITING_INTERVIEW);
            List<HiringManagerModel> ConvertedContract = ConvertContractToModel(InterviewApplicants);

            return View(ConvertedContract);

            //var x = FillInterviewList();

            //return View(x);

        }

        [HttpPost]
        public ActionResult InterviewList(int ApplicantID, string ApplicantStatus, string InterviewNotes)
        {
            IApplicationSvc appSvc = new ApplicationSvcClient();

            if (ApplicantStatus == "Hire")
            {
                appSvc.SaveInterview(ApplicantID, InterviewNotes, AppStatus.APPROVED);
            }
            else if (ApplicantStatus == "Deny")
            {
                appSvc.SaveInterview(ApplicantID, InterviewNotes, AppStatus.DENIED);
            }
            else
            {
                appSvc.SaveInterview(ApplicantID, InterviewNotes, AppStatus.INTERVIEW_COMPLETE);
            }

            return View();
        }

        [HttpPost]
        public ActionResult FullApplicantInfo(int ApplicantID)
        {
            IApplicationSvc appSvc = new ApplicationSvcClient();

            // ** Change this waiting call status to awaiting calls!
            ApplicationInfoContract App = appSvc.GetApplication(ApplicantID, Shared.AppStatus.WAITING_INTERVIEW);

            FullApplicationModel ConvertedFullAppModel = ConvertAppContractToModel(App);

            return View(ConvertedFullAppModel);
        }


        [HttpPost]
        public ActionResult FullApplicationCollapse(int ApplicantID)
        {
            IApplicationSvc appSvc = new ApplicationSvcClient();

            // ** Change this waiting call status to awaiting interview!
            ApplicationInfoContract App = appSvc.GetApplication(ApplicantID, Shared.AppStatus.WAITING_INTERVIEW);

            FullApplicationModel ConvertedFullAppModel = ConvertAppContractToModel(App);

            //Keep track of the applicant ID for hire or deny buttons
            ConvertedFullAppModel.ApplicantID = ApplicantID;

            return View(ConvertedFullAppModel);
        }

        // Hardcoded HiringManagerViewModel
        public List<HiringManagerModel> FillInterviewList()
        {
            return new List<HiringManagerModel>()
            {
                new HiringManagerModel()
                {
                    FirstName = "John",
                    LastName = "Smith",
                    ApplicantID = 1
                },

                new HiringManagerModel()
                {
                    FirstName = "Awesome",
                    LastName = "Fun",
                    ApplicantID = 2
                },

                new HiringManagerModel()
                {
                    FirstName = "Awesome",
                    LastName = "Fun",
                    ApplicantID = 3
                },

                new HiringManagerModel()
                {
                    FirstName = "Awesome",
                    LastName = "Fun",
                    ApplicantID = 3
                },

                 new HiringManagerModel()
                {
                    FirstName = "Awesome",
                    LastName = "Fun",
                    ApplicantID = 3
                },

                  new HiringManagerModel()
                {
                    FirstName = "Awesome",
                    LastName = "Fun",
                    ApplicantID = 3
                },

                   new HiringManagerModel()
                {
                    FirstName = "Awesome",
                    LastName = "Fun",
                    ApplicantID = 3
                },

                new HiringManagerModel()
                {
                    FirstName = "Awesome",
                    LastName = "Fun",
                    ApplicantID = 4
                }
            };
        }



        #region Converters

        #region Contract to Model

        private List<HiringManagerModel> ConvertContractToModel(IEnumerable<ApplicantInfoContract> ApplicantInfo)
        {
            var RetAppInfo = new List<HiringManagerModel>();

            foreach (var AppInfo in ApplicantInfo)
            {
                RetAppInfo.Add(new HiringManagerModel()
                {
                    FirstName = AppInfo.FirstName,
                    LastName = AppInfo.LastName,
                    ApplicantID = (int)AppInfo.UserID
                });
            }

            return RetAppInfo;
        }

        public FullApplicationModel ConvertAppContractToModel(ApplicationInfoContract app)
        {
            return new FullApplicationModel
            {
                Profile = new ProfileViewModel()
                {
                    Address = app.UserInfo.Address,
                    City = app.UserInfo.City,
                    DOB = app.DOB,
                    EndCallTime = app.UserInfo.EndCallTime,
                    FirstName = app.FirstName,
                    LastName = app.LastName,
                    Nickname = app.UserInfo.Nickname,
                    Phone = app.UserInfo.Phone,
                    SalaryExpectation = app.UserInfo.SalaryExpectation,
                    StartCallTime = app.UserInfo.StartCallTime,
                    State = app.UserInfo.State,
                    Zip = app.UserInfo.Zip
                },

                Availibility = new AvailabilityViewModel()
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
                },

                WorkHistory = ConvertContractToModel(app.Jobs),

                Education = ConvertContractToModel(app.Educations),

                References = ConvertContractToModel(app.References),

                Questionnaire = ConvertContractToModel(app.QA)

            };
        }

        private List<WorkHistoryViewModel> ConvertContractToModel(IEnumerable<JobHistoryContract> Jobs)
        {
            var RetJobs = new List<WorkHistoryViewModel>();

            foreach (var Job in Jobs)
            {
                RetJobs.Add(new WorkHistoryViewModel()
                {
                    EmployerAddress = Job.EmployerAddress,
                    EmployerCity = Job.EmployerCity,
                    EmployerCountry = Job.EmployerCountry,
                    EmployerName = Job.EmployerName,
                    EmployerPhone = Job.EmployerPhone,
                    EmployerState = Job.EmployerState,
                    EmployerZip = Job.EmployerZip,
                    EndingSalary = Job.EndingSalary,
                    ReasonForLeaving = Job.ReasonForLeaving,
                    Responsibilities = Job.Responsibilities,
                    StartingSalary = Job.StartingSalary,
                    SupervisorName = Job.SupervisorName,
                    WorkedFrom = Job.StartDate,
                    WorkedTo = Job.EndDate
                });
            }

            return RetJobs;
        }

        private List<EducationViewModel> ConvertContractToModel(IEnumerable<EducationHistoryContract> EdHis)
        {
            var RetEdHis = new List<EducationViewModel>();

            foreach (var ed in EdHis)
            {
                RetEdHis.Add(new EducationViewModel()
                {
                    Degree = ed.Degree,
                    GraduationDate = ed.Graduated,
                    Major = ed.Major,
                    SchoolAddress = ed.SchoolAddress,
                    SchoolCity = ed.SchoolCity,
                    SchoolCountry = ed.SchoolCountry,
                    SchoolName = ed.SchoolName,
                    SchoolState = ed.SchoolState,
                    SchoolZip = ed.SchoolZIP,
                    YearAttended = ed.YearsAttended
                });
            }

            return RetEdHis;
        }

        private List<ReferencesViewModel> ConvertContractToModel(IEnumerable<ReferenceContract> References)
        {
            var RetReferences = new List<ReferencesViewModel>();

            foreach (var Ref in References)
            {
                RetReferences.Add(new ReferencesViewModel()
                {
                    Company = Ref.Company,
                    Name = Ref.Name,
                    Phone = Ref.Phone,
                    Title = Ref.Title
                });
            }

            return RetReferences;
        }

        private List<QuestionnaireViewModel> ConvertContractToModel(IEnumerable<QAContract> QA)
        {
            var RetQA = new List<QuestionnaireViewModel>();

            foreach (var ques in QA)
            {
                RetQA.Add(new QuestionnaireViewModel()
                {
                    MC_Answers = ques.MC_Answers,
                    Options = ques.Options,
                    Question = ques.Question,
                    QuestionID = ques.QuestionID,
                    ShortAnswer = ques.ShortAnswer,
                    Type = ques.Type
                });
            }

            return RetQA;
        }

        private List<JobModel> ConvertContractToModel(IEnumerable<JobContract> jobs)
        {
            List<JobModel> retList = new List<JobModel>();

            foreach (var job in jobs)
            {
                retList.Add(new JobModel()
                {
                    JobID = job.JobID,
                    LongDescription = job.LongDescription,
                    ShortDescription = job.ShortDescription,
                    Title = job.Title
                });
            }

            return retList;
        }

        #endregion

        #region Model to Contract

        private JobContract ConvertModelToContract(JobModel job)
        {
            return new JobContract()
            {
                JobID = job.JobID,
                LongDescription = job.LongDescription,
                ShortDescription = job.ShortDescription,
                Title = job.Title
            };
        }

        #endregion

        #endregion
    }
}