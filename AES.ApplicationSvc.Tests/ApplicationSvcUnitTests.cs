using AES.ApplicationSvc.Contracts;
using AES.Entities.Contexts;
using AES.Entities.Tables;
using AES.Shared;
using AES.Shared.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AES.ApplicationSvc.Tests
{
    [TestClass]
    public class ApplicationSvcUnitTests
    {
        public ApplicationSvcUnitTests()
        {
            DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            while (dir.Name != "AESProject")
            {
                dir = dir.Parent;
            }
            dir = dir.CreateSubdirectory("TestDB");
            AppDomain.CurrentDomain.SetData("DataDirectory", dir.FullName);
        }

        private static bool hasSaveRun = false;

        [TestMethod]
        public void TC10_SavePartialApplication()
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
                AssertPartialApp4_Pass1(user, app);
            }
        }

        [TestMethod]
        public void TC10_GetPartialApplication()
        {
            TC10_SavePartialApplication();

            var a = new ApplicationSvc();

            var returnedData = a.GetApplication(new ApplicantInfoContract() { UserID = PartialApp1().ApplicantID });
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
                var eq = expectedData.QA.FirstOrDefault(q => q.Type == rq.Type);

                Assert.AreEqual(rq.QuestionID, eq.QuestionID);
                Assert.AreEqual(rq.Type, eq.Type);
                Assert.AreEqual(rq.ShortAnswer, eq.ShortAnswer);
                Assert.IsTrue(rq.MC_Answers.SequenceEqual(eq.MC_Answers));
            }
        }

        [TestMethod]
        public void TC10_SubmitApplication()
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
                    AppliedJobs = db.Jobs.Select(j => j.ID).Take(2).ToList(),
                    Availability = new AvailabilityContract()
                    {
                        MondayStart = new TimeSpan(8, 0, 0),
                        MondayEnd = new TimeSpan(20, 0, 0),
                        WednesdayStart = new TimeSpan(8, 0, 0),
                        WednesdayEnd = new TimeSpan(16, 0, 0),
                        FridayStart = new TimeSpan(7, 0, 0),
                        FridayEnd = new TimeSpan(23, 0, 0)
                    },
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
                        QuestionID = db.Questions.FirstOrDefault(q => q.Text == "What do you feel you can bring to the job?").ID,
                        Type = QuestionType.SHORT,
                        ShortAnswer = "Cleaning skills"
                    },
                    new QAContract()
                    {
                        QuestionID = db.Questions.FirstOrDefault(q => q.Text == "Can you lift more than 50 pounds?").ID,
                        Type = QuestionType.RADIO,
                        MC_Answers = new List<bool> { true, false, false, false }
                    },
                    new QAContract()
                    {
                        QuestionID = db.Questions.FirstOrDefault(q => q.Text == "Which of the following are cleaning products? (Check all that apply)").ID,
                        Type = QuestionType.CHECKBOX,
                        MC_Answers = new List<bool> { true, true, false, false }
                    }
                };
            }

            return app;
        }
        private void AssertPartialApp4_Pass1(ApplicantUser user, ApplicationInfoContract app)
        {
            AssertPartialApp3(user, app);

            var application = user.Applications.FirstOrDefault(a => a.Status == Shared.AppStatus.PARTIAL && 
                                                                    a.Applicant.userID == user.userID &&
                                                                    a.MultiAnswers.Count > 0 &&
                                                                    a.ShortAnswers.Count > 0);

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

        private ApplicationInfoContract PartialApp4_BadAnswers()
        {
            var app = PartialApp3();

            using (var db = new AESDbContext())
            {
                app.QA = new List<QAContract>()
                {
                    new QAContract()
                    {
                        QuestionID = db.Questions.FirstOrDefault(q => q.Text == "What do you feel you can bring to the job?").ID,
                        Type = QuestionType.SHORT,
                        ShortAnswer = "Cleaning skills"
                    },
                    new QAContract()
                    {
                        QuestionID = db.Questions.FirstOrDefault(q => q.Text == "Can you lift more than 50 pounds?").ID,
                        Type = QuestionType.RADIO,
                        MC_Answers = new List<bool> { true, false, false, false }
                    },
                    new QAContract()
                    {
                        QuestionID = db.Questions.FirstOrDefault(q => q.Text == "Which of the following are cleaning products? (Check all that apply)").ID,
                        Type = QuestionType.CHECKBOX,
                        MC_Answers = new List<bool> { false, true, false, false }
                    }
                };
            }

            return app;
        }
    }
}
