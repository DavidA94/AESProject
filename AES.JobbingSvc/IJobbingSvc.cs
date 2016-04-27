using AES.JobbingSvc.Contracts;
using AES.Shared;
using AES.Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace AES.JobbingSvc
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IJobbingSvc
    {
        /// <summary>
        /// Adds a new job
        /// </summary>
        /// <param name="job">The job to add</param>
        /// <returns>See JobbingResponse summaries</returns>
        [OperationContract]
        JobbingResponse AddJob(JobContract job);

        /// <summary>
        /// Adds a new question.
        /// </summary>
        /// <param name="question">The question to add</param>
        /// <returns>See JobbingResponse summaries</returns>
        [OperationContract]
        JobbingResponse AddQuestion(QAContract question);

        /// <summary>
        /// Adds questions to a job
        /// </summary>
        /// <param name="job">The job to assign questions to</param>
        /// <param name="questions">The questions to assign to this job</param>
        /// <returns>See JobbingResponse summaries</returns>
        [OperationContract]
        JobbingResponse AddQuestionToJob(int jobID, int questionID);

        /// <summary>
        /// Edits a job who's ID matches the one passed in
        /// </summary>
        /// <param name="job">The new job information</param>
        /// <returns>See JobbingResponse summaries</returns>
        [OperationContract]
        JobbingResponse EditJob(JobContract job);

        /// <summary>
        /// Edit's a question who's ID matches the one passed in
        /// </summary>
        /// <param name="question">The new question information</param>
        /// <returns>See JobbingResponse summaries</returns>
        [OperationContract]
        JobbingResponse EditQuestion(QAContract question);

        /// <summary>
        /// Gets an enumerable of all jobs
        /// </summary>
        /// <returns>An IEnumerable of all jobs</returns>
        [OperationContract]
        IEnumerable<JobContract> GetJobs();

        /// <summary>
        /// Gets an enumerable of questions.
        /// </summary>
        /// <param name="job">[Optional] Pass this in to get questions relating to a specific job</param>
        /// <returns>An IEnumerable of Questions, or null if there aren't any</returns>
        [OperationContract]
        IEnumerable<QAContract> GetQuestions(int jobID = -1);

        /// <summary>
        /// Removes questions from a job
        /// </summary>
        /// <param name="job">The job who has the questions</param>
        /// <param name="questions">The questions to remove</param>
        /// <returns>See JobbingResponse Summaries</returns>
        [OperationContract]
        JobbingResponse RemoveQuestionFromJob(int jobID, int questionID);
    }
}