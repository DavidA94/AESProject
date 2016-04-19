using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AES.JobbingSvc.Contracts
{
    [DataContract]
    public class JobContract
    {
        public JobContract()
        {
            JobID = -1;
        }

        /// <summary>
        /// The ID of this job
        /// </summary>
        [DataMember]
        public int JobID { get; set; }

        /// <summary>
        /// The title of this job
        /// </summary>
        [DataMember]
        [StringLength(128)]
        public string Title { get; set; }

        /// <summary>
        /// The short description for this job
        /// </summary>
        [DataMember]
        [StringLength(512)]
        public string ShortDescription { get; set; }

        /// <summary>
        /// The long description for this job
        /// </summary>
        [DataMember]
        [StringLength(4000)]
        public string LongDescription { get; set; }
    }
}
