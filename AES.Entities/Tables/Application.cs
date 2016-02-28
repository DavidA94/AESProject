using System;
using System.ComponentModel.DataAnnotations;

namespace AES.Entities.Tables
{
    public class Application
    {
        public Application()
        {
            Timestamp = new DateTime(1970, 1, 1);
        }

        [Key]
        [Required]
        public int ID { get; set; }

        /// <summary>
        /// Which job the application is for
        /// </summary>
        [Required]
        public Job Job { get; set; }

        /// <summary>
        /// The applicant this application is for
        /// </summary>
        [Required]
        public ApplicantUser Applicant { get; set; }

        /// <summary>
        /// The date this application was submitted
        /// </summary>
        public DateTime Timestamp { get; set; }

    }
}
