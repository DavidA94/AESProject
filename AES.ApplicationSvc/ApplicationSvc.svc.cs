using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using AES.ApplicationSvc.Contracts;
using AES.Shared;
using AES.Shared.Contracts;
using AES.Entities.Tables;
using AES.Entities.Contexts;

namespace AES.ApplicationSvc
{
    public class ApplicationSvc : IApplicationSvc
    {
        public bool CancelApplication(ApplicationInfoContract app)
        {
            throw new NotImplementedException();
        }

        public UserInfoContract GetApplicantsAwaitingCalls()
        {
            throw new NotImplementedException();
        }

        public UserInfoContract GetApplicantsAwaitingInterview(int storeID)
        {
            throw new NotImplementedException();
        }

        public ApplicationInfoContract GetApplication(UserInfoContract user)
        {
            throw new NotImplementedException();
        }

        public ApplicationInfoContract GetCallApplication(UserInfoContract user)
        {
            throw new NotImplementedException();
        }

        public ApplicationInfoContract GetInterviewApplication(UserInfoContract user)
        {
            throw new NotImplementedException();
        }

        public bool PullApplicantFromCallQueue(UserInfoContract user)
        {
            throw new NotImplementedException();
        }

        public AppSvcResponse SavePartialApplication(ApplicationInfoContract app)
        {
            if(app.Applicant.UserID == null)
            {
                return AppSvcResponse.BAD_USER;
            }

            using (var db = new ApplicationDbContext())
            {
                foreach (var job in app.AppliedJobs)
                {
                    var appliedJob = db.Applications.FirstOrDefault(a => a.Job.ID == job);
                    if (appliedJob == null)
                    {
                        return AppSvcResponse.BAD_JOB;
                    }

                    var application = new Application();
                    application.Job = new Job() { ID = job };
                    application.ApplicantID = (int)app.Applicant.UserID;
                    application.Status = AppStatus.PARTIAL;
                    application.Timestamp = DateTime.Now;

                    foreach(var q in app.Questions)
                    {
                        var dbQ = db.Questions.FirstOrDefault(question => question.ID == q.QuestionID);
                        if(dbQ == null)
                        {
                            return AppSvcResponse.BAD_QUESTION;
                        }
                        else if(dbQ.Job.ID != job)
                        {
                            continue;
                        }

                        switch (q.Type)
                        {
                            case QuestionType.CHECKBOX:
                            case QuestionType.RADIO:
                                application.MultiAnswers.Add(new ApplicationMultiAnswer()
                                {
                                    Answer1 = app.Answers.FirstOrDefault(a => a.QuestionId == dbQ.ID).MC_Answer1,
                                    Answer2 = app.Answers.FirstOrDefault(a => a.QuestionId == dbQ.ID).MC_Answer2,
                                    Answer3 = app.Answers.FirstOrDefault(a => a.QuestionId == dbQ.ID).MC_Answer3,
                                    Answer4 = app.Answers.FirstOrDefault(a => a.QuestionId == dbQ.ID).MC_Answer4,
                                    Question = dbQ,
                                });
                                break;
                            case QuestionType.SHORT:
                                application.ShortAnswers.Add(new ApplicationShortAnswer()
                                {
                                    Answer = app.Answers.FirstOrDefault(a => a.QuestionId == dbQ.ID).ShortAnswer,
                                    Question = dbQ
                                });
                                break;
                        }
                    }
                    db.Applications.Add(application);
                }

                if(db.SaveChanges() == app.AppliedJobs.Count)
                {
                    return AppSvcResponse.GOOD;
                }
            }

            return AppSvcResponse.ERROR;
        }

        public bool SetApplicationStatus(ApplicationInfoContract app, AppStatus status)
        {
            throw new NotImplementedException();
        }

        public bool SubmitApplication(ApplicationInfoContract app)
        {
            throw new NotImplementedException();
        }

        #region Converters

        #region Availability

        private Availability ConvertContractToTable(AvailabilityContract availability)
        {
            // We only care about the time. Using this method where we just get the time will make the date irrelvant
            return new Availability()
            {
                SundayStart = availability.SundayStart,
                SundayEnd = availability.SundayEnd,
                MondayStart = availability.MondayStart,
                MondayEnd = availability.MondayEnd,
                TuesdayStart = availability.TuesdayStart,
                TuesdayEnd = availability.TuesdayEnd,
                WednesdayStart = availability.WednesdayStart,
                WednesdayEnd = availability.WednesdayEnd,
                ThursdayStart = availability.ThursdayStart,
                ThursdayEnd = availability.ThursdayEnd,
                FridayStart = availability.FridayStart,
                FridayEnd = availability.FridayEnd,
                SaturdayStart = availability.SaturdayStart,
                SaturdayEnd = availability.SaturdayEnd
            };
        }

        private AvailabilityContract ConvertTableToContract(Availability availability)
        {
            return new AvailabilityContract()
            {
                SundayStart = availability.SundayStart,
                SundayEnd = availability.SundayEnd,
                MondayStart = availability.MondayStart,
                MondayEnd = availability.MondayEnd,
                TuesdayStart = availability.TuesdayStart,
                TuesdayEnd = availability.TuesdayEnd,
                WednesdayStart = availability.WednesdayStart,
                WednesdayEnd = availability.WednesdayEnd,
                ThursdayStart = availability.ThursdayStart,
                ThursdayEnd = availability.ThursdayEnd,
                FridayStart = availability.FridayStart,
                FridayEnd = availability.FridayEnd,
                SaturdayStart = availability.SaturdayStart,
                SaturdayEnd = availability.SaturdayEnd
            };
        }

        #endregion

        #region ApplicantUser

        private ApplicantUser ConvertContractToTable(ApplicantInfoContract applicant)
        {
            return new ApplicantUser()
            {
                Availability = ConvertContractToTable(applicant.Availability),
                CallEndTime = applicant.EndCallTime,
                CallStartTime = applicant.StartCallTime,
                DOB = applicant.DOB,
                EducationHistory = ConvertContractToTable(applicant.Education),
                EmploymentHistory = ConvertContractToTable(applicant.PastJobs),
                FirstName = applicant.FirstName,
                LastName = applicant.LastName,
                References = ConvertContractToTable(applicant.References),
                userID = applicant.UserID ?? -1,
                UserInfo = ConvertContractToTable(applicant.UserInfo),
                
            };
        }

        private ApplicantInfoContract ConvertTableToContract(ApplicantUser applicant)
        {
            return new ApplicantInfoContract()
            {
                Availability = ConvertTableToContract(applicant.Availability),
                EndCallTime = applicant.CallEndTime,
                StartCallTime = applicant.CallStartTime,
                DOB = applicant.DOB,
                Education = ConvertTableToContract(applicant.EducationHistory).ToList(),
                PastJobs = ConvertTableToContract(applicant.EmploymentHistory).ToList(),
                FirstName = applicant.FirstName,
                LastName = applicant.LastName,
                References = ConvertTableToContract(applicant.References).ToList(),
                UserID = applicant.userID,
                UserInfo = ConvertTableToContract(applicant.UserInfo),
            };
        }

        #endregion

        #region Education

        private ICollection<EducationHistory> ConvertContractToTable(ICollection<EducationHistoryContract> eHist)
        {
            var schools = new List<EducationHistory>();
            foreach(var school in eHist)
            {
                schools.Add(new EducationHistory()
                {
                    Degree = school.Degree,
                    GraduationDate = school.Graduated,
                    Major = school.Major,
                    SchoolAddress = school.SchoolAddress,
                    SchoolCity = school.SchoolCity,
                    SchoolCountry = school.SchoolCountry,
                    SchoolName = school.SchoolName,
                    SchoolState = school.SchoolState,
                    SchoolZip = school.SchoolZIP,
                    YearsAttended = school.YearsAttended
                });
            }

            return schools;
        }

        private ICollection<EducationHistoryContract> ConvertTableToContract(ICollection<EducationHistory> eHist)
        {
            var schools = new List<EducationHistoryContract>();
            foreach (var school in eHist)
            {
                schools.Add(new EducationHistoryContract()
                {
                    Degree = school.Degree,
                    Graduated = school.GraduationDate,
                    Major = school.Major,
                    SchoolAddress = school.SchoolAddress,
                    SchoolCity = school.SchoolCity,
                    SchoolCountry = school.SchoolCountry,
                    SchoolName = school.SchoolName,
                    SchoolState = school.SchoolState,
                    SchoolZIP = school.SchoolZip,
                    YearsAttended = school.YearsAttended
                });
            }

            return schools;
        }

        #endregion

        #region Employment

        private ICollection<JobHistory> ConvertContractToTable(ICollection<JobHistoryContract> jobs)
        {
            var retJobs = new List<JobHistory>();

            foreach(var job in jobs)
            {
                retJobs.Add(new JobHistory()
                {
                    EmployerAddress = job.EmployerAddress,
                    EmployerCity = job.EmployerCity,
                    EmployerCountry = job.EmployerCountry,
                    EmployerName = job.EmployerName,
                    EmployerPhone = job.EmployerPhone,
                    EmployerState = job.EmployerState,
                    EmployerZip = job.EmployerZip,
                    EndDate = job.EndDate,
                    EndingSalary = job.EndingSalary,
                    ReasonForLeaving = job.ReasonForLeaving,
                    Responsibilities = job.Responsibilities,
                    StartDate = job.StartDate,
                    StartingSalary = job.StartingSalary,
                    SupervisorName = job.SupervisorName
                });
            }

            return retJobs;
        }

        private ICollection<JobHistoryContract> ConvertTableToContract(ICollection<JobHistory> jobs)
        {
            var retJobs = new List<JobHistoryContract>();

            foreach (var job in jobs)
            {
                retJobs.Add(new JobHistoryContract()
                {
                    EmployerAddress = job.EmployerAddress,
                    EmployerCity = job.EmployerCity,
                    EmployerCountry = job.EmployerCountry,
                    EmployerName = job.EmployerName,
                    EmployerPhone = job.EmployerPhone,
                    EmployerState = job.EmployerState,
                    EmployerZip = job.EmployerZip,
                    EndDate = job.EndDate,
                    EndingSalary = job.EndingSalary,
                    ReasonForLeaving = job.ReasonForLeaving,
                    Responsibilities = job.Responsibilities,
                    StartDate = job.StartDate,
                    StartingSalary = job.StartingSalary,
                    SupervisorName = job.SupervisorName
                });
            }

            return retJobs;
        }

        #endregion

        #region References

        private ICollection<Reference> ConvertContractToTable(ICollection<ReferenceContract> refs)
        {
            var references = new List<Reference>();
            foreach(var r in refs)
            {
                references.Add(new Reference()
                {
                    Company = r.Company,
                    Name = r.Name,
                    Phone = r.Phone,
                    Title = r.Title
                });
            }

            return references;
        }

        private ICollection<ReferenceContract> ConvertTableToContract(ICollection<Reference> refs)
        {
            var references = new List<ReferenceContract>();
            foreach (var r in refs)
            {
                references.Add(new ReferenceContract()
                {
                    Company = r.Company,
                    Name = r.Name,
                    Phone = r.Phone,
                    Title = r.Title
                });
            }

            return references;
        }

        #endregion

        #region UserInfo

        private UserInfo ConvertContractToTable(UserInfoContract info)
        {
            return new UserInfo()
            {
                Address = info.Address,
                City = info.City,
                Nickname = info.Nickname,
                Phone = info.Phone,
                SalaryExpectation = info.SalaryExpectation,
                State = info.State,
                Zip = info.Zip
            };
        }

        private UserInfoContract ConvertTableToContract(UserInfo info)
        {
            return new UserInfoContract()
            {
                Address = info.Address,
                City = info.City,
                Nickname = info.Nickname,
                Phone = info.Phone,
                SalaryExpectation = info.SalaryExpectation,
                State = info.State,
                Zip = info.Zip
            };
        }

        #endregion

        #endregion
    }
}
