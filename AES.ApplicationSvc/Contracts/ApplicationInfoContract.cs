using AES.Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AES.ApplicationSvc.Contracts
{
    [DataContract]
    public class ApplicationInfoContract
    {
        public ApplicantInfoContract Applicant { get; set; }
        public List<int> AppliedJobs { get; set; }
        public List<QuestionContract> Questions { get; set; }
        public List<AnswerContract> Answers { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
