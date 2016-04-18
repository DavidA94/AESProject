using AES.Shared.Contracts;
using AES.ApplicationSvc.Contracts;
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
        public ApplicationInfoContract()
        {
            AppliedJobs = new List<int>();
            Educations = new List<EducationHistoryContract>();
            Jobs = new List<JobHistoryContract>();
            QA = new List<QAContract>();
            References = new List<ReferenceContract>();
        }

        /// <summary>
        /// [Out] The first name of the person who owns this application
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// [Out] The last name of the person who owns this application
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// [Out] The DOB of the person who owns this application
        /// </summary>
        public DateTime DOB { get; set; }

        /// <summary>
        /// [In/Out] The user's availability
        /// </summary>
        [DataMember]
        public AvailabilityContract Availability { get; set; }
        
        /// <summary>
        /// [In] The ID of the user who is submitting this application
        /// </summary>
        [DataMember]
        public int ApplicantID { get; set; }

        /// <summary>
        /// [In] A list of Job IDs indicating which jobs a user is applying for
        /// </summary>
        [DataMember]
        public List<int> AppliedJobs { get; set; }

        /// <summary>
        /// [In/Out] The user's education history
        /// </summary>
        [DataMember]
        public List<EducationHistoryContract> Educations { get; set; }

        /// <summary>
        /// [In/Out] The user's job history
        /// </summary>
        [DataMember]
        public List<JobHistoryContract> Jobs { get; set; }

        /// <summary>
        /// [In/Out] The list of Questions/Answers
        /// </summary>
        [DataMember]
        public List<QAContract> QA { get; set; }

        /// <summary>
        /// [In/Out] The user's references
        /// </summary>
        [DataMember]
        public List<ReferenceContract> References { get; set; }

        /// <summary>
        /// [In/Out] The user's information
        /// </summary>
        [DataMember]
        public UserInfoContract UserInfo { get; set; }

        /// <summary>
        /// [In/Out] The screening notes
        /// </summary>
        [DataMember]
        public string ScreeningNotes { get; set; }

        /// <summary>
        /// [In/Out] The interview notes
        /// </summary>
        [DataMember]
        public string InterviewNotes { get; set; }

    }
}
