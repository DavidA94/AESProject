using AES.Shared;
using AES.Shared.Contracts;
using AES.Web.Authorization;
using AES.Web.JobbingService;
using AES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AES.Web.Controllers
{
    [AESAuthorize(BadRedirectURL = "/EmployeeLogin", Role = EmployeeRole.HqQStaffingExpert)]
    public class StaffingController : Controller
    {
        /// <summary>
        /// Gets the main Dashboard view
        /// </summary>
        public ActionResult Dashboard()
        {
            return View();
        }

        /// <summary>
        /// Adds a question to a job
        /// </summary>
        /// <param name="jobID">The ID of the job to be added to</param>
        /// <param name="questionID">The ID of the question to be added</param>
        public string AddJobQuestionLink(int jobID, int questionID)
        {
            using (var jobSvc = new JobbingSvcClient())
            {
                return jobSvc.AddQuestionToJob(jobID, questionID).ToString();
            }
        }

        /// <summary>
        /// Gets a single job edit template
        /// </summary>
        public ActionResult GetJobTemplate()
        {
            return PartialView("_SingleJobEdit", new JobModel());
        }

        /// <summary>
        /// Gets all the questions in the database
        /// </summary>
        public ActionResult GetListOfQuestions()
        {
            using (var jobSvc = new JobbingSvcClient())
            {
                return PartialView("_QuestionsTextList", ConvertContractToModel(jobSvc.GetQuestions(-1)));
            }
        }

        /// <summary>
        /// Gets a single question edit template
        /// </summary>
        public ActionResult GetQuestionTemplate()
        {
            return PartialView("_SingleQuestionEdit", new QuestionnaireViewModel()
            {
                Options = new List<string> { "", "", "", "" },              // Ensure there are options
                MC_Answers = new List<bool> { false, false, false, false }  // and MC answers
            });
        }
        
        /// <summary>
        /// Gets the list of all jobs that are in the system
        /// </summary>
        public ActionResult JobList()
        {
            using (var jobSvc = new JobbingSvcClient())
            {
                var jobs = jobSvc.GetJobs();

                return PartialView("_JobListPartial", ConvertContractToModel(jobs));
            }
        }

        /// <summary>
        /// Gets the list of of questions for a given job
        /// </summary>
        /// <param name="jobID"></param>
        public ActionResult QuestionList(int jobID)
        {
            // Error if an invalid jobID
            if(jobID < 0)
            {
                return Content("Invaild data passed");
            }

            using (var jobSvc = new JobbingSvcClient())
            {
                var questions = jobSvc.GetQuestions(jobID);

                // Remember the jobID to be used in the partial list
                ViewBag.jobID = jobID;

                return PartialView("_QuestionListPartial", ConvertContractToModel(questions));
            }
        }

        /// <summary>
        /// Removes a question from the given job
        /// </summary>
        /// <param name="jobID">The ID of the job to have the question remove</param>
        /// <param name="questionID">The ID of the question to remove</param>
        public string RemoveJobQuestionLink(int jobID, int questionID)
        {
            using (var jobSvc = new JobbingSvcClient())
            {
                return jobSvc.RemoveQuestionFromJob(jobID, questionID).ToString();
            }
        }

        /// <summary>
        /// Creates/Updates a job in the sysetm
        /// </summary>
        /// <param name="job">The job to save/update</param>
        [HttpPost]
        public ActionResult SaveJob(JobModel job)
        {
            // If no job, error out
            if(job == null)
            {
                return Content("No job provided");
            }

            JobbingResponse response;
            using (var jobSvc = new JobbingSvcClient())
            {
                // If <0, then it must be a new job, otherwise, edit the job
                if (job.JobID < 0)
                {
                    response = jobSvc.AddJob(ConvertModelToContract(job));
                }
                else
                {
                    response = jobSvc.EditJob(ConvertModelToContract(job));
                }
            }

            // If we got a good response, respond with the udpated list of jobs, otherwise, return what went werong
            if(response == JobbingResponse.SUCCESS)
            {
                return RedirectToAction("JobList");
            }
            else if(response == JobbingResponse.INVALID)
            {
                return Content("Invalid information given");
            }
            else if(response == JobbingResponse.DUPLICATE)
            {
                return Content("This job already exists");
            }
            else if(response == JobbingResponse.ERROR)
            {
                return Content("An unknown error occurred");
            }

            return Content("Weird things can happen");
        }

        /// <summary>
        /// Creates/Updates a question in the system
        /// </summary>
        /// <param name="postQuestion">The question to be created/updated</param>
        [HttpPost]
        public ActionResult SaveQuestion(QuestionnaireViewModel postQuestion)
        {
            // If not question was given, or if radio was chosen, but no option was selected, return an error
            if (postQuestion == null)
            {
                return Content("No question provided");
            }
            else if (postQuestion.Type == QuestionType.RADIO && string.IsNullOrWhiteSpace(postQuestion.RadioOption))
            {
                return Content("Please choose which option is correct.");
            }

            // If it's a radio type, ensure the MC_Answers is set up based on the RadioOption
            if(postQuestion.Type == QuestionType.RADIO)
            {
                postQuestion.MC_Answers = new List<bool> { false, false, false, false };
                try {
                    postQuestion.MC_Answers[Convert.ToInt32(postQuestion.RadioOption.Last().ToString())] = true;
                }
                catch
                {
                    return Content("Stop trying to hack me!");
                }
            }


            JobbingResponse response;

            using (var jobSvc = new JobbingSvcClient())
            {
                // If <0, then it must be a new question, otherwise, update it
                if (postQuestion.QuestionID < 0)
                {
                    response = jobSvc.AddQuestion(ConvertModelToContract(postQuestion));
                }
                else
                {
                    response = jobSvc.EditQuestion(ConvertModelToContract(postQuestion));
                }

                // If we get a good response, then get the id of the question that was added and return it with "Success",
                // otherwise return what went wrong
                if (response == JobbingResponse.SUCCESS)
                {
                    int qID = jobSvc.GetQuestions(-1).FirstOrDefault(q => q.Question == postQuestion.Question).QuestionID;
                    return Content("Success:" + qID.ToString());
                }
                else if (response == JobbingResponse.INVALID)
                {
                    return Content("Invalid information given");
                }
                else if (response == JobbingResponse.DUPLICATE)
                {
                    return Content("This job already exists");
                }
                else if (response == JobbingResponse.ERROR)
                {
                    return Content("An unknown error occurred");
                }

                return Content("Weird things can happen");
            }
        }

        #region Converters

        #region Contract to Model

        private List<JobModel> ConvertContractToModel(IEnumerable<JobContract> jobs)
        {
            List<JobModel> retList = new List<JobModel>();

            foreach(var job in jobs)
            {
                retList.Add(new JobModel()
                {
                    JobID = job.JobID,
                    LongDescription = job.LongDescription,
                    ShortDescription = job.ShortDescription,
                    Title = job.Title
                });
            }

            return retList;
        }

        private List<QuestionnaireViewModel> ConvertContractToModel(IEnumerable<QAContract> questions)
        {
            var retList = new List<QuestionnaireViewModel>();

            foreach(var q in questions)
            {
                retList.Add(new QuestionnaireViewModel()
                {
                    MC_Answers = q.MC_Answers,
                    Options = q.Options,
                    Question = q.Question,
                    QuestionID = q.QuestionID,
                    RadioOption = "i" + q.QuestionID.ToString() + q.MC_Answers.ToList().IndexOf(true).ToString(),
                    Type = q.Type,
                });
            }

            return retList;
        }

        #endregion

        #region Model to Contract

        private JobContract ConvertModelToContract(JobModel job)
        {
            return new JobContract()
            {
                JobID = job.JobID,
                LongDescription = job.LongDescription,
                ShortDescription = job.ShortDescription,
                Title = job.Title
            };
        }

        private QAContract ConvertModelToContract(QuestionnaireViewModel q)
        {
            return new QAContract()
            {
                MC_Answers = q.MC_Answers,
                NeededRight = q.NeededCorrect,
                Options = q.Options,
                Question = q.Question,
                QuestionID = q.QuestionID,
                ShortAnswer = q.ShortAnswer,
                Type = q.Type
            };
        }

        #endregion

        #endregion
    }
}