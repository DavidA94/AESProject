using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AES.Entities.Tables
{
    public class Application
    {
        [Key]
        [Required]
        public int ID { get; set; }

        public enum AppStatus
        {
            PARTIAL,
            AUTO_REJECT,
            WAITING_CALL,
            WAITING_INTERVIEW,
            CALL_DENIED,
            APPROVED,
            DENIED
        }

        /// <summary>
        /// Which job the application is for
        /// </summary>
        [Required]
        public virtual Job Job { get; set; }

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

    }
}
