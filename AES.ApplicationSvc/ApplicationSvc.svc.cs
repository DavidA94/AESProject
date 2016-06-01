﻿using AES.ApplicationSvc.Contracts;
using AES.Entities.Contexts;
using AES.Entities.Tables;
using AES.Shared;
using AES.Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;

namespace AES.ApplicationSvc
{
    public class ApplicationSvc : IApplicationSvc
    {
        public ApplicationSvc()
        {
            DBFileManager.SetDataDirectory();
        }

        public bool ApplicantDidNotAnswer(int applicantID)
        {
            return SetApplicationStatus(applicantID, AppStatus.IN_CALL, AppStatus.WAITING_CALL);
        }

        public bool CallApplicant(int applicantID)
        {
            return SetApplicationStatus(applicantID, AppStatus.WAITING_CALL, AppStatus.IN_CALL);
        }

        public bool CancelApplication(ApplicationInfoContract app)
        {
            if(app == null)
            {
                return false;
            }

            using (var db = new AESDbContext())
            {
                // Get the application, and ensure we got one
                var applications = db.Applications.Where(a => a.Status == AppStatus.PARTIAL && a.ApplicantID == app.ApplicantID);
                if(!applications.Any())
                {
                    return false;
                }

                // Get the user, and ensure there is one
                var user = applications.FirstOrDefault().Applicant;
                if(user == null)
                {
                    return false;
                }

                // Ensure the data given matches what is in the DB, assuming there is information in the database.
                if ((user.FirstName != app.FirstName && !string.IsNullOrWhiteSpace(user.FirstName)) ||
                    (user.LastName != app.LastName && !string.IsNullOrWhiteSpace(user.LastName)) ||
                    (user.DOB != app.DOB && user.DOB != new DateTime(1970, 1, 1)))
                {
                    return false;
                }

                // Disassociate the user and job from the application, and remove any answers
                foreach (var application in applications)
                {
                    application.Applicant = null;
                    application.ApplicantID = -1;
                    application.Job = null;
                    application.JobID = -1;

                    db.MultiAnswers.RemoveRange(application.MultiAnswers);
                    db.ShortAnswers.RemoveRange(application.ShortAnswers);
                }

                // Remove the applications
                db.Applications.RemoveRange(applications);

                // As long as the changes are saved
                try {
                    return db.SaveChanges() > 0;
                }
                catch
                {
                    return false;
                }
            }
        }

        public List<ApplicantInfoContract> GetApplicantsAwaitingCalls(DateTime currentDateTime)
        {
            TimeSpan currentTime = new TimeSpan(currentDateTime.Hour, currentDateTime.Minute, currentDateTime.Second);
            List<ApplicantInfoContract> returnedApplicants = new List<ApplicantInfoContract>();

            using (var db = new AESDbContext())
            {
                var apps = db.Applications.Where(aa => aa.Status == AppStatus.WAITING_CALL);

                foreach (var au in apps.Select(a => a.Applicant))
                {
                    var callStart = au.UserInfo.CallStartTime;
                    var callEnd = au.UserInfo.CallEndTime;

                    if ((currentTime > callStart) && (currentTime < callEnd))
                    {
                        returnedApplicants.Add(ConvertTableToContract(au));
                    }
                }
            }

            return returnedApplicants.Distinct().ToList();
        }

        public List<ApplicantInfoContract> GetApplicantsAwaiting(int storeID, AppStatus status)
        {
            List<ApplicantInfoContract> returnedApplicants = new List<ApplicantInfoContract>();

            using (var db = new AESDbContext())
            {
                var apps = db.Applications.Where(a => a.Status == status && a.StoreID == storeID);

                foreach (var app in apps.Select(a => a.Applicant))
                {
                    returnedApplicants.Add(ConvertTableToContract(app));
                }
            }

            return returnedApplicants.Distinct().ToList();
        }

        public ApplicationInfoContract GetApplication(int userID, AppStatus userAppStatus)
        {
            var retApp = new ApplicationInfoContract()
            {
                ApplicantID = userID
            };

            using (var db = new AESDbContext())
            {
                var applications = db.Applications.Where(a => a.Applicant.userID == userID &&
                                                              a.Status == userAppStatus);
                var submittedApp = db.Applications.Where(a => a.Applicant.userID == userID &&
                                                              a.Status != userAppStatus)
                                                              .OrderByDescending(a => a.Timestamp).FirstOrDefault();
                if (applications != null)
                {
                    foreach (var app in applications)
                    {
                        retApp.AppliedJobs.Add(Tuple.Create(app.Job.JobID, app.StoreID));
                        var job = db.Jobs.Where(j => j.JobID == app.Job.JobID).FirstOrDefault();

                        if (job == null)
                        {
                            continue;
                        }

                        retApp.FirstName = app.Applicant.FirstName;
                        retApp.LastName = app.Applicant.LastName;
                        retApp.DOB = app.Applicant.DOB;

                        retApp.Availability = ConvertTableToContract(app.Applicant.Availability);
                        retApp.Educations = ConvertTableToContract(app.Applicant.EducationHistory).ToList();
                        retApp.Jobs = ConvertTableToContract(app.Applicant.EmploymentHistory).ToList();
                        retApp.References = ConvertTableToContract(app.Applicant.References).ToList();
                        retApp.UserInfo = ConvertTableToContract(app.Applicant.UserInfo);

                        foreach (var q in job.Questions)
                        {
                            // Don't add the same Q&A twice
                            if (retApp.QA.FirstOrDefault(qa => qa.QuestionID == q.QuestionID) != null)
                            {
                                continue;
                            }

                            if (q.Type == QuestionType.CHECKBOX || q.Type == QuestionType.RADIO)
                            {
                                var options = new List<string>();
                                if (q.Option1 != null && q.Option1 != "") options.Add(q.Option1);
                                if (q.Option2 != null && q.Option2 != "") options.Add(q.Option2);
                                if (q.Option3 != null && q.Option3 != "") options.Add(q.Option3);
                                if (q.Option4 != null && q.Option4 != "") options.Add(q.Option4);


                                var multiAnswer = app.MultiAnswers.FirstOrDefault(qa => qa.Question.QuestionID == q.QuestionID);
                                var multiAnswers = multiAnswer == null ? new List<bool> { false, false, false, false } : new List<bool>
                                {
                                    multiAnswer.Answer1,
                                    multiAnswer.Answer2,
                                    multiAnswer.Answer3,
                                    multiAnswer.Answer4
                                };

                                retApp.QA.Add(new QAContract()
                                {
                                    Question = q.Text,
                                    QuestionID = q.QuestionID,
                                    Options = options,
                                    Type = q.Type,

                                    MC_Answers = multiAnswers
                                });
                            }
                            else
                            {
                                var userAnswer = app.ShortAnswers.FirstOrDefault(qa => qa.Question.QuestionID == q.QuestionID);

                                retApp.QA.Add(new QAContract()
                                {
                                    Question = q.Text,
                                    QuestionID = q.QuestionID,
                                    Type = q.Type,

                                    ShortAnswer = userAnswer == null ? "" : userAnswer.Answer
                                });
                            }
                        }
                    }
                }
                else if (submittedApp != null)
                {
                    retApp.FirstName = submittedApp.Applicant.FirstName;
                    retApp.LastName = submittedApp.Applicant.LastName;
                    retApp.DOB = submittedApp.Applicant.DOB;

                    retApp.Availability = ConvertTableToContract(submittedApp.Applicant.Availability);
                    retApp.Educations = ConvertTableToContract(submittedApp.Applicant.EducationHistory).ToList();
                    retApp.Jobs = ConvertTableToContract(submittedApp.Applicant.EmploymentHistory).ToList();
                    retApp.References = ConvertTableToContract(submittedApp.Applicant.References).ToList();
                    retApp.UserInfo = ConvertTableToContract(submittedApp.Applicant.UserInfo);
                }
            }

            return retApp;
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
                    var appliedJob = db.Jobs.FirstOrDefault(a => a.JobID == job.Item1);

                    // If it doens't exist, return BAD_JOB
                    if (appliedJob == null)
                    {
                        return AppSvcResponse.BAD_JOB;
                    }

                    // Get the store, and return BAD_STORE if it doesn't exist.
                    var appliedStore = db.Stores.FirstOrDefault(s => s.ID == job.Item2);
                    if(appliedStore == null)
                    {
                        return AppSvcResponse.BAD_STORE;
                    }

                    // Try to get a partial application for this user for this job
                    var application = db.Applications.FirstOrDefault(a => a.Status == AppStatus.PARTIAL &&
                    /**/                                                  a.Job.JobID == job.Item1 &&
                    /**/                                                  a.Store.ID == job.Item2 &&
                    /**/                                                  a.Applicant.userID == user.userID);
                    bool isNewApp = application == null;

                    // If we didn't get one, then make a new one
                    if (application == null)
                    {
                        application = new Application();

                        // Assign its Job and Status
                        application.Job = appliedJob;
                        application.Status = AppStatus.PARTIAL;
                        application.Applicant = user;
                        application.StoreID = appliedStore.ID;
                    }

                    // Update the TimeStamp
                    application.Timestamp = DateTime.Now;

                    #region Answers

                    // Loop through all the questions we've gotten
                    foreach (var q in app.QA ?? new List<QAContract>())
                    {
                        // Get the question from the DB
                        var dbQ = db.Questions.FirstOrDefault(question => question.QuestionID == q.QuestionID);

                        // If we didn't get a question back, return BAD_QUESTION
                        if (dbQ == null)
                        {
                            return AppSvcResponse.BAD_QUESTION;
                        }
                        // If the ID of the job for this question is not the job we're on, then skip it
                        else if (dbQ.Jobs.FirstOrDefault(j => j.JobID == job.Item1) == null)
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
                                    answer.Answer1 = app.QA.FirstOrDefault(a => a.QuestionID == dbQ.QuestionID).MC_Answers.ElementAtOrDefault(0);
                                    answer.Answer2 = app.QA.FirstOrDefault(a => a.QuestionID == dbQ.QuestionID).MC_Answers.ElementAtOrDefault(1);
                                    answer.Answer3 = app.QA.FirstOrDefault(a => a.QuestionID == dbQ.QuestionID).MC_Answers.ElementAtOrDefault(2);
                                    answer.Answer4 = app.QA.FirstOrDefault(a => a.QuestionID == dbQ.QuestionID).MC_Answers.ElementAtOrDefault(3);
                                }
                                // Otherwise, add it
                                else {
                                    application.MultiAnswers.Add(new ApplicationMultiAnswer()
                                    {
                                        Answer1 = app.QA.FirstOrDefault(a => a.QuestionID == dbQ.QuestionID).MC_Answers.ElementAtOrDefault(0),
                                        Answer2 = app.QA.FirstOrDefault(a => a.QuestionID == dbQ.QuestionID).MC_Answers.ElementAtOrDefault(1),
                                        Answer3 = app.QA.FirstOrDefault(a => a.QuestionID == dbQ.QuestionID).MC_Answers.ElementAtOrDefault(2),
                                        Answer4 = app.QA.FirstOrDefault(a => a.QuestionID == dbQ.QuestionID).MC_Answers.ElementAtOrDefault(3),
                                        Question = dbQ,
                                    });
                                }
                                break;
                            case QuestionType.SHORT:
                                // Get the last answer
                                var sAnswer = application.ShortAnswers.FirstOrDefault(a => a.Question == dbQ);

                                // If it already had an answer, update it
                                if (sAnswer != null)
                                {
                                    sAnswer.Answer = app.QA.FirstOrDefault(a => a.QuestionID == dbQ.QuestionID).ShortAnswer;
                                }
                                // Otherwise, add it
                                else {
                                    application.ShortAnswers.Add(new ApplicationShortAnswer()
                                    {
                                        Answer = app.QA.FirstOrDefault(a => a.QuestionID == dbQ.QuestionID).ShortAnswer,
                                        Question = dbQ
                                    });
                                }
                                break;
                        }
                    }

                    #endregion

                    #region Availability

                    // Just reset the entire row
                    user.Availability = ConvertContractToTable(app.Availability ?? new AvailabilityContract());

                    #endregion

                    #region Education

                    /// This is a bit dirty, and heavy on the DB, but since everything is very light as it is, that won't matter.

                    // Clear out any old education information
                    db.EducationHistories.RemoveRange(user.EducationHistory);

                    // Loop through our passed in education information, and add each one to the user's list
                    foreach (var e in ConvertContractToTable(app.Educations ?? new List<EducationHistoryContract>()))
                    {
                        e.Applicant = user;
                        user.EducationHistory.Add(e);

                    }

                    #endregion

                    #region Employment

                    /// Ditto from Education

                    db.JobHistories.RemoveRange(user.EmploymentHistory);

                    foreach (var e in ConvertContractToTable(app.Jobs ?? new List<JobHistoryContract>()))
                    {
                        e.Applicant = user;
                        user.EmploymentHistory.Add(e);
                    }

                    #endregion

                    #region References

                    /// Ditto from Education

                    db.References.RemoveRange(user.References);

                    foreach (var r in ConvertContractToTable(app.References ?? new List<ReferenceContract>()))
                    {
                        r.Applicant = user;
                        user.References.Add(r);
                    }

                    #endregion

                    #region User Info

                    if (app.UserInfo != null)
                    {
                        user.UserInfo.Address = app.UserInfo.Address;
                        user.UserInfo.CallEndTime = app.UserInfo.EndCallTime;
                        user.UserInfo.CallStartTime = app.UserInfo.StartCallTime;
                        user.UserInfo.City = app.UserInfo.City;
                        user.UserInfo.Nickname = app.UserInfo.Nickname;
                        user.UserInfo.Phone = app.UserInfo.Phone;
                        user.UserInfo.SalaryExpectation = app.UserInfo.SalaryExpectation;
                        user.UserInfo.State = app.UserInfo.State;
                        user.UserInfo.Zip = app.UserInfo.Zip;
                    }

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

        public bool SavePhoneInterview(int applicantID, string notes, bool approved)
        {
            AppStatus setStatus = approved ? AppStatus.WAITING_INTERVIEW : AppStatus.CALL_DENIED;

            if (!SetApplicationStatus(applicantID, AppStatus.IN_CALL, setStatus))
            {
                return false;
            }
            using (var db = new AESDbContext())
            {
                var apps = db.Applications.Where(a => a.Applicant.userID == applicantID && a.Status == setStatus);

                if (!apps.Any())
                {
                    return false;
                }

                foreach (var app in apps)
                {
                    app.ScreeningNotes = notes;
                }

                if (db.SaveChanges() == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public bool SaveInterview(int applicantID, string notes, AppStatus setStatus)
        {
            // If the status is not one of the 3 acceptable ones, return false
            if((setStatus != AppStatus.INTERVIEW_COMPLETE) && (setStatus != AppStatus.APPROVED) && (setStatus != AppStatus.DENIED))
            {
                return false;
            }

            // Sets the applicant's status to one of the three, return false if they are not awaiting an interview
            if (!SetApplicationStatus(applicantID, AppStatus.WAITING_INTERVIEW, setStatus))
            {
                return false;
            }

            using (var db = new AESDbContext())
            {
                var apps = db.Applications.Where(a => a.Applicant.userID == applicantID && a.Status == setStatus);

                // Just in case
                if (!apps.Any())
                {
                    return false;
                }

                // Save the interview notes
                foreach (var app in apps)
                {
                    app.InterviewNotes = notes;
                }

                // Just in case
                if (db.SaveChanges() == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public bool SetIntervieweeApplicantStatus(int applicantID, bool approved)
        {
            return SetApplicationStatus(applicantID, AppStatus.INTERVIEW_COMPLETE, approved ? AppStatus.APPROVED : AppStatus.DENIED);
        }

        public bool SetApplicationStatus(ApplicationInfoContract app, AppStatus status)
        {
            throw new NotImplementedException();
        }

        public bool SubmitApplication(ApplicantInfoContract user)
        {
            using (var db = new AESDbContext())
            {
                // Get the user for this application
                var dbUser = db.ApplicantUsers.FirstOrDefault(u => u.userID == user.UserID);

                // If the user is not found, or doesn't have any apps
                if (dbUser == null || dbUser.Applications.Count == 0)
                {
                    return false;
                }

                // Get the applications we're looking at
                var apps = dbUser.Applications.Where(a => a.Status == AppStatus.PARTIAL);
                int appCount = apps.Count();

                foreach (var app in apps)
                {
                    app.Status = analyzeApp(app, db);
                }

                if (db.SaveChanges() != appCount)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Sets an Applicant's status to setStatus, but only if expectedStatus is its current status
        /// </summary>
        /// <param name="applicantID">The ID of the applicant</param>
        /// <param name="expectedStatus">The current status to expect</param>
        /// <param name="setStatus">The new status to set</param>
        /// <returns></returns>
        private bool SetApplicationStatus(int applicantID, AppStatus expectedStatus, AppStatus setStatus)
        {
            using (var db = new AESDbContext())
            {
                var changes = 0;

                foreach (var app in db.Applications.Where(a => a.Applicant.userID == applicantID && a.Status == expectedStatus))
                {
                    //app.Applicant = db.ApplicantUsers.Where(a => a.userID == applicantID).FirstOrDefault();
                    app.Status = setStatus;
                    changes += 1;
                }

                if (changes == 0)
                {
                    return false;
                }

                try
                {
                    // Try to save the changes
                    changes = db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }

                if (changes == 1)
                {
                    return true;
                }

                // Changes could not be saved
                return false;
            }
        }

        private AppStatus analyzeApp(Application app, AESDbContext db)
        {
            foreach (var q in app.MultiAnswers)
            {
                // Get the question from the database
                var dbQuestion = db.Questions.FirstOrDefault(qa => qa.QuestionID == q.Question.QuestionID);

                // If we can't find the question, reject the application
                if (dbQuestion == null)
                {
                    return AppStatus.AUTO_REJECT;
                }

                // Hold how many we have right, and how many we need right
                int amtRight = 0;
                int neededRight = dbQuestion.CorrectAnswerThreshold;

                // Count the amount of correct answers
                if (q.Answer1 && dbQuestion.CorrectAnswers.Contains("1")) ++amtRight;
                if (q.Answer2 && dbQuestion.CorrectAnswers.Contains("2")) ++amtRight;
                if (q.Answer3 && dbQuestion.CorrectAnswers.Contains("3")) ++amtRight;
                if (q.Answer4 && dbQuestion.CorrectAnswers.Contains("4")) ++amtRight;

                // If we didn't get enough, reject the application
                if (amtRight < neededRight)
                {
                    return AppStatus.AUTO_REJECT;
                }
            }

            // If we make it this far, move the application to the next stage
            return AppStatus.WAITING_CALL;
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
                UserInfo = ConvertTableToContract(applicant.UserInfo)
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
