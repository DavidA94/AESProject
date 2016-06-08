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
using System.Reflection;
using AES.Web.OpeningService;

namespace AES.Web.Controllers
{
   
    [AESAuthorize(BadRedirectURL = "/EmployeeLogin", Role = EmployeeRole.HiringManager)]
    public class HiringManagerController : Controller
    {
        // GET: HiringManager
        public ActionResult DashboardHM()
        {
            return View();

        }

        public ActionResult ApplicantInformationList()
        {
            IApplicationSvc appSvc = new ApplicationSvcClient();

            // ** Get the store ID from the database**********
            ApplicantInfoContract[] ApplicantInformation = appSvc.GetApplicantsAwaiting(EmployeeUserManager.GetUser().StoreID, AppStatus.WAITING_INTERVIEW);

            //ApplicantInfoContract[] ApplicantInformation = appSvc.GetApplicantsAwaiting(1, AppStatus.WAITING_INTERVIEW);
            List<HiringManagerModel> ConvertedContract = ConvertContractToModel(ApplicantInformation);

            return View(ConvertedContract);
        }

        public ActionResult ApplicantsAwaitingInterview()
        {
            IApplicationSvc appSvc = new ApplicationSvcClient();

            // ** Get the store ID from the database**********
            ApplicantInfoContract[] AppAwaitInter = appSvc.GetApplicantsAwaiting(EmployeeUserManager.GetUser().StoreID, AppStatus.WAITING_INTERVIEW);

            //ApplicantInfoContract[] AppAwaitInter = appSvc.GetApplicantsAwaiting(1, AppStatus.WAITING_INTERVIEW);
            List<HiringManagerModel> ConvertedContract = ConvertContractToModel(AppAwaitInter);

            return View(ConvertedContract);
        }

        public ActionResult HiredApplicants()
        {
            IApplicationSvc appSvc = new ApplicationSvcClient();

            // ** Get the store ID from the database**********
            ApplicantInfoContract[] HiredAppl = appSvc.GetApplicantsAwaiting(EmployeeUserManager.GetUser().StoreID, AppStatus.APPROVED);
            //ApplicantInfoContract[] HiredAppl = appSvc.GetApplicantsAwaiting(1, AppStatus.APPROVED);
            List<HiringManagerModel> ConvertedContract = ConvertContractToModel(HiredAppl);

            return View(ConvertedContract);
        }

        public ActionResult ApplicantsAwaitingDecision()
        {
            IApplicationSvc appSvc = new ApplicationSvcClient();

            // ** Get the store ID from the database**********
            ApplicantInfoContract[] ApplAwaitDec = appSvc.GetApplicantsAwaiting(EmployeeUserManager.GetUser().StoreID, AppStatus.INTERVIEW_COMPLETE);
            //ApplicantInfoContract[] ApplAwaitDec = appSvc.GetApplicantsAwaiting(1, AppStatus.INTERVIEW_COMPLETE);
            List<HiringManagerModel> ConvertedContract = ConvertContractToModel(ApplAwaitDec);

            return View(ConvertedContract);
        }

        public ActionResult ApplicantsNotAccepted()
        {
            IApplicationSvc appSvc = new ApplicationSvcClient();

            // ** Get the store ID from the database**********
            ApplicantInfoContract[] ApplNotAccepted = appSvc.GetApplicantsAwaiting(EmployeeUserManager.GetUser().StoreID, AppStatus.DENIED);

            //ApplicantInfoContract[] ApplNotAccepted = appSvc.GetApplicantsAwaiting(1, AppStatus.DENIED);
            List<HiringManagerModel> ConvertedContract = ConvertContractToModel(ApplNotAccepted);

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

        [HttpPost]
        public ActionResult RequestPositions(string RequestNotes, int JobID, int NumOfPos )
        {
            IOpeningSvc opnSvc = new OpeningSvcClient();

            JobOpeningContract OpeningContract = new JobOpeningContract()
            {
                RequestNotes = RequestNotes,
                JobID = JobID,
                Positions = NumOfPos,
                StoreID = EmployeeUserManager.GetUser().StoreID
            };

            opnSvc.RequestOpenings(EmployeeUserManager.GetUser().StoreID, OpeningContract, NumOfPos);

            return RedirectToActionPermanent("DashboardHM");
        }

        [HttpPost]
        public ActionResult StaticApplicationHM(int ApplicantID, string ApplicantStatus)
        {
            IApplicationSvc appSvc = new ApplicationSvcClient();

            AppStatus whatToGet;

            if (ApplicantStatus == "Hired")
            {
                whatToGet = AppStatus.APPROVED;

                
            }
            else if (ApplicantStatus == "Denied")
            {
                whatToGet = AppStatus.DENIED;
            }
            else
            {
                whatToGet = AppStatus.WAITING_INTERVIEW;
            }

            ApplicationInfoContract App = appSvc.GetApplication(ApplicantID, whatToGet);

            FullApplicationModel ConvertedFullAppModel = ConvertAppContractToModel(App);
            ConvertedFullAppModel.ApplicantID = ApplicantID;

            using (var oSvc = new OpeningSvcClient())
            {
                ConvertedFullAppModel.JobTitle = oSvc.GetJobName(App.AppliedJobs.FirstOrDefault()?.Item1 ?? -1);
            }

            return View(ConvertedFullAppModel);
        }


        [HttpPost]
        public ActionResult Interview(int ApplicantID, string ApplicantStatus, string InterviewNotes)
        {
            IApplicationSvc appSvc = new ApplicationSvcClient();

            if (ApplicantStatus == "Hire")
            {
                appSvc.SaveInterview(ApplicantID, InterviewNotes, AppStatus.APPROVED);
                return Redirect(Url.Action("DashboardHM", "HiringManager") + "#InterviewSection");
            }
            else if (ApplicantStatus == "Deny")
            {
                appSvc.SaveInterview(ApplicantID, InterviewNotes, AppStatus.DENIED);
                return Redirect(Url.Action("DashboardHM", "HiringManager") + "#InterviewSection");
            }
            else if (ApplicantStatus == "DecideLater")
            {
                appSvc.SaveInterview(ApplicantID, InterviewNotes, AppStatus.INTERVIEW_COMPLETE);
                return Redirect(Url.Action("DashboardHM", "HiringManager") + "#InterviewSection");
            }
            else if (ApplicantStatus == "Decide")
            {
                ApplicationInfoContract App = appSvc.GetApplication(ApplicantID, Shared.AppStatus.INTERVIEW_COMPLETE);

                FullApplicationModel ConvertedFullAppModel = ConvertAppContractToModel(App);

                ConvertedFullAppModel.ApplicantID = ApplicantID;

                using (var oSvc = new OpeningSvcClient())
                {
                    ConvertedFullAppModel.JobTitle = oSvc.GetJobName(App.AppliedJobs.FirstOrDefault()?.Item1 ?? -1);
                }

                return View(ConvertedFullAppModel);
            }
            else
            {
                ApplicationInfoContract App = appSvc.GetApplication(ApplicantID, Shared.AppStatus.WAITING_INTERVIEW);

                FullApplicationModel ConvertedFullAppModel = ConvertAppContractToModel(App);

                ConvertedFullAppModel.ApplicantID = ApplicantID;

                using (var oSvc = new OpeningSvcClient())
                {
                    ConvertedFullAppModel.JobTitle = oSvc.GetJobName(App.AppliedJobs.FirstOrDefault()?.Item1 ?? -1);
                }

                return View(ConvertedFullAppModel);
            }
          
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

                Questionnaire = ConvertContractToModel(app.QA),

                InterviewNotes = app.InterviewNotes,
                ScreeningNotes = app.ScreeningNotes,
                
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