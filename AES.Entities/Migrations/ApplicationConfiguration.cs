using System;
using AES.Entities.Tables;
using AES.Shared;

namespace AES.Entities.Migrations
{
    using Contexts;
    using System.Data.Entity.Migrations;

    internal sealed class ApplicationConfiguration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public ApplicationConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            context.Applications.RemoveRange(context.Applications);
            context.Users.RemoveRange(context.Users);
            context.Questions.RemoveRange(context.Questions);

            var userInfo = new UserInfo()
            {
                State = "",
                Address = "",
                City = "",
                Phone = "",
                Nickname = "",
                SalaryExpectation = 8,
                Zip = 97219
            };

            var applicantUser = new ApplicantUser()
            {
                UserInfo = userInfo,
                Availability = null,
                CallEndTime = new TimeSpan(10, 0, 0),
                DOB = new DateTime(1989, 3, 14),
                CallStartTime = new TimeSpan(6, 0, 0),
                FirstName = "",
                LastName = "",
                SSN = ""
            };

            var employmentHistory = new JobHistory()
            {
                Applicant = applicantUser,
                EmployerAddress = "",
                EmployerCity = "",
                EmployerCountry = "",
                EmployerName = "",
                EmployerPhone = "",
                EmployerState = "",
                EmployerZip = 98765,
                EndDate = new DateTime(2015, 11, 8),
                EndingSalary = 8,
                StartDate = new DateTime(2013, 1, 14),
                StartingSalary = 7,
                SupervisorName = "",
                Responsibilities = "",
                ReasonForLeaving = ""
            };

            var educationHistory = new EducationHistory()
            {
                Applicant = applicantUser,
                Degree = DegreeType.AA,
                GraduationDate = new DateTime(2010, 6, 27),
                Major = "",
                SchoolAddress = "",
                SchoolCity = "",
                SchoolCountry = "",
                SchoolName = "",
                SchoolState = "",
                SchoolZip = 87654,
                YearsAttended = 4
            };

            var job = new Job()
            {
                descShort = "",
                descLong = "",
                title = ""
            };

            var shortQuestion = new JobQuestion()
            {
                Job = job,
                Type = QuestionType.SHORT,
                Text = ""
            };

            var radioQuestion = new JobQuestion()
            {
                Job = job,
                Type = QuestionType.RADIO,
                Text = "Can you lift more than 50 pounds?",
                Option1 = "Yes",
                Option2 = "No",
                CorrectAnswerThreshold = 1,
                CorrectAnswers = "1"
            };

            var checkQuestion = new JobQuestion()
            {
                Job = job,
                Type = QuestionType.CHECKBOX,
                Text = "Which of the following are computer components? (Check all that apply)",
                Option1 = "Hard Drive",
                Option2 = "Turbo Encabulator",
                Option3 = "Phase Array",
                Option4 = "Motherboard",
                CorrectAnswerThreshold = 1,
                CorrectAnswers = "13"
            };

            var application = new Application()
            {
                Status = AppStatus.WAITING_CALL,
                Applicant = applicantUser,
                Job = job,
                Timestamp = new DateTime(2016, 3, 11)
            };

        }
    }
}
