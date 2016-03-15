using AES.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AES.ApplicationSvc.Contracts
{
    /// <summary>
    /// Contract used for each Question/Answer from the user for the Questionaire section
    /// </summary>
    [DataContract]
    public class QAContract
    {
        /// <summary>
        /// Creates a new QA Contract
        /// </summary>
        public QAContract()
        {
            Options = new List<string>();
            MC_Answers = new List<bool>();
        }

        /// <summary>
        /// [In/Out] The ID of the question
        /// </summary>
        [DataMember]
        public int QuestionID { get; set; }

        /// <summary>
        /// [In] The answers given to the multiple choice questions, in order
        /// </summary>
        [DataMember]
        public List<bool> MC_Answers { get; set; }

        /// <summary>
        /// [Out] The options to be presented to the user for the given question, in order
        /// </summary>
        [DataMember]
        public List<string> Options { get; set; }

        /// <summary>
        /// [In/Out] Indicates what type of question this is / what the user should be presented with in order to answer
        /// </summary>
        [DataMember]
        public QuestionType Type { get; set; }

        /// <summary>
        /// [Out] The question to be presented
        /// </summary>
        [DataMember]
        public string Question { get; set; }

        /// <summary>
        /// [In] The short answer given to a short answer question
        /// </summary>
        [DataMember]
        public string ShortAnswer { get; set; }
    }
}
