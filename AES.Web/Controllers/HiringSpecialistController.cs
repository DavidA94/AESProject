using AES.Shared;
using AES.Shared.Contracts;
using AES.Web.ApplicationService;
using AES.Web.Authorization;
using AES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AES.Web.Controllers
{
    //[AESAuthorize(BadRedirectURL = "/EmployeeLogin", Role = EmployeeRole.HqHiringSpecialist)]
    public class HiringSpecialistController : Controller
    {
        HiringSpecialistModel hs = new HiringSpecialistModel();

        // GET: HiringSpecialist
        [HttpGet]
        public ActionResult DashboardHS()
        {
            //var x = FillHSData();

            IApplicationSvc appSvc = new ApplicationSvcClient();
            ApplicantInfoContract[] CallingApplicants = appSvc.GetApplicantsAwaitingCalls(DateTime.Now);

            List<HiringSpecialistModel> ConvertedContract = ConvertContractToModel(CallingApplicants);

            return View(ConvertedContract);

            //return View(x);
        }

        [HttpPost]
        public ActionResult DashboardHS(string ApplicantStatus, int ApplicantID, string Notes)
        {
            IApplicationSvc appSvc = new ApplicationSvcClient();

            if (ApplicantStatus == "Accept")
            {
                appSvc.SavePhoneInterview(ApplicantID, Notes, true);
            }
            else if (ApplicantStatus == "Deny")
            {
                appSvc.SavePhoneInterview(ApplicantID, Notes, false);
            }
            else
            {
                appSvc.ApplicantDidNotAnswer(ApplicantID);
            }

            ApplicantInfoContract[] CallingApplicants = appSvc.GetApplicantsAwaitingCalls(DateTime.Now);

            List<HiringSpecialistModel> ConvertedContract = ConvertContractToModel(CallingApplicants);

            return View(ConvertedContract);
        }

        private List<HiringSpecialistModel> ConvertContractToModel(IEnumerable<ApplicantInfoContract> ApplicantInfo)
        {
            var RetAppInfo = new List<HiringSpecialistModel>();

            foreach (var AppInfo in ApplicantInfo)
            {
                RetAppInfo.Add(new HiringSpecialistModel()
                {
                    FirstName = AppInfo.FirstName,
                    LastName = AppInfo.LastName,
                    ETA = AppInfo.UserInfo.EndCallTime,
                    ApplicantID = (int)AppInfo.UserID
                });
            }

            return RetAppInfo;
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
                    ApplicantID = 1
                },

                new HiringSpecialistModel()
                {
                    ETA = new TimeSpan(11,11,11),
                    FirstName = "Awesome",
                    LastName = "Fun",
                    ApplicantID = 2
                },

                new HiringSpecialistModel()
                {
                    ETA = new TimeSpan(11,11,11),
                    FirstName = "Awesome",
                    LastName = "Fun",
                    ApplicantID = 3
                },

                new HiringSpecialistModel()
                {
                    ETA = new TimeSpan(11,11,11),
                    FirstName = "Awesome",
                    LastName = "Fun",
                    ApplicantID = 4
                }
            };
        }

        [HttpPost]
        public ActionResult CallPage(int ApplicantID)
        {

            IApplicationSvc appSvc = new ApplicationSvcClient();

            if (!appSvc.CallApplicant(ApplicantID))
            {
                return RedirectToAction("DashboardHS");
            }

            appSvc.CallApplicant(ApplicantID);

            ApplicationInfoContract App = appSvc.GetApplication(ApplicantID, Shared.AppStatus.IN_CALL);

            FullApplicationModel ConvertedFullAppModel = ConvertAppContractToModel(App);

            //Keep track of the applicant ID for apply or deny buttons
            ConvertedFullAppModel.ApplicantID = ApplicantID;

            return View(ConvertedFullAppModel);
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
    }
}