using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using AES.JobbingSvc.Contracts;
using AES.Shared.Contracts;
using AES.Entities.Contexts;
using AES.Entities.Tables;
using AES.Shared;

namespace AES.JobbingSvc
{
    /// <summary>
    /// The Jobbing Service: Responsible for managing all jobs and job questions
    /// </summary>
    public class JobbingSvc : IJobbingSvc
    {
        public JobbingSvc()
        {
            DBFileManager.SetDataDirectory();
        }

        /// See Interface for method descriptions

        public JobbingResponse AddJob(JobContract job)
        {
            // Ensure we were passed a vald job
            if (!isValidJob(job))
            {
                return JobbingResponse.INVALID;
            }

            // Connect to the DB
            using(var db = new AESDbContext())
            {
                // Get any job that has the same title or short description
                var duplicateJob = db.Jobs.FirstOrDefault(j => j.Title.ToLower() == job.Title.ToLower() || 
                                                               j.ShortDescription.ToLower() == job.ShortDescription.ToLower());

                // If it isn't null, then we have a duplicate
                if(duplicateJob != null)
                {
                    return JobbingResponse.DUPLICATE;
                }

                // Add the new job
                db.Jobs.Add(new Job()
                {
                    LongDescription = job.LongDescription,
                    ShortDescription = job.ShortDescription,
                    Title = job.Title
                });

                // Save the changes, and if they save, return true
                if(db.SaveChanges() != 0)
                {
                    return JobbingResponse.SUCCESS;
                }
            }

            // If we make it to here, something didn't work
            return JobbingResponse.ERROR;
        }

        public JobbingResponse AddQuestion(QAContract question)
        {
            // Ensure this is a valid question
            if (!isValidQuestion(question))
            {
                return JobbingResponse.INVALID;
            }

            // Connect to the DB
            using (var db = new AESDbContext()) {

                // Get any question that matches this one
                var duplicate = db.Questions.FirstOrDefault(q => q.Text.ToLower() == question.Question.ToLower());

                // If we got a duplicate, return false
                if(duplicate != null)
                {
                    return JobbingResponse.DUPLICATE;
                }

                string rightAnswers = "";
                if (question.MC_Answers != null)
                {
                    rightAnswers += question.MC_Answers.ElementAtOrDefault(0) == true ? "1" : "";
                    rightAnswers += question.MC_Answers.ElementAtOrDefault(1) == true ? "2" : "";
                    rightAnswers += question.MC_Answers.ElementAtOrDefault(2) == true ? "3" : "";
                    rightAnswers += question.MC_Answers.ElementAtOrDefault(3) == true ? "4" : "";
                }
                if(question.Options == null)
                {
                    question.Options = new List<string>();
                }


                // Add the new question
                db.Questions.Add(new JobQuestion()
                {
                    CorrectAnswers = rightAnswers,
                    CorrectAnswerThreshold = question.NeededRight,
                    Option1 = question.Options.ElementAtOrDefault(0),
                    Option2 = question.Options.ElementAtOrDefault(1),
                    Option3 = question.Options.ElementAtOrDefault(2),
                    Option4 = question.Options.ElementAtOrDefault(3),
                    Text = question.Question,
                    Type = question.Type
                });

                // Save the changes, and reutrn true if it works
                if(db.SaveChanges() != 0)
                {
                    return JobbingResponse.SUCCESS;
                }
            }

            // If we make it this far, return false
            return JobbingResponse.ERROR;
        }

        public JobbingResponse AddQuestionToJob(int jobID, int questionID)
        {
            /// We only allow a job/question to be added if it's Title/QuestionText is unique. 
            /// Thus, we can safely search by Title/QuestionText when looking in the DB.

            // Ensure we have a valid job
            if (jobID < 0 || questionID < 0)
            {
                return JobbingResponse.INVALID;
            }

            // Open the DB
            using (var db = new AESDbContext())
            {
                var dbJob = db.Jobs.FirstOrDefault(j => j.JobID == jobID);

                // If the job doesn't exist, return INVALID
                if (dbJob == null)
                {
                    return JobbingResponse.INVALID;
                }

                // If the question is alerady in the list, do nothing
                if (dbJob.Questions.FirstOrDefault(q => q.QuestionID == questionID) != null)
                {
                    return JobbingResponse.SUCCESS;
                }
                
                // Try to get the question from the DB
                var dbQ = db.Questions.FirstOrDefault(q => q.QuestionID == questionID);

                // If it doesn't exist, return INVALID
                if(dbQ == null)
                {
                    return JobbingResponse.INVALID;
                }

                // Otherwise add the question to the job
                dbJob.Questions.Add(dbQ);

                // Save the changes, and return SUCCESS.
                // We don't check the value of SaveChanges because it could be zero if no new questions were added to the job
                try
                {
                    db.SaveChanges();
                    return JobbingResponse.SUCCESS;
                }
                catch { }
            }

            // If we make it this far, return ERROR
            return JobbingResponse.ERROR;
        }

        public JobbingResponse EditJob(JobContract job)
        {
            // If the job isn't valid, return false
            if (!isValidJob(job))
            {
                return JobbingResponse.INVALID;
            }

            // Open a connection to the DB
            using (var db = new AESDbContext())
            {
                // Get the job that we're trying to edit
                var dbJob = db.Jobs.FirstOrDefault(j => j.JobID == job.JobID);

                // If we don't have one, then return invalid
                if (dbJob == null)
                {
                    return JobbingResponse.INVALID;
                }

                // Check that we won't be making a duplicate
                var duplicateJob = db.Jobs.FirstOrDefault(j => j.JobID != job.JobID &&
                                                              (j.Title.ToLower() == job.Title.ToLower() ||
                                                               j.ShortDescription.ToLower() == job.ShortDescription.ToLower()));

                if(duplicateJob != null)
                {
                    return JobbingResponse.DUPLICATE;
                }

                // Update the job data
                dbJob.LongDescription = job.LongDescription;
                dbJob.ShortDescription = job.ShortDescription;
                dbJob.Title = job.Title;

                // Save the DB, and return success if there are no issues
                try {
                    db.SaveChanges();
                    return JobbingResponse.SUCCESS;
                }
                catch
                {
                    // If something goes wrong with saving, return an error.
                    return JobbingResponse.ERROR;
                }
            }
        }

        public JobbingResponse EditQuestion(QAContract question)
        {
            // Ensure the question is valid
            if (!isValidQuestion(question))
            {
                return JobbingResponse.INVALID;
            }

            // Connect to the DB
            using (var db = new AESDbContext())
            {

                // Get the question from the DB
                var dbQ = db.Questions.FirstOrDefault(q => q.QuestionID == question.QuestionID);

                // If it doesn't exist, return invalid
                if(dbQ == null)
                {
                    return JobbingResponse.INVALID;
                }

                // Check to see if we'll be making a duplicate with the change
                var duplicate = db.Questions.FirstOrDefault(q => q.QuestionID != question.QuestionID && q.Text.ToLower() == question.Question.ToLower());

                // If it does, then return duplciate
                if (duplicate != null)
                {
                    return JobbingResponse.DUPLICATE;
                }

                // Get the correct answers string
                string rightAnswers = "";
                rightAnswers += question.MC_Answers.ElementAtOrDefault(0) == true ? "1" : "";
                rightAnswers += question.MC_Answers.ElementAtOrDefault(1) == true ? "2" : "";
                rightAnswers += question.MC_Answers.ElementAtOrDefault(2) == true ? "3" : "";
                rightAnswers += question.MC_Answers.ElementAtOrDefault(3) == true ? "4" : "";

                // Update the data
                dbQ.CorrectAnswers = rightAnswers;
                dbQ.CorrectAnswerThreshold = question.NeededRight;
                dbQ.Option1 = question.Options.ElementAtOrDefault(0);
                dbQ.Option2 = question.Options.ElementAtOrDefault(1);
                dbQ.Option3 = question.Options.ElementAtOrDefault(2);
                dbQ.Option4 = question.Options.ElementAtOrDefault(3);
                dbQ.Text = question.Question;
                dbQ.Type = question.Type;

                // Save the data, and if it goes, return success
                if(db.SaveChanges() != 0)
                {
                    return JobbingResponse.SUCCESS;
                }
            }

            // If we make it this far, return error
            return JobbingResponse.ERROR;
        }

        public IEnumerable<JobContract> GetJobs()
        {
            // Open a connection to the DB
            using (var db = new AESDbContext())
            {
                // Holds what we will return
                var retVal = new List<JobContract>();

                // Get all the jobs
                var jobs = db.Jobs;

                // Loop through the jobs, convert them to contracts, and add them to the return list
                foreach(var job in jobs)
                {
                    retVal.Add(ConvertTableToContract(job));
                }

                // Return what we got
                return retVal;
            }
        }

        public IEnumerable<QAContract> GetQuestions(int jobID = -1)
        {
            // Connect to the DB
            using (var db = new AESDbContext())
            {
                // Holds what we will return
                var retVal = new List<QAContract>();

                // Holds what we get from the dB
                IEnumerable<JobQuestion> questions;

                // Return all questions if it's null
                if (jobID < 0 )
                {
                    questions = db.Questions;
                }
                // Otherwise just what matches the ID in job
                else
                {
                    questions = db.Questions.Where(q => q.Jobs.FirstOrDefault(j => j.JobID == jobID) != null);
                }

                // Loop through what we got, convert it, and add it to the return list
                foreach(var q in questions)
                {
                    retVal.Add(ConvertTableToContract(q));
                }

                // Return what we got
                return retVal;
            }
        }

        public JobbingResponse RemoveQuestionFromJob(int jobID, int questionID)
        {
            // Ensure we have a valid job
            if (jobID < 0 || questionID < 0)
            {
                return JobbingResponse.INVALID;
            }

            // Open the DB
            using (var db = new AESDbContext())
            {
                // Try to get the job that is having questions removed from
                var dbJob = db.Jobs.FirstOrDefault(j => j.JobID == jobID);

                // If we didn't get the job, then return INVALID
                if(dbJob == null)
                {
                    return JobbingResponse.INVALID;
                }

                // If the question exists
                if (dbJob.Questions.Any(q => q.QuestionID == questionID))
                {
                    // Remove it
                    dbJob.Questions.Remove(dbJob.Questions.First(q => q.QuestionID == questionID));
                }

                // SaveChanges may not remove anything if the questions are not in the job, so just save and return SUCCESS
                try
                {
                    db.SaveChanges();
                    return JobbingResponse.SUCCESS;
                }
                catch { }
            }

            // If we make it this far, return ERROR
            return JobbingResponse.ERROR;
        }

        #region Helper Methods

        /// <summary>
        /// Checks if a Job is valid
        /// </summary>
        /// <param name="job">The Job to check</param>
        /// <returns>The if the Job is valid</returns>
        private bool isValidJob(JobContract job)
        {
            return !(job == null ||                                     // Can't be null
                    string.IsNullOrWhiteSpace(job.Title) ||             // Must have a title
                    string.IsNullOrWhiteSpace(job.ShortDescription) ||  // Must have a short description
                    string.IsNullOrWhiteSpace(job.LongDescription));    // Must have a long description
        }

        /// <summary>
        /// Checks if a question is valid
        /// </summary>
        /// <param name="q">The question to check</param>
        /// <returns>True if the question is valid</returns>
        private bool isValidQuestion(QAContract q)  
        {
            // Check for errors, and NOT it so it returns true if no errors were found
            return !(q == null ||                               // Can't be null
                     string.IsNullOrWhiteSpace(q.Question) ||   // Needs to have a question
                     (q.Type != QuestionType.SHORT &&           // If it's a multiple choice question
                     (q.Options.Count < 2 ||                    // Then ensure we have at least two options
                      q.Options.Count != q.MC_Answers.Count ||  // And the number of answers matches the number of options
                      q.NeededRight <= 0)));                    // And that we have to have at least one right

        }

        /// <summary>
        /// Converts a Job to a JobContract
        /// </summary>
        /// <param name="job">The Job to convert</param>
        /// <returns>A JobContract with the contents of [job] in it</returns>
        private JobContract ConvertTableToContract(Job job)
        {
            return new JobContract()
            {
                JobID = job.JobID,
                LongDescription = job.LongDescription,
                ShortDescription = job.ShortDescription,
                Title = job.Title
            };
        }

        /// <summary>
        /// Converts a JobQuestino to a QAContract
        /// </summary>
        /// <param name="question">The JobQuestion to convert</param>
        /// <returns>A QAContract with the contents of [question]</returns>
        private QAContract ConvertTableToContract(JobQuestion question)
        {
            return new QAContract()
            {
                MC_Answers = new List<bool> {
                    question.CorrectAnswers != null && question.CorrectAnswers.Contains("1"),
                    question.CorrectAnswers != null && question.CorrectAnswers.Contains("2"),
                    question.CorrectAnswers != null && question.CorrectAnswers.Contains("3"),
                    question.CorrectAnswers != null && question.CorrectAnswers.Contains("4")
                },
                NeededRight = question.CorrectAnswerThreshold,
                Options = new List<string> { question.Option1, question.Option2, question.Option3, question.Option4 },
                Question = question.Text,
                QuestionID = question.QuestionID,
                Type = question.Type
            };
        }

        #endregion
    }
}
