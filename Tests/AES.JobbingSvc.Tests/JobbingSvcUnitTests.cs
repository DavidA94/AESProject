using System;
using AES.Entities.Contexts;
using AES.Entities.Tables;
using AES.JobbingSvc.Contracts;
using AES.Shared;
using AES.Shared.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AES.JobbingSvc.Tests
{
    [TestClass]
    public class JobbingSvcUnitTests
    {
        private string LONG_DESC = "A super long description";
        private string SHORT_DESC = "Hello";
        private string TITLE = "Hello World";

        private string QUESTION = "Are you qualified to fill out this application?";
        private List<bool> ANSWERS = new List<bool> { true, false, false, true };
        private List<string> OPTIONS = new List<string> { "Opt1", "Opt2", "Opt3", "Opt4" };


        public JobbingSvcUnitTests()
        {
            DBFileManager.SetDataDirectory(true);
        }

        [TestMethod]
        public void JobbingSvc_AddInvalidJob()
        {
            JobbingSvc jobSvc = new JobbingSvc();

            var badLong = jobSvc.AddJob(new JobContract() { ShortDescription = SHORT_DESC, Title = TITLE });
            var badShort = jobSvc.AddJob(new JobContract() { LongDescription = LONG_DESC, Title = TITLE });
            var badTitle = jobSvc.AddJob(new JobContract() { LongDescription = LONG_DESC, ShortDescription = SHORT_DESC });
            var nullJob = jobSvc.AddJob(null);

            Assert.AreEqual(badLong, JobbingResponse.INVALID);
            Assert.AreEqual(badShort, JobbingResponse.INVALID);
            Assert.AreEqual(badTitle, JobbingResponse.INVALID);
            Assert.AreEqual(nullJob, JobbingResponse.INVALID);
        }

        [TestMethod]
        public void JobbingSvc_AddValidAndDuplicateJob()
        {
            JobbingSvc jobSvc = new JobbingSvc();

            // Ensure the job is not in the DB
            using (var db = new AESDbContext())
            {
                if (db.Jobs.Any(j => j.Title == TITLE))
                {
                    db.Jobs.Remove(db.Jobs.FirstOrDefault(j => j.Title == TITLE));
                    db.SaveChanges();
                }
            }

            // Use the service
            var validJob = jobSvc.AddJob(new JobContract() { LongDescription = LONG_DESC, ShortDescription = SHORT_DESC, Title = TITLE });
            var duplicateJob = jobSvc.AddJob(new JobContract() { LongDescription = LONG_DESC, ShortDescription = SHORT_DESC, Title = TITLE });
            Job dbJob;

            // Get the job from the DB
            using (var db = new AESDbContext())
            {
                dbJob = db.Jobs.FirstOrDefault(j => j.Title == TITLE);
            }

            // Ensure we got what we wanted
            Assert.AreEqual(validJob, JobbingResponse.SUCCESS);
            Assert.AreEqual(duplicateJob, JobbingResponse.DUPLICATE);
            Assert.IsNotNull(dbJob);
        }

        [TestMethod]
        public void JobbingSvc_AddInvaildQuestion()
        {
            JobbingSvc jobSvc = new JobbingSvc();

            var badQText = jobSvc.AddQuestion(new QAContract() { Question = "", Type = QuestionType.SHORT });
            var badOptions = jobSvc.AddQuestion(new QAContract()
            {
                MC_Answers = ANSWERS.Take(1).ToList(),
                NeededRight = 2,
                Options = OPTIONS.Take(1).ToList(),
                Question = QUESTION,
                Type = QuestionType.RADIO
            });
            var mismatchOptionsAnswers = jobSvc.AddQuestion(new QAContract()
            {
                MC_Answers = ANSWERS.Take(3).ToList(),
                NeededRight = 2,
                Options = OPTIONS,
                Question = QUESTION,
                Type = QuestionType.RADIO
            });
            var needMoreRight = jobSvc.AddQuestion(new QAContract()
            {
                MC_Answers = ANSWERS.Take(3).ToList(),
                NeededRight = 0,
                Options = OPTIONS,
                Question = QUESTION,
                Type = QuestionType.RADIO
            });
            var nullQ = jobSvc.AddQuestion(null);

            Assert.AreEqual(badQText, JobbingResponse.INVALID);
            Assert.AreEqual(badOptions, JobbingResponse.INVALID);
            Assert.AreEqual(mismatchOptionsAnswers, JobbingResponse.INVALID);
            Assert.AreEqual(needMoreRight, JobbingResponse.INVALID);
            Assert.AreEqual(nullQ, JobbingResponse.INVALID);
        }

        [TestMethod]
        public void JobbingSvc_AddValidAndDuplicateQuestion()
        {
            JobbingSvc jobSvc = new JobbingSvc();

            // Ensure the question does not exist
            using (var db = new AESDbContext())
            {
                if (db.Questions.Any(q => q.Text == QUESTION))
                {
                    db.Questions.Remove(db.Questions.FirstOrDefault(q => q.Text == QUESTION));
                    db.SaveChanges();
                }
            }

            // Add the new question
            var newQuestion = jobSvc.AddQuestion(new QAContract()
            {
                MC_Answers = ANSWERS,
                NeededRight = 2,
                Options = OPTIONS,
                Question = QUESTION,
                Type = QuestionType.CHECKBOX
            });

            // Add the duplicate question
            var duplicateQuestion = jobSvc.AddQuestion(new QAContract()
            {
                MC_Answers = ANSWERS,
                NeededRight = 2,
                Options = OPTIONS,
                Question = QUESTION,
                Type = QuestionType.CHECKBOX
            });

            // Get the question from the DB
            JobQuestion dbQ;
            using (var db = new AESDbContext())
            {
                dbQ = db.Questions.FirstOrDefault(q => q.Text == QUESTION);
            }

            Assert.AreEqual(newQuestion, JobbingResponse.SUCCESS);
            Assert.AreEqual(duplicateQuestion, JobbingResponse.DUPLICATE);
            Assert.IsNotNull(dbQ);
            Assert.AreEqual(dbQ.CorrectAnswers, "14");
            Assert.AreEqual(dbQ.CorrectAnswerThreshold, 2);
            Assert.AreEqual(dbQ.Option1, OPTIONS[0]);
            Assert.AreEqual(dbQ.Option2, OPTIONS[1]);
            Assert.AreEqual(dbQ.Option3, OPTIONS[2]);
            Assert.AreEqual(dbQ.Option4, OPTIONS[3]);
            Assert.AreEqual(dbQ.Text, QUESTION);
            Assert.AreEqual(dbQ.Type, QuestionType.CHECKBOX);
        }

        [TestMethod]
        public void JobbingSvc_AddQuestionToJob()
        {
            // Ensure both the job and question exist
            JobbingSvc_AddValidAndDuplicateJob();
            JobbingSvc_AddValidAndDuplicateQuestion();

            // Get the job and question from the DB
            Job dbJob;
            JobQuestion dbQ;
            using (var db = new AESDbContext())
            {
                dbJob = db.Jobs.FirstOrDefault(j => j.Title == TITLE);
                dbQ = db.Questions.FirstOrDefault(q => q.Text == QUESTION);
            }

            // Get the service
            JobbingSvc jobSvc = new JobbingSvc();

            // Link it twice, so we can ensure that it is only added once
            var link1 = jobSvc.AddQuestionToJob(dbJob.JobID, dbQ.QuestionID);
            var link2 = jobSvc.AddQuestionToJob(dbJob.JobID, dbQ.QuestionID);

            // Get the Job from the DB
            using (var db = new AESDbContext())
            {
                dbJob = db.Jobs.FirstOrDefault(j => j.Title == TITLE);

                // We leave the below inside the using so dbJob does not get disposed.

                // Ensure that everything is as expected
                Assert.AreEqual(link1, JobbingResponse.SUCCESS);
                Assert.AreEqual(link2, JobbingResponse.SUCCESS);
                Assert.AreEqual(dbJob.Questions.Count, 1);
            }
        }

        [TestMethod]
        public void JobbingSvc_EditJob()
        {
            const string NEW_SHORT = "A shorty, shiny job";
            const string NEW_TITLE = "Shiny Job";

            // From seeded data
            const string DUP_SHORT = "Responsible for all sales activities, from lead generation through to close. The successful candidate will be able " +
                                     "to elevate company standards, achieve sales goals and meet clients expectations.";

            // Ensure the job is in the DB
            JobbingSvc_AddValidAndDuplicateJob();

            // Get job from DB, and ensure the one we will be changing to does not exist
            Job dbJob;
            using (var db = new AESDbContext())
            {
                dbJob = db.Jobs.FirstOrDefault(j => j.Title == TITLE);
                if (db.Jobs.Any(j => j.Title == NEW_TITLE))
                {
                    db.Jobs.Remove(db.Jobs.FirstOrDefault(j => j.Title == NEW_TITLE));
                    db.SaveChanges();
                }
            }

            JobbingSvc jobSvc = new JobbingSvc();

            var invalidJob = jobSvc.EditJob(new JobContract());
            var duplicateJobShortDesc = jobSvc.EditJob(new JobContract()
            {
                JobID = dbJob.JobID,
                LongDescription = "A different long",
                ShortDescription = DUP_SHORT,
                Title = "Different Title"
            });
            var duplicateJobTitle = jobSvc.EditJob(new JobContract()
            {
                JobID = dbJob.JobID,
                LongDescription = "A different long",
                ShortDescription = "A different short",
                Title = "Sales Associate" // From seeded data
            });
            var successfulChange = jobSvc.EditJob(new JobContract()
            {
                JobID = dbJob.JobID,
                LongDescription = dbJob.LongDescription,
                ShortDescription = NEW_SHORT,
                Title = NEW_TITLE
            });

            // Update the dbJob
            using (var db = new AESDbContext())
            {
                dbJob = db.Jobs.FirstOrDefault(j => j.JobID == dbJob.JobID);
            }

            // Ensure things are as we expect it
            Assert.AreEqual(invalidJob, JobbingResponse.INVALID);
            Assert.AreEqual(duplicateJobShortDesc, JobbingResponse.DUPLICATE);
            Assert.AreEqual(duplicateJobTitle, JobbingResponse.DUPLICATE);
            Assert.AreEqual(successfulChange, JobbingResponse.SUCCESS);
            Assert.AreEqual(dbJob.Title, NEW_TITLE);
            Assert.AreEqual(dbJob.ShortDescription, NEW_SHORT);
        }

        [TestMethod]
        public void JobbingSvc_EditQuestion()
        {
            const string NEW_QUESTION = "Are you an oompa loompa?";

            // Ensure the question is in the DB
            JobbingSvc_AddValidAndDuplicateQuestion();

            // Get the question, and esnure the new one is not in the DB
            JobQuestion dbQ;
            using (var db = new AESDbContext())
            {
                dbQ = db.Questions.FirstOrDefault(q => q.Text == QUESTION);
                if (db.Questions.Any(q => q.Text == NEW_QUESTION))
                {
                    db.Questions.Remove(db.Questions.FirstOrDefault(q => q.Text == NEW_QUESTION));
                    db.SaveChanges();
                }
            }

            JobbingSvc jobSvc = new JobbingSvc();

            var invalidQ = jobSvc.EditQuestion(new QAContract());
            var duplicateQ = jobSvc.EditQuestion(new QAContract()
            {
                MC_Answers = ANSWERS,
                NeededRight = dbQ.CorrectAnswerThreshold,
                Options = new List<string> { dbQ.Option1, dbQ.Option2, dbQ.Option3, dbQ.Option4 },
                Question = "Can you lift more than 50 pounds?", // From seeded data
                QuestionID = dbQ.QuestionID,
                Type = dbQ.Type
            });
            var successfulChange = jobSvc.EditQuestion(new QAContract()
            {
                MC_Answers = new List<bool> { true, false, true, false },
                NeededRight = dbQ.CorrectAnswerThreshold + 1,
                Options = new List<string> { dbQ.Option1 + "A", dbQ.Option2 + "B", dbQ.Option3 + "C", dbQ.Option4 + "D" },
                Question = NEW_QUESTION,
                QuestionID = dbQ.QuestionID,
                Type = dbQ.Type
            });

            // Get a new dbQ
            JobQuestion newDbQ;
            using (var db = new AESDbContext())
            {
                newDbQ = db.Questions.FirstOrDefault(q => q.QuestionID == dbQ.QuestionID);
            }

            Assert.AreEqual(invalidQ, JobbingResponse.INVALID);
            Assert.AreEqual(duplicateQ, JobbingResponse.DUPLICATE);
            Assert.AreEqual(successfulChange, JobbingResponse.SUCCESS);
            Assert.AreEqual(newDbQ.CorrectAnswers, "13");
            Assert.AreEqual(newDbQ.CorrectAnswerThreshold, dbQ.CorrectAnswerThreshold + 1);
            Assert.AreEqual(newDbQ.Option1, dbQ.Option1 + "A");
            Assert.AreEqual(newDbQ.Option2, dbQ.Option2 + "B");
            Assert.AreEqual(newDbQ.Option3, dbQ.Option3 + "C");
            Assert.AreEqual(newDbQ.Option4, dbQ.Option4 + "D");
            Assert.AreEqual(newDbQ.Text, NEW_QUESTION);
            Assert.AreEqual(newDbQ.Type, dbQ.Type);
        }

        [TestMethod]
        public void JobbingSvc_GetJobs()
        {
            // And a new job to the DB
            JobbingSvc_AddValidAndDuplicateJob();

            // Get the jobs via the service
            JobbingSvc jobSvc = new JobbingSvc();
            var jobs = jobSvc.GetJobs();

            // Connect to the DB
            using (var db = new AESDbContext())
            {
                // Get the jobs in the DB
                var dbJobs = db.Jobs.ToList();

                // Check that we got the same amount of jobs back
                Assert.AreEqual(jobs.Count(), dbJobs.Count());
                Assert.IsTrue(jobs.Count() > 2); // Because seeded + the one we add

                // Loop through what we got and ensure everything is the same
                for(int i = 0; i < jobs.Count(); ++i)
                {
                    Assert.AreEqual(jobs.ElementAt(i).JobID, dbJobs[i].JobID);
                    Assert.AreEqual(jobs.ElementAt(i).LongDescription, dbJobs[i].LongDescription);
                    Assert.AreEqual(jobs.ElementAt(i).ShortDescription, dbJobs[i].ShortDescription);
                    Assert.AreEqual(jobs.ElementAt(i).Title, dbJobs[i].Title);
                }
            }
        }

        [TestMethod]
        public void JobbingSvc_GetAllQuestions()
        {
            // And a new question to the DB
            JobbingSvc_AddValidAndDuplicateQuestion();

            // Get the questions via the service
            JobbingSvc jobSvc = new JobbingSvc();
            var questions = jobSvc.GetQuestions();

            // Connect to the DB
            using (var db = new AESDbContext())
            {
                // Get the questions in the DB
                var dbQs = db.Questions.ToList();

                // Check that we got the same amount of questions back
                Assert.AreEqual(questions.Count(), dbQs.Count());
                Assert.IsTrue(questions.Count() > 2); // Because seeded + the one we add

                // Loop through what we got and ensure everything is the same
                for (int i = 0; i < questions.Count(); ++i)
                {
                    Assert.AreEqual(questions.ElementAt(i).NeededRight, dbQs.ElementAt(i).CorrectAnswerThreshold);
                    Assert.IsTrue(questions.ElementAt(i).Options.SequenceEqual(new List<string> { dbQs[i].Option1, dbQs[i].Option2, dbQs[i].Option3, dbQs[i].Option4 }));
                    Assert.AreEqual(questions.ElementAt(i).Question, dbQs.ElementAt(i).Text);
                    Assert.AreEqual(questions.ElementAt(i).QuestionID, dbQs.ElementAt(i).QuestionID);
                    Assert.AreEqual(questions.ElementAt(i).Type, dbQs.ElementAt(i).Type);
                }
            }
        }

        [TestMethod]
        public void JobbingSvc_GetQuestionsForJob()
        {
            // Ensure the job has a question
            JobbingSvc_AddQuestionToJob();
            
            // Get a new instance of the server
            JobbingSvc jobSvc = new JobbingSvc();

            // Get the job that we added via the service
            var job = jobSvc.GetJobs().FirstOrDefault(j => j.Title == TITLE);

            // Get the questions via the service
            var questions = jobSvc.GetQuestions(job.JobID);

            // Connect to the DB
            using (var db = new AESDbContext())
            {
                // Get the questions in the db from the job
                var dbQs = db.Jobs.FirstOrDefault(j => j.JobID == job.JobID).Questions.ToList();

                // We should only have one in each list
                Assert.AreEqual(questions.Count(), 1);
                Assert.AreEqual(dbQs.Count(), 1);

                // Loop through what we got and ensure everything is the same between them
                for (int i = 0; i < questions.Count(); ++i)
                {
                    Assert.AreEqual(questions.ElementAt(i).NeededRight, dbQs[i].CorrectAnswerThreshold);
                    Assert.IsTrue(questions.ElementAt(i).Options.SequenceEqual(new List<string> { dbQs[i].Option1, dbQs[i].Option2, dbQs[i].Option3, dbQs[i].Option4 }));
                    Assert.AreEqual(questions.ElementAt(i).Question, dbQs.ElementAt(i).Text);
                    Assert.AreEqual(questions.ElementAt(i).QuestionID, dbQs.ElementAt(i).QuestionID);
                    Assert.AreEqual(questions.ElementAt(i).Type, dbQs.ElementAt(i).Type);
                }
            }
        }

        [TestMethod]
        public void JobbingSvc_RemoveQuestionsFromJob()
        {
            // Link a question so we can remove it (this also confirms that the job has a question)
            JobbingSvc_AddQuestionToJob();

            // Get the job and question we've added back
            JobbingSvc jobSvc = new JobbingSvc();
            var job = jobSvc.GetJobs().FirstOrDefault(j => j.Title == TITLE);
            var question = jobSvc.GetQuestions().FirstOrDefault(q => q.Question == QUESTION);

            // Try to remove it
            var remove = jobSvc.RemoveQuestionFromJob(job.JobID, question.QuestionID);
            Assert.AreEqual(remove, JobbingResponse.SUCCESS);

            // Connect to the DB
            using (var db = new AESDbContext())
            {
                // Get the questions for the job we added
                var questions = db.Jobs.FirstOrDefault(j => j.Title == TITLE).Questions;

                Assert.AreEqual(questions.Count, 0);
            }
        }
        /*
        [TestMethod]
        public void JobbingSvc_Sanity()
        {
            var s = new JobbingSvcTestClient.JobbingSvcClient();
            var excepted = false;

            using (var db = new AESDbContext())
            {

                var job = db.Jobs.FirstOrDefault();
                var question = db.Questions.FirstOrDefault();

                var jobContract = new JobContract()
                {
                    JobID = job.JobID,
                    ShortDescription = job.ShortDescription,
                    LongDescription = job.LongDescription,
                    Title = job.Title
                };

                var questionContract = new QAContract()
                {
                    Question = question.Text,
                    Type = question.Type,
                    NeededRight = question.CorrectAnswerThreshold,
                    QuestionID = question.QuestionID
                };

                var newJob = new JobContract()
                {
                    ShortDescription = "ShortDesc",
                    LongDescription = "LongDesc",
                    Title = "Title"
                };

                var newQuestion = new QAContract()
                {
                     Question = "Sup?",
                     Type = QuestionType.SHORT
                };

                try
                {
                    s.AddJob(newJob);
                    s.AddQuestion(newQuestion);
                    s.AddQuestionToJob(job.JobID, question.QuestionID);
                    s.EditJob(jobContract);
                    s.EditQuestion(questionContract);
                    s.RemoveQuestionFromJob(job.JobID, question.QuestionID);
                    s.GetQuestions(job.JobID);
                    s.GetJobs();
                }
                catch (Exception)
                {
                    excepted = true;
                }
            }

            s.Close();
            Assert.IsFalse(excepted);
        }
        */
    }
}
