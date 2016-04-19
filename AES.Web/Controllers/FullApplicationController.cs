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
    public class FullApplicationController : Controller
    {
        // GET: FullApplication
        public ActionResult Index()
        {
            return View();
        }

        public FullApplicationModel ConvertAppContractToModel(ApplicationInfoContract app)
        {
            return new FullApplicationModel
            {
                Profile = new ProfileViewModel()
                {
                    Address = app.UserInfo.Address,
                    City = app.UserInfo.City,
                    //DOB = app.Applicant.DOB,
                    EndCallTime = app.UserInfo.EndCallTime,
                    //FirstName = app.Applicant.FirstName,
                    //LastName = app.Applicant.LastName,
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

                //WorkHistory = ConvertAppContractToModel(app.Jobs),

                //Education = ConvertAppContractToModel(app.Educations),

                //References = ConvertAppContractToModel(app.References),

                //Questionnaire = ConvertAppContractToModel(app.QA)

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
                    //MC_Answers = ques.MC_Answers,
                    //Options = ques.Options,
                    Question = ques.Question,
                    QuestionID = ques.QuestionID,
                    //RadioOption = ques.Type,
                    ShortAnswer = ques.ShortAnswer,
                    Type = ques.Type
                });
            }

            return RetQA;
        }
    }
}