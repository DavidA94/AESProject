using AES.ApplicationSvc.Contracts;
using AES.Entities.Contexts;
using AES.Entities.Tables;
using AES.Shared;
using AES.Shared.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AES.ApplicationSvc.Tests
{
    [TestClass]
    public class ApplicationSvcUnitTests
    {
        private static bool hasSaveRun = false;
        private const string ApplicantFirstName = "Joseph";
        private const string ApplicantLastName = "Morgan";

        public ApplicationSvcUnitTests()
        {
            DBFileManager.SetDataDirectory(true);
        }

        [TestMethod]
        public void ApplicationSvc_ApproveApplicant()
        {
            const string testNotes = "Success!";
            var applicationService = new ApplicationSvc();

            // Set applications for the test applicant to AppStatus.WAITING_CALL
            SetupValidWaitingCallApp();

            // Get the applicants awaiting calls using the service
            var applicantsAwaitingCalls = applicationService.GetApplicantsAwaitingCalls(new DateTime(1970, 1, 1, 6, 30, 0, 0));

            // Get the applicant being tested
            var applicant = applicantsAwaitingCalls.FirstOrDefault(a => a.FirstName == ApplicantFirstName);

            // Make sure an applicant was gotten
            Assert.IsNotNull(applicant);

            // Make sure the applicant can be called
            Assert.IsTrue(applicationService.CallApplicant((int)applicant.UserID));

            // Make sure the phone interview can be saved and approved
            Assert.IsTrue(applicationService.SavePhoneInterview((int)applicant.UserID, testNotes, true));

            using (var db = new AESDbContext())
            {
                // Ensure the notes were saved to the db
                Assert.IsTrue(db.Applications.Any(a => a.Status == AppStatus.WAITING_INTERVIEW && a.ScreeningNotes == testNotes));

                // Ensure that the interview cannot be saved and approved again
                Assert.IsFalse(applicationService.SavePhoneInterview((int)applicant.UserID, testNotes, true));

                // Ensure that the applicant's status is still awaiting an interview
                Assert.IsTrue(db.Applications.Any(a => a.Status == AppStatus.WAITING_INTERVIEW));
            
                // Ensure that the interview cannot be re-saved as rejected
                Assert.IsFalse(applicationService.SavePhoneInterview((int)applicant.UserID, testNotes, false));

                // Ensure that the applicant is still awaiting an interview
                Assert.IsTrue(db.Applications.Any(a => a.Status == AppStatus.WAITING_INTERVIEW));
            }
		}

        [TestMethod]
        public void ApplicationSvc_CallApplicant()
        {
            var applicationService = new ApplicationSvc();

            // Set up the applications
            SetupValidWaitingCallApp();

            //Get the applicants awaiting calls
            var applicantsAwaitingCalls = applicationService.GetApplicantsAwaitingCalls(new DateTime(1970, 1, 1, 6, 30, 0, 0));

            // Get the applicant we are testing
            var applicant = applicantsAwaitingCalls.FirstOrDefault(a => a.FirstName == ApplicantFirstName);

            // Ensure the applicant was gotten
            Assert.IsNotNull(applicant);

            // Ensure the call was successful
            Assert.IsTrue(applicationService.CallApplicant((int)applicant.UserID));

            using (var db = new AESDbContext())
            {
                // Ensure that, in the db, there is actually an applicant in a call
                Assert.AreEqual(db.Applications.Count(a => a.Status == AppStatus.IN_CALL), 1);

                // Ensure that the applicant cannot be called again
                Assert.IsFalse(applicationService.CallApplicant((int)applicant.UserID));

                // Ensure that the applicant can be marked as having not answered
                Assert.IsTrue(applicationService.ApplicantDidNotAnswer((int)applicant.UserID));

                // Ensure in the db that the applicant is now awaiting a call again
                Assert.AreEqual(db.Applications.Count(a => a.Status == AppStatus.WAITING_CALL && a.ApplicantID == (int)applicant.UserID), 1);
            }

            Assert.IsFalse(applicationService.ApplicantDidNotAnswer((int)applicant.UserID));
        }

        [TestMethod]
        public void ApplicationSvc_CancelApplication()
        {
            using (var db = new AESDbContext())
            {
                ApplicationSvc appSvc = new ApplicationSvc();

                // Ensure the data is in the database
                ApplicationSvc_SavePartialApplication();

                // Test with a bad first name
                var app = PartialApp4();
                app.FirstName = "Hello";
                var badFirst = appSvc.CancelApplication(app);

                // Test with a bad last anme
                app = PartialApp4();
                app.LastName = "Hello";
                var badLast = appSvc.CancelApplication(app);

                // Test with a bad DOB
                app = PartialApp4();
                app.DOB = DateTime.MinValue;
                var badDOB = appSvc.CancelApplication(app);

                // Test with no user
                var badUser = appSvc.CancelApplication(null);

                // Bad user ID
                app = PartialApp4();
                app.ApplicantID = -1;
                var badID = appSvc.CancelApplication(app);

                // Cancel the application with all valid data
                app = PartialApp4();
                var valid = appSvc.CancelApplication(app);

                // Ensure the application is not in the DB
                var appInDB = db.Applications.Any(a => a.ApplicantID == app.ApplicantID && a.Status == AppStatus.PARTIAL);

                // We need to ensure next time SavePartialApplication is called, that it will be executed
                hasSaveRun = false;

                // Ensure all the responses were valid
                Assert.IsFalse(badFirst);
                Assert.IsFalse(badLast);
                Assert.IsFalse(badDOB);
                Assert.IsFalse(badUser);
                Assert.IsFalse(badID);
                Assert.IsTrue(valid);
                Assert.IsFalse(appInDB);
            }


        }

        [TestMethod]
        public void ApplicationSvc_DenyApplicant()
        {
            const string testNotes = "Failure!";
            var applicationService = new ApplicationSvc();

            // Set applications for the test applicant to AppStatus.WAITING_CALL
            SetupValidWaitingCallApp();

            // Get the applicants awaiting calls using the service
            var applicantsAwaitingCalls = applicationService.GetApplicantsAwaitingCalls(new DateTime(1970, 1, 1, 6, 30, 0, 0));

            // Get the applicant being tested
            var applicant = applicantsAwaitingCalls.FirstOrDefault(a => a.FirstName == ApplicantFirstName);

            // Make sure an applicant was gotten
            Assert.IsNotNull(applicant);

            // Make sure the applicant can be called
            Assert.IsTrue(applicationService.CallApplicant((int)applicant.UserID));

            // Make sure the phone interview can be saved and denied
            Assert.IsTrue(applicationService.SavePhoneInterview((int)applicant.UserID, testNotes, false));

            using (var db = new AESDbContext())
            {
                // Ensure the notes were saved to the db
                Assert.IsTrue(db.Applications.Any(a => a.Status == AppStatus.CALL_DENIED && a.ScreeningNotes == testNotes));

                // Ensure that the interview cannot be saved and rejected again
                Assert.IsFalse(applicationService.SavePhoneInterview((int)applicant.UserID, testNotes, false));

                // Ensure that the applicant's status is still call denied
                Assert.IsTrue(db.Applications.Any(a => a.Status == AppStatus.CALL_DENIED));

                // Ensure that the interview cannot be re-saved as approved
                Assert.IsFalse(applicationService.SavePhoneInterview((int)applicant.UserID, testNotes, true));

                // Ensure that the applicant is still denied
                Assert.IsTrue(db.Applications.Any(a => a.Status == AppStatus.CALL_DENIED));
            }
        }

        [TestMethod]
        public void ApplicationSvc_GetApplicantsAwaitingScreening()
        {
            var applicationService = new ApplicationSvc();
            SetupValidWaitingCallApp();

            // Ensure that at least one applicant is returned inside of the time range
            var applicantsAwaitingCalls = applicationService.GetApplicantsAwaitingCalls(new DateTime(1970, 1, 1, 6, 30, 0, 0));
            Assert.IsTrue(applicantsAwaitingCalls.Any(a => a.FirstName == ApplicantFirstName && a.LastName == ApplicantLastName));

            // Ensure that no applicants are returned before the start time
            var applicantsAwaitingCallsTooEarly = applicationService.GetApplicantsAwaitingCalls(new DateTime(1970, 1, 1, 5, 30, 0, 0));
            Assert.IsFalse(applicantsAwaitingCallsTooEarly.Any(a => a.FirstName == ApplicantFirstName && a.LastName == ApplicantLastName));

            // Ensure that no applicants are returned after the end time
            var applicantsAwaitingCallsTooLate = applicationService.GetApplicantsAwaitingCalls(new DateTime(1970, 1, 1, 11, 30, 0, 0));
            Assert.IsFalse(applicantsAwaitingCallsTooLate.Any(a => a.FirstName == ApplicantFirstName && a.LastName == ApplicantLastName));
        }

        [TestMethod]
        public void ApplicationSvc_GetPartialApplication()
        {
            ApplicationSvc_SavePartialApplication();

            var a = new ApplicationSvc();

            var returnedData = a.GetApplication(PartialApp1().ApplicantID, AppStatus.PARTIAL);
            var expectedData = PartialApp4();


            Assert.AreEqual(returnedData.ApplicantID, expectedData.ApplicantID);
            Assert.IsTrue(returnedData.AppliedJobs.SequenceEqual(expectedData.AppliedJobs));
            Assert.AreEqual(returnedData.Availability.SundayStart, expectedData.Availability.SundayStart);
            Assert.AreEqual(returnedData.Availability.SundayEnd, expectedData.Availability.SundayEnd);
            Assert.AreEqual(returnedData.Availability.MondayStart, expectedData.Availability.MondayStart);
            Assert.AreEqual(returnedData.Availability.MondayEnd, expectedData.Availability.MondayEnd);
            Assert.AreEqual(returnedData.Availability.TuesdayStart, expectedData.Availability.TuesdayStart);
            Assert.AreEqual(returnedData.Availability.TuesdayEnd, expectedData.Availability.TuesdayEnd);
            Assert.AreEqual(returnedData.Availability.WednesdayStart, expectedData.Availability.WednesdayStart);
            Assert.AreEqual(returnedData.Availability.WednesdayEnd, expectedData.Availability.WednesdayEnd);
            Assert.AreEqual(returnedData.Availability.ThursdayStart, expectedData.Availability.ThursdayStart);
            Assert.AreEqual(returnedData.Availability.ThursdayEnd, expectedData.Availability.ThursdayEnd);
            Assert.AreEqual(returnedData.Availability.FridayStart, expectedData.Availability.FridayStart);
            Assert.AreEqual(returnedData.Availability.FridayEnd, expectedData.Availability.FridayEnd);
            Assert.AreEqual(returnedData.Availability.SaturdayStart, expectedData.Availability.SaturdayStart);
            Assert.AreEqual(returnedData.Availability.SaturdayEnd, expectedData.Availability.SaturdayEnd);
            Assert.AreEqual(returnedData.Availability.SundayStart, expectedData.Availability.SundayStart);
            Assert.AreEqual(returnedData.Availability.SundayEnd, expectedData.Availability.SundayEnd);
            Assert.AreEqual(returnedData.UserInfo.Address, expectedData.UserInfo.Address);
            Assert.AreEqual(returnedData.UserInfo.City, expectedData.UserInfo.City);
            Assert.AreEqual(returnedData.UserInfo.EndCallTime, expectedData.UserInfo.EndCallTime);
            Assert.AreEqual(returnedData.UserInfo.Nickname, expectedData.UserInfo.Nickname);
            Assert.AreEqual(returnedData.UserInfo.Phone, expectedData.UserInfo.Phone);
            Assert.AreEqual(returnedData.UserInfo.SalaryExpectation, expectedData.UserInfo.SalaryExpectation);
            Assert.AreEqual(returnedData.UserInfo.StartCallTime, expectedData.UserInfo.StartCallTime);
            Assert.AreEqual(returnedData.UserInfo.State, expectedData.UserInfo.State);
            Assert.AreEqual(returnedData.UserInfo.Zip, expectedData.UserInfo.Zip);

            foreach(var re in returnedData.Educations)
            {
                var ee = expectedData.Educations.FirstOrDefault(e => e.Degree == re.Degree);

                Assert.AreEqual(re.Degree, ee.Degree);
                Assert.AreEqual(re.Graduated, ee.Graduated);
                Assert.AreEqual(re.Major, ee.Major);
                Assert.AreEqual(re.SchoolAddress, ee.SchoolAddress);
                Assert.AreEqual(re.SchoolCity, ee.SchoolCity);
                Assert.AreEqual(re.SchoolCountry, ee.SchoolCountry);
                Assert.AreEqual(re.SchoolName, ee.SchoolName);
                Assert.AreEqual(re.SchoolState, ee.SchoolState);
                Assert.AreEqual(re.SchoolZIP, ee.SchoolZIP);
                Assert.AreEqual(re.YearsAttended, ee.YearsAttended);
            }

            foreach (var rj in returnedData.Jobs)
            {
                var ej = expectedData.Jobs.FirstOrDefault(e => e.EmployerAddress == rj.EmployerAddress);

                Assert.AreEqual(rj.EmployerAddress, ej.EmployerAddress);
                Assert.AreEqual(rj.EmployerCity, ej.EmployerCity);
                Assert.AreEqual(rj.EmployerCountry, ej.EmployerCountry);
                Assert.AreEqual(rj.EmployerName, ej.EmployerName);
                Assert.AreEqual(rj.EmployerPhone, ej.EmployerPhone);
                Assert.AreEqual(rj.EmployerState, ej.EmployerState);
                Assert.AreEqual(rj.EmployerZip, ej.EmployerZip);
                Assert.AreEqual(rj.EndDate, ej.EndDate);
                Assert.AreEqual(rj.EndingSalary, ej.EndingSalary);
                Assert.AreEqual(rj.ReasonForLeaving, ej.ReasonForLeaving);
                Assert.AreEqual(rj.Responsibilities, ej.Responsibilities);
                Assert.AreEqual(rj.StartDate, ej.StartDate);
                Assert.AreEqual(rj.StartingSalary, ej.StartingSalary);
                Assert.AreEqual(rj.SupervisorName, ej.SupervisorName);
            }

            foreach(var rr in returnedData.References)
            {
                var er = expectedData.References.FirstOrDefault(r => r.Company == rr.Company);

                Assert.AreEqual(rr.Company, er.Company);
                Assert.AreEqual(rr.Name, er.Name);
                Assert.AreEqual(rr.Phone, er.Phone);
                Assert.AreEqual(rr.Title, er.Title);
            }

            foreach(var rq in returnedData.QA)
            {
                var eq = expectedData.QA.FirstOrDefault(q => q.Type == rq.Type && q.QuestionID == rq.QuestionID);

                Assert.AreEqual(rq.QuestionID, eq.QuestionID);
                Assert.AreEqual(rq.Type, eq.Type);
                Assert.AreEqual(rq.ShortAnswer, eq.ShortAnswer);
                Assert.IsTrue(rq.MC_Answers.SequenceEqual(eq.MC_Answers));
            }
        }

        [TestMethod]
        public void ApplicationSvc_SavePartialApplication()
        {
            // Allows us to call this one from another test to ensure data is in the DB first
            if (!hasSaveRun)
            {
                hasSaveRun = true;
            }
            else
            {
                return;
            }

            var a = new ApplicationSvc();

            using (var db = new AESDbContext())
            {
                var app = PartialApp1();

                // Ensure this user doesn't have any current apps
                db.MultiAnswers.RemoveRange(db.MultiAnswers.Where(ma => true));
                db.ShortAnswers.RemoveRange(db.ShortAnswers.Where(sa => true));
                db.Applications.RemoveRange(db.Applications.Where(apps => apps.Applicant.userID == app.ApplicantID));
                db.SaveChanges();

                a.SavePartialApplication(app);
                var user = db.ApplicantUsers.FirstOrDefault(u => u.userID == app.ApplicantID);
                AssertPartialApp1(user, app);
            }
            using (var db = new AESDbContext())
            {
                var app = PartialApp2();
                a.SavePartialApplication(app);
                var user = db.ApplicantUsers.FirstOrDefault(u => u.userID == app.ApplicantID);
                AssertPartialApp2(user, app);
            }
            using (var db = new AESDbContext())
            {
                var app = PartialApp3();
                a.SavePartialApplication(app);
                var user = db.ApplicantUsers.FirstOrDefault(u => u.userID == app.ApplicantID);
                AssertPartialApp3(user, app);
            }
            using (var db = new AESDbContext())
            {
                var app = PartialApp4();
                a.SavePartialApplication(app);
                var user = db.ApplicantUsers.FirstOrDefault(u => u.userID == app.ApplicantID);
                AssertPartialApp4_Pass(user, app);
            }
        }

        [TestMethod]
        public void ApplicationSvc_SubmitApplication()
        {
            var appSvc = new ApplicationSvc();
            
            // Get the application
            var app = PartialApp4_BadAnswers();

            using (var db = new AESDbContext())
            {
                // Remove any old applications for this user
                db.Applications.RemoveRange(db.Applications.Where(apps => apps.Applicant.userID == app.ApplicantID));

                // Save/Submit the application
                appSvc.SavePartialApplication(app);
                appSvc.SubmitApplication(new ApplicantInfoContract() { UserID = app.ApplicantID });
            }

            // Refresh the context
            using (var db = new AESDbContext())
            { 
                // Get the status of both jobs submitted
                int job1 = app.AppliedJobs[0];
                int job2 = app.AppliedJobs[1];
                AppStatus status1 = db.Applications.FirstOrDefault(a => a.Applicant.userID == app.ApplicantID &&
                                                                        a.JobID == job1).Status;
                AppStatus status2 = db.Applications.FirstOrDefault(a => a.Applicant.userID == app.ApplicantID &&
                                                                        a.JobID == job2).Status;

                // Asure one was moved alone while the other was accepted
                Assert.IsTrue((status1 == AppStatus.WAITING_CALL && status2 == AppStatus.AUTO_REJECT) ||
                              (status2 == AppStatus.WAITING_CALL && status1 == AppStatus.AUTO_REJECT));
            }
        }


        #region Helper Methods

        private ApplicationInfoContract PartialApp1()
        {
            const string FIRST_NAME = "Application";
            const string LAST_NAME = "Service";
            const string SSN = "333-33-3333";
            DateTime DOB = new DateTime(2001, 01, 01);

            var s = new SecuritySvc.SecuritySvc();
            int userID = (int)s.ValidateUser(new ApplicantInfoContract(FIRST_NAME, LAST_NAME, SSN, DOB)).UserID;

            using (var db = new AESDbContext())
            {
                return new ApplicationInfoContract()
                {
                    ApplicantID = userID,
                    AppliedJobs = db.Jobs.Select(j => j.JobID).Take(2).ToList(),
                    Availability = new AvailabilityContract()
                    {
                        MondayStart = new TimeSpan(8, 0, 0),
                        MondayEnd = new TimeSpan(20, 0, 0),
                        WednesdayStart = new TimeSpan(8, 0, 0),
                        WednesdayEnd = new TimeSpan(16, 0, 0),
                        FridayStart = new TimeSpan(7, 0, 0),
                        FridayEnd = new TimeSpan(23, 0, 0)
                    },
                    FirstName = FIRST_NAME,
                    LastName = LAST_NAME,
                    DOB = DOB,
                    UserInfo = new UserInfoContract()
                    {
                        Address = "123 Real St.",
                        City = "Realtown",
                        EndCallTime = new TimeSpan(16, 0, 0),
                        Nickname = "App",
                        Phone = "333-555-4422",
                        SalaryExpectation = 100000,
                        StartCallTime = new TimeSpan(14, 0, 0),
                        State = "RT",
                        Zip = 55667
                    }
                };
            }
        }
		
        private void AssertPartialApp1(ApplicantUser user, ApplicationInfoContract app)
        {
            Assert.AreEqual(user.UserInfo.Address, app.UserInfo.Address);
            Assert.AreEqual(user.UserInfo.CallEndTime, app.UserInfo.EndCallTime);
            Assert.AreEqual(user.UserInfo.CallStartTime, app.UserInfo.StartCallTime);
            Assert.AreEqual(user.UserInfo.City, app.UserInfo.City);
            Assert.AreEqual(user.UserInfo.Nickname, app.UserInfo.Nickname);
            Assert.AreEqual(user.UserInfo.Phone, app.UserInfo.Phone);
            Assert.AreEqual(user.UserInfo.SalaryExpectation, app.UserInfo.SalaryExpectation);
            Assert.AreEqual(user.UserInfo.State, app.UserInfo.State);
            Assert.AreEqual(user.UserInfo.Zip, app.UserInfo.Zip);

            Assert.AreEqual(user.Availability.SundayStart, app.Availability.SundayStart);
            Assert.AreEqual(user.Availability.SundayEnd, app.Availability.SundayEnd);
            Assert.AreEqual(user.Availability.MondayStart, app.Availability.MondayStart);
            Assert.AreEqual(user.Availability.MondayEnd, app.Availability.MondayEnd);
            Assert.AreEqual(user.Availability.TuesdayStart, app.Availability.TuesdayStart);
            Assert.AreEqual(user.Availability.TuesdayEnd, app.Availability.TuesdayEnd);
            Assert.AreEqual(user.Availability.WednesdayStart, app.Availability.WednesdayStart);
            Assert.AreEqual(user.Availability.WednesdayEnd, app.Availability.WednesdayEnd);
            Assert.AreEqual(user.Availability.ThursdayStart, app.Availability.ThursdayStart);
            Assert.AreEqual(user.Availability.ThursdayEnd, app.Availability.ThursdayEnd);
            Assert.AreEqual(user.Availability.FridayStart, app.Availability.FridayStart);
            Assert.AreEqual(user.Availability.FridayEnd, app.Availability.FridayEnd);
            Assert.AreEqual(user.Availability.SaturdayStart, app.Availability.SaturdayStart);
            Assert.AreEqual(user.Availability.SaturdayEnd, app.Availability.SaturdayEnd);
        }

        private ApplicationInfoContract PartialApp2()
        {
            var app = PartialApp1();
            app.UserInfo.Nickname = "New Nickname";
            app.Educations = new List<EducationHistoryContract>()
            {
                new EducationHistoryContract()
                {
                    Degree = Shared.DegreeType.AA,
                    Major = "General Studies",
                    SchoolCity = "Weirdtown",
                    SchoolCountry = "USA",
                    SchoolName = "Weird University",
                    SchoolState = "WT",
                    SchoolZIP = 22334,
                    YearsAttended = 2,
                },
                new EducationHistoryContract()
                {
                    Degree = Shared.DegreeType.BA,
                    Graduated = new DateTime(2016, 03, 15),
                    Major = "Amazing Major",
                    SchoolAddress = "123 Amazing St.",
                    SchoolCity = "Normaltown",
                    SchoolCountry = "USA",
                    SchoolName = "Amazing Degree University",
                    SchoolState = "NT",
                    SchoolZIP = 33445,
                    YearsAttended = 2
                }
            };

            return app;
        }
		
        private void AssertPartialApp2(ApplicantUser user, ApplicationInfoContract app)
        {
            AssertPartialApp1(user, app);

            foreach(var lhsE in app.Educations)
            {
                foreach(var rhsE in user.EducationHistory)
                {
                    // The inner and outer will only match one way, so break out of the wrong way
                    if(lhsE.Degree != rhsE.Degree)
                    {
                        break;
                    }

                    Assert.AreEqual(lhsE.Degree, rhsE.Degree);
                    Assert.AreEqual(lhsE.Graduated, rhsE.GraduationDate);
                    Assert.AreEqual(lhsE.Major, rhsE.Major);
                    Assert.AreEqual(lhsE.SchoolAddress, rhsE.SchoolAddress);
                    Assert.AreEqual(lhsE.SchoolCity, rhsE.SchoolCity);
                    Assert.AreEqual(lhsE.SchoolCountry, rhsE.SchoolCountry);
                    Assert.AreEqual(lhsE.SchoolName, rhsE.SchoolName);
                    Assert.AreEqual(lhsE.SchoolState, rhsE.SchoolState);
                    Assert.AreEqual(lhsE.SchoolZIP, rhsE.SchoolZip);
                    Assert.AreEqual(lhsE.YearsAttended, rhsE.YearsAttended);
                }
            }            
        }

        private ApplicationInfoContract PartialApp3()
        {
            var app = PartialApp2();

            app.Jobs = new List<JobHistoryContract>()
            {
                new JobHistoryContract()
                {
                    EmployerAddress = "5601 W. Buckeye Rd.",
                    EmployerCity = "Phoenix",
                    EmployerCountry = "USA",
                    EmployerName = "KTTS",
                    EmployerPhone = "555-555-5555",
                    EmployerState = "AZ",
                    EmployerZip = 85043,
                    EndDate = new DateTime(2013, 09, 02),
                    EndingSalary = 50000,
                    ReasonForLeaving = "Amazing Opportunities",
                    Responsibilities = "Many",
                    StartDate = new DateTime(2011, 09, 03),
                    StartingSalary = 20,
                    SupervisorName = "Nobody"
                }
            };

            return app;
        }
		
        private void AssertPartialApp3(ApplicantUser user, ApplicationInfoContract app)
        {
            AssertPartialApp2(user, app);

            // Should only be one
            foreach(var e in user.EmploymentHistory)
            {
                Assert.AreEqual(e.EmployerAddress, app.Jobs[0].EmployerAddress);
                Assert.AreEqual(e.EmployerCity, app.Jobs[0].EmployerCity);
                Assert.AreEqual(e.EmployerCountry, app.Jobs[0].EmployerCountry);
                Assert.AreEqual(e.EmployerName, app.Jobs[0].EmployerName);
                Assert.AreEqual(e.EmployerPhone, app.Jobs[0].EmployerPhone);
                Assert.AreEqual(e.EmployerState, app.Jobs[0].EmployerState);
                Assert.AreEqual(e.EmployerZip, app.Jobs[0].EmployerZip);
                Assert.AreEqual(e.EndDate, app.Jobs[0].EndDate);
                Assert.AreEqual(e.EndingSalary, app.Jobs[0].EndingSalary);
                Assert.AreEqual(e.ReasonForLeaving, app.Jobs[0].ReasonForLeaving);
                Assert.AreEqual(e.Responsibilities, app.Jobs[0].Responsibilities);
                Assert.AreEqual(e.StartDate, app.Jobs[0].StartDate);
                Assert.AreEqual(e.StartingSalary, app.Jobs[0].StartingSalary);
                Assert.AreEqual(e.SupervisorName, app.Jobs[0].SupervisorName);
            }
        }

        private ApplicationInfoContract PartialApp4()
        {
            var app = PartialApp3();

            using (var db = new AESDbContext())
            {
                app.QA = new List<QAContract>()
                {
                    new QAContract()
                    {
                        QuestionID = db.Questions.FirstOrDefault(q => q.Text == "What do you feel you can bring to the job?").QuestionID,
                        Type = QuestionType.SHORT,
                        ShortAnswer = "Cleaning skills"
                    },
                    new QAContract()
                    {
                        QuestionID = db.Questions.FirstOrDefault(q => q.Text == "Can you lift more than 50 pounds?").QuestionID,
                        Type = QuestionType.RADIO,
                        MC_Answers = new List<bool> { true, false, false, false }
                    },
                    new QAContract()
                    {
                        QuestionID = db.Questions.FirstOrDefault(q => q.Text == "Which of the following are cleaning products? (Check all that apply)").QuestionID,
                        Type = QuestionType.CHECKBOX,
                        MC_Answers = new List<bool> { true, true, false, false }
                    },
                    new QAContract()
                    {
                        QuestionID = db.Questions.FirstOrDefault(q => q.Text == "How would you make a good sales associate?").QuestionID,
                        Type = QuestionType.SHORT,
                        ShortAnswer = "cuz of my falwless speeling"
                    },
                    new QAContract()
                    {
                        QuestionID = db.Questions.FirstOrDefault(q => q.Text == "Are you applying for the job of sales associate?").QuestionID,
                        Type = QuestionType.RADIO,
                        MC_Answers = new List<bool> { true, false, false, false }
                    },
                    new QAContract()
                    {
                        QuestionID = db.Questions.FirstOrDefault(q => q.Text == "Which are sales terms? (Check all that apply)").QuestionID,
                        Type = QuestionType.CHECKBOX,
                        MC_Answers = new List<bool> { true, true, false, false }
                    }
                };
            }

            return app;
        }
		
        private ApplicationInfoContract PartialApp4_BadAnswers()
        {
            var app = PartialApp3();

            using (var db = new AESDbContext())
            {
                app.QA = new List<QAContract>()
                {
                    new QAContract()
                    {
                        QuestionID = db.Questions.FirstOrDefault(q => q.Text == "What do you feel you can bring to the job?").QuestionID,
                        Type = QuestionType.SHORT,
                        ShortAnswer = "Cleaning skills"
                    },
                    new QAContract()
                    {
                        QuestionID = db.Questions.FirstOrDefault(q => q.Text == "Can you lift more than 50 pounds?").QuestionID,
                        Type = QuestionType.RADIO,
                        MC_Answers = new List<bool> { true, false, false, false }
                    },
                    new QAContract()
                    {
                        QuestionID = db.Questions.FirstOrDefault(q => q.Text == "Which of the following are cleaning products? (Check all that apply)").QuestionID,
                        Type = QuestionType.CHECKBOX,
                        MC_Answers = new List<bool> { false, true, false, false }
                    }
                };
            }

            return app;
        }

		private void AssertPartialApp4_Pass(ApplicantUser user, ApplicationInfoContract app)
        {
            AssertPartialApp3(user, app);

            var application = user.Applications.FirstOrDefault(a => a.Status == Shared.AppStatus.PARTIAL && 
                                                                    a.Applicant.userID == user.userID &&
                                                                    a.MultiAnswers.Count > 0 &&
                                                                    a.ShortAnswers.Count > 0 &&
                                                                    a.Job.Title == "Maintenece Technician");

            var answer = application.MultiAnswers.FirstOrDefault(a => a.Question.Text == "Can you lift more than 50 pounds?");
            Assert.IsTrue(answer.Answer1);
            Assert.IsFalse(answer.Answer2);
            Assert.IsFalse(answer.Answer3);
            Assert.IsFalse(answer.Answer4);

            answer = application.MultiAnswers.FirstOrDefault(a => a.Question.Text == "Which of the following are cleaning products? (Check all that apply)");
            Assert.IsTrue(answer.Answer1);
            Assert.IsTrue(answer.Answer2);
            Assert.IsFalse(answer.Answer3);
            Assert.IsFalse(answer.Answer4);

            var sanswer = application.ShortAnswers.FirstOrDefault(a => a.Question.Text == "What do you feel you can bring to the job?");
            Assert.AreEqual(sanswer.Answer, "Cleaning skills");
        }
		
		private void SetupValidWaitingCallApp()
        {
            using (var db = new AESDbContext())
            {
                foreach (var applications in db.ApplicantUsers.Where(a => a.FirstName == ApplicantFirstName).Select(a => a.Applications).ToList())
                {
                    foreach (var app in applications)
                    {
                        app.ScreeningNotes = null;
                        app.Status = AppStatus.WAITING_CALL;
                    }
                }

                db.SaveChanges();
            }
        }

		#endregion
                
    }
}
