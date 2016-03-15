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

        public ApplicationInfoContract GetApplication(ApplicantInfoContract user)
        {
            if (user.UserID == null)
            {
                return null;
            }

            var retApp = new ApplicationInfoContract()
            {
                ApplicantID = (int)user.UserID
            };

            using (var db = new AESDbContext())
            {
                var applications = db.Applications.Where(a => a.Applicant.userID == user.UserID &&
                                                              a.Status == AppStatus.PARTIAL);

                foreach(var app in applications)
                {
                    retApp.AppliedJobs.Add(app.Job.ID);

                    retApp.Availability = ConvertTableToContract(app.Applicant.Availability);
                    retApp.Educations = ConvertTableToContract(app.Applicant.EducationHistory).ToList();
                    retApp.Jobs = ConvertTableToContract(app.Applicant.EmploymentHistory).ToList();
                    retApp.References = ConvertTableToContract(app.Applicant.References).ToList();
                    retApp.UserInfo = ConvertTableToContract(app.Applicant.UserInfo);

                    foreach(var a in app.MultiAnswers)
                    {
                        // Don't add the same Q&A twice
                        if(retApp.QA.FirstOrDefault(qa => qa.QuestionID == a.Question.ID) != null)
                        {
                            continue;
                        }

                        retApp.QA.Add(new QAContract()
                        {
                            MC_Answers = new List<bool> { a.Answer1, a.Answer2, a.Answer3, a.Answer4 },
                            Options = new List<string> { a.Question.Option1, a.Question.Option2,
                                                         a.Question.Option3, a.Question.Option4 },
                            Question = a.Question.Text,
                            QuestionID = a.Question.ID,
                            Type = a.Question.Type
                        });
                    }

                    foreach(var a in app.ShortAnswers)
                    {
                        // Don't add the same Q&A twice
                        if (retApp.QA.FirstOrDefault(qa => qa.QuestionID == a.Question.ID) != null)
                        {
                            continue;
                        }

                        retApp.QA.Add(new QAContract()
                        {
                            Question = a.Question.Text,
                            QuestionID = a.Question.ID,
                            ShortAnswer = a.Answer,
                            Type = a.Question.Type,
                        });
                    }
                }
            }

            return retApp;
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
            using (var db = new AESDbContext())
            {
                // Get the user for this application
                var user = db.ApplicantUsers.FirstOrDefault(u => u.userID == app.ApplicantID);

                // If the user is not found, return BAD_USER
                if (user == null)
                {
                    return AppSvcResponse.BAD_USER;
                }

                // Loop through all the jobs the user is applying for
                foreach (var job in app.AppliedJobs)
                {
                    // Get the job
                    var appliedJob = db.Jobs.FirstOrDefault(a => a.ID == job);

                    // If it doens't exist, return BAD_JOB
                    if (appliedJob == null)
                    {
                        return AppSvcResponse.BAD_JOB;
                    }

                    // Try to get a partial application for this user for this job
                    var application = db.Applications.FirstOrDefault(a => a.Status == AppStatus.PARTIAL &&
                    /**/                                                  a.Job.ID == job &&
                    /**/                                                  a.Applicant.userID == user.userID);
                    bool isNewApp = application == null;

                    // If we didn't get one, then make a new one
                    if (application == null)
                    {
                        application = new Application();

                        // Assign it's Job and Status
                        application.Job = appliedJob;
                        application.Status = AppStatus.PARTIAL;
                        application.Applicant = user;
                    }

                    // Update the TimeStamp
                    application.Timestamp = DateTime.Now;

                    #region Answers

                    // Loop through all the questions we've gotten
                    foreach (var q in app.QA)
                    {
                        // Get the question from the DB
                        var dbQ = db.Questions.FirstOrDefault(question => question.ID == q.QuestionID);

                        // If we didn't get a question back, return BAD_QUESTION
                        if (dbQ == null)
                        {
                            return AppSvcResponse.BAD_QUESTION;
                        }
                        // If the ID of the job for this question is not the job we're on, then skip it
                        else if (dbQ.Jobs.FirstOrDefault(j => j.ID == job) == null)
                        {
                            continue;
                        }

                        // Enter the QA Data based on the question type
                        switch (q.Type)
                        {
                            case QuestionType.CHECKBOX:
                            case QuestionType.RADIO:
                                var answer = application.MultiAnswers.FirstOrDefault(a => a.Question == dbQ);

                                // See if this answer has already been added
                                if (answer != null)
                                {
                                    answer.Answer1 = app.QA.FirstOrDefault(a => a.QuestionID == dbQ.ID).MC_Answers.ElementAtOrDefault(0);
                                    answer.Answer2 = app.QA.FirstOrDefault(a => a.QuestionID == dbQ.ID).MC_Answers.ElementAtOrDefault(1);
                                    answer.Answer3 = app.QA.FirstOrDefault(a => a.QuestionID == dbQ.ID).MC_Answers.ElementAtOrDefault(2);
                                    answer.Answer4 = app.QA.FirstOrDefault(a => a.QuestionID == dbQ.ID).MC_Answers.ElementAtOrDefault(3);
                                }
                                // Otherwise, add it
                                else {
                                    application.MultiAnswers.Add(new ApplicationMultiAnswer()
                                    {
                                        Answer1 = app.QA.FirstOrDefault(a => a.QuestionID == dbQ.ID).MC_Answers.ElementAtOrDefault(0),
                                        Answer2 = app.QA.FirstOrDefault(a => a.QuestionID == dbQ.ID).MC_Answers.ElementAtOrDefault(1),
                                        Answer3 = app.QA.FirstOrDefault(a => a.QuestionID == dbQ.ID).MC_Answers.ElementAtOrDefault(2),
                                        Answer4 = app.QA.FirstOrDefault(a => a.QuestionID == dbQ.ID).MC_Answers.ElementAtOrDefault(3),
                                        Question = dbQ
                                    });
                                }
                                break;
                            case QuestionType.SHORT:
                                application.ShortAnswers.Add(new ApplicationShortAnswer()
                                {
                                    Answer = app.QA.FirstOrDefault(a => a.QuestionID == dbQ.ID).ShortAnswer,
                                    Question = dbQ
                                });
                                break;
                        }
                    }

                    #endregion

                    #region Availability

                    // Just reset the entire row
                    user.Availability = ConvertContractToTable(app.Availability);

                    #endregion

                    #region Education

                    /// This is a bit dirty, and heavy on the DB, but since everything is very light as it is, that won't matter.

                    // Clear out any old education information
                    db.EducationHistories.RemoveRange(user.EducationHistory);

                    // Loop through our passed in education information, and add each one to the user's list
                    foreach (var e in ConvertContractToTable(app.Educations))
                    {
                        e.Applicant = user;
                        user.EducationHistory.Add(e);

                    }

                    #endregion

                    #region Employment

                    /// Ditto from Education

                    db.JobHistories.RemoveRange(user.EmploymentHistory);

                    foreach (var e in ConvertContractToTable(app.Jobs))
                    {
                        e.Applicant = user;
                        user.EmploymentHistory.Add(e);
                    }

                    #endregion

                    #region References

                    /// Ditto from Education

                    db.References.RemoveRange(user.References);

                    foreach (var r in ConvertContractToTable(app.References))
                    {
                        r.Applicant = user;
                        user.References.Add(r);
                    }

                    #endregion

                    #region User Info

                    user.UserInfo.Address = app.UserInfo.Address;
                    user.UserInfo.CallEndTime = app.UserInfo.EndCallTime;
                    user.UserInfo.CallStartTime = app.UserInfo.StartCallTime;
                    user.UserInfo.City = app.UserInfo.City;
                    user.UserInfo.Nickname = app.UserInfo.Nickname;
                    user.UserInfo.Phone = app.UserInfo.Phone;
                    user.UserInfo.SalaryExpectation = app.UserInfo.SalaryExpectation;
                    user.UserInfo.State = app.UserInfo.State;
                    user.UserInfo.Zip = app.UserInfo.Zip;

                    #endregion

                    // If this is a new application, then we need to add it
                    if (isNewApp)
                    {
                        db.Applications.Add(application);
                    }
                }

                if (db.SaveChanges() != 0)
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
            foreach (var school in eHist)
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

            foreach (var job in jobs)
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
            foreach (var r in refs)
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
                CallEndTime = info.EndCallTime,
                CallStartTime = info.StartCallTime,
                City = info.City,
                Nickname = info.Nickname,
                Phone = info.Phone,
                SalaryExpectation = info.SalaryExpectation,
                State = info.State,
                Zip = info.Zip,
            };
        }

        private UserInfoContract ConvertTableToContract(UserInfo info)
        {
            return new UserInfoContract()
            {
                Address = info.Address,
                City = info.City,
                EndCallTime = info.CallEndTime,
                Nickname = info.Nickname,
                Phone = info.Phone,
                SalaryExpectation = info.SalaryExpectation,
                StartCallTime = info.CallStartTime,
                State = info.State,
                Zip = info.Zip
            };
        }

        #endregion

        #endregion
    }
}
