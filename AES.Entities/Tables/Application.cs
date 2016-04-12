using AES.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AES.Entities.Tables
{
    public class Application
    {
        public Application()
        {
            Timestamp = new DateTime(1970, 1, 1);
            MultiAnswers = new List<ApplicationMultiAnswer>();
            ShortAnswers = new List<ApplicationShortAnswer>();
        }

        [Key]
        [Required]
        public int ID { get; set; }

        /// <summary>
        /// Which job the application is for
        /// </summary>
        [ForeignKey("JobID")]
        public virtual Job Job { get; set; }

        /// <summary>
        /// Which job the application is for
        /// </summary>
        [Required]
        public int JobID { get; set; }

        /// <summary>
        /// The applicant this application is for
        /// </summary>
        [Required]
        public virtual ApplicantUser Applicant { get; set; }

        /// <summary>
        /// The date this application was submitted
        /// </summary>
        public DateTime Timestamp { get; set; }

        public virtual ICollection<ApplicationMultiAnswer> MultiAnswers { get; set; }
        public virtual ICollection<ApplicationShortAnswer> ShortAnswers { get; set; }

        /// <summary>
        /// Where the application is at in the process
        /// </summary>
        public AppStatus Status { get; set; }

        [StringLength(4000)]
        public string ScreeningNotes { get; set; }

        [StringLength(4000)]
        public string InterviewNotes { get; set; }
    }
}
