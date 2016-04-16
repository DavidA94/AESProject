using System;
using AES.Entities.Tables;
using AES.Shared;

namespace AES.Entities.Migrations
{
    using Contexts;
    using System.Data.Entity.Migrations;
    using System.Linq;
    internal sealed class AESDbConfiguration : DbMigrationsConfiguration<AESDbContext>
    {
        public AESDbConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(AESDbContext context)
        {
            //context.Applications.RemoveRange(context.Applications);
            //context.ApplicantUsers.RemoveRange(context.ApplicantUsers);
            //context.Questions.RemoveRange(context.Questions);
            //context.Jobs.RemoveRange(context.Jobs);
            //context.JobOpenings.RemoveRange(context.JobOpenings);
            //context.UserInfo.RemoveRange(context.UserInfo);
            //context.Stores.RemoveRange(context.Stores);
            //context.Availabilities.RemoveRange(context.Availabilities);
            //context.EducationHistories.RemoveRange(context.EducationHistories);
            //context.MultiAnswers.RemoveRange(context.MultiAnswers);
            //context.ShortAnswers.RemoveRange(context.ShortAnswers);
            //context.JobHistories.RemoveRange(context.JobHistories);
            //context.References.RemoveRange(context.References);

            // If this user exists, assume we're already seeded
            var SSN = Encryption.Encrypt("123-45-6789");
            if(context.ApplicantUsers.FirstOrDefault(a => a.SSN == SSN) != null)
            {
                return;
            }

            var portlandStore = new Store
            {
                Address = "8000 SW Barbur Blvd.",
                City = "Portland",
                State = "OR",
                Name = "AES Electronics Portland",
                Phone = "503-555-0000",
                Zip = 97219
            };

            var tualatinStore = new Store
            {
                Address = "2700 SW Tualatin Sherwood Rd.",
                City = "Tualatin",
                State = "OR",
                Name = "AES Electronics Tualatin",
                Phone = "503-555-0000",
                Zip = 97062
            };

            var salesAssociateJob = new Job
            {
                title = "Sales Associate",
                descShort = "Responsible for all sales activities, from lead generation through to close. The successful candidate will be able to elevate company standards, achieve sales goals and meet clients expectations.",
                descLong = "Responsibilities:\n* Ensure high levels of customer satisfaction through excellent sales service\n* Assess customers needs and provide assistance and information on product features\n* 'Go the extra mile' to drive sales\n* Maintain in-stock and presentable condition assigned areas\n* Actively seek out customers in store\n* Remain knowledgeable on products offered and discuss available options\n* Cross sell products\n* Team up with co-workers to ensure proper customer service\n* Build productive trust relationships with customers\n\nRequirements:\n* Proven working experience as sales associate\n* Basic understanding of sales principles and customer service practices\n* Proficiency in English\n* Working knowledge of customer and market dynamics and requirements\n* Track record of over-achieving sales quota\n* Solid communication and interpersonal skills\n* Customer service focus\n* High school degree; BS degree in Marketing or related field would be a plus"
            };

            var janitorJob = new Job
            {
                title = "Maintenece Technician",
                descShort = "We are looking for a thorough Maintenance Technician to undertake the responsibility to preserve the good condition and functionality of premises. You will perform maintenance tasks of great variety such as painting, HVAC installations, landscaping etc.",
                descLong = "A Maintenance Technician is a thorough professional with a practical mind and attention to detail. The ideal candidate will be able to work autonomously and responsibly by observing all health and safety guidelines.\nThe goal is to maintain the buildings and common areas in the best possible condition."
            };

            context.Stores.AddOrUpdate(
                portlandStore,
                tualatinStore
            );

            context.SaveChanges();

            context.Jobs.AddOrUpdate(
                salesAssociateJob,
                janitorJob
            );

            context.SaveChanges();

            var tualatinJanitorOpening = new JobOpening
            {
                Job = janitorJob
            };
            tualatinJanitorOpening.Store = tualatinStore;

            var portlandJanitorOpening = new JobOpening
            {
                Job = janitorJob
            };
            portlandJanitorOpening.Store = portlandStore;

            var tualatinSalesOpening = new JobOpening
            {
                    Job = salesAssociateJob
            };
            tualatinSalesOpening.Store = tualatinStore;

            var portlandSalesOpening = new JobOpening
            {
                Job = salesAssociateJob
            };
            portlandSalesOpening.Store = portlandStore;

            context.JobOpenings.AddOrUpdate(
                tualatinJanitorOpening,
                portlandJanitorOpening,
                tualatinSalesOpening,
                portlandSalesOpening
            );

            context.SaveChanges();

            var userInfo = new UserInfo()
            {
                CallEndTime = new TimeSpan(10, 0, 0),
                CallStartTime = new TimeSpan(6, 0, 0),
                State = "OR",
                Address = "1200 SW 185th Ave.",
                City = "Beaverton",
                Phone = "503-555-1234",
                Nickname = "Joe",
                SalaryExpectation = 8,
                Zip = 97219
            };

            context.UserInfo.AddOrUpdate(userInfo);

            context.SaveChanges();

            var availability = new Availability
            {
                MondayStart = new TimeSpan(7, 0, 0),
                MondayEnd = new TimeSpan(20, 0, 0),
                TuesdayStart = new TimeSpan(7, 0, 0),
                TuesdayEnd = new TimeSpan(20, 0, 0),
                WednesdayStart = new TimeSpan(7, 0, 0),
                WednesdayEnd = new TimeSpan(20, 0, 0),
                ThursdayStart = new TimeSpan(7, 0, 0),
                ThursdayEnd = new TimeSpan(20, 0, 0),
                FridayStart = new TimeSpan(7, 0, 0),
                FridayEnd = new TimeSpan(20, 0, 0)
            };

            context.Availabilities.AddOrUpdate(availability);

            context.SaveChanges();

            var applicantUser = new ApplicantUser()
            {
                UserInfo = userInfo,
                Availability = availability,
                DOB = new DateTime(1989, 3, 14),
                FirstName = "Joseph",
                LastName = "Morgan",
                SSN = Encryption.Encrypt("123-45-6789")
            };

            context.ApplicantUsers.AddOrUpdate(applicantUser);

            context.SaveChanges();

            var reference = new Reference
            {
                Applicant = applicantUser,
                Company = "Walgreen's",
                Name = "Raymond Hammond",
                Phone = "503-444-5555",
                Title = "Floor Supervisor"
            };

            context.References.AddOrUpdate(reference);

            context.SaveChanges();

            var employmentHistory = new JobHistory()
            {
                Applicant = applicantUser,
                EmployerAddress = "1234 Maple St.",
                EmployerCity = "Tigard",
                EmployerCountry = "USA",
                EmployerName = "Bob's Beef Barn",
                EmployerPhone = "503-333-2222",
                EmployerState = "OR",
                EmployerZip = 98765,
                EndDate = new DateTime(2015, 11, 8),
                EndingSalary = 8,
                StartDate = new DateTime(2013, 1, 14),
                StartingSalary = 7,
                SupervisorName = "Robert Reynolds",
                Responsibilities = "Janitorial work",
                ReasonForLeaving = "The company folded"
            };

            context.JobHistories.AddOrUpdate(employmentHistory);

            context.SaveChanges();

            var educationHistory = new EducationHistory()
            {
                Applicant = applicantUser,
                Degree = DegreeType.AA,
                GraduationDate = new DateTime(2010, 6, 27),
                Major = "Janitorial Studies",
                SchoolAddress = "1 School Ave.",
                SchoolCity = "Bend",
                SchoolCountry = "USA",
                SchoolName = "Bend Community College",
                SchoolState = "OR",
                SchoolZip = 96421,
                YearsAttended = 2
            };

            context.EducationHistories.AddOrUpdate(educationHistory);

            context.SaveChanges();

            var shortQuestion = new JobQuestion()
            {
                Type = QuestionType.SHORT,
                Text = "What do you feel you can bring to the job?"
            };
            shortQuestion.Jobs.Add(janitorJob);

            context.Questions.AddOrUpdate(shortQuestion);

            context.SaveChanges();

            var radioQuestion = new JobQuestion()
            {
                Type = QuestionType.RADIO,
                Text = "Can you lift more than 50 pounds?",
                Option1 = "Yes",
                Option2 = "No",
                CorrectAnswerThreshold = 1,
                CorrectAnswers = "1"
            };
            radioQuestion.Jobs.Add(janitorJob);

            context.Questions.AddOrUpdate(radioQuestion);

            context.SaveChanges();

            var checkQuestion = new JobQuestion()
            {
                Type = QuestionType.CHECKBOX,
                Text = "Which of the following are cleaning products? (Check all that apply)",
                Option1 = "Mop",
                Option2 = "Turbo Encabulator",
                Option3 = "Barkeeper's Friend",
                Option4 = "",
                CorrectAnswerThreshold = 1,
                CorrectAnswers = "13"
            };
            checkQuestion.Jobs.Add(janitorJob);

            context.Questions.AddOrUpdate(checkQuestion);

            context.SaveChanges();

            var application = new Application()
            {
                Status = AppStatus.WAITING_CALL,
                Applicant = applicantUser,
                Job = janitorJob,
                Timestamp = new DateTime(2016, 3, 11)
            };

            context.Applications.AddOrUpdate(application);

            context.SaveChanges();

            var shortAnswer = new ApplicationShortAnswer
            {
                Answer = "My mop, my bucket, and my experience",
                Question = shortQuestion
            };

            context.ShortAnswers.AddOrUpdate(shortAnswer);

            context.SaveChanges();

            var radioAnswer = new ApplicationMultiAnswer
            {
                Question = radioQuestion,
                Answer1 = true
            };

            var checkAnswer = new ApplicationMultiAnswer
            {
                Question = checkQuestion,
                Answer1 = true,
                Answer2 = false,
                Answer3 = true,
                Answer4 = false
            };

            context.MultiAnswers.AddOrUpdate(radioAnswer, checkAnswer);

            context.SaveChanges();
        }
    }
}
