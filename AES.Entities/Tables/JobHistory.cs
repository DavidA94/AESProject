using System;
using System.ComponentModel.DataAnnotations;

namespace AES.Entities.Tables
{
    public class JobHistory
    {
        [Key]
        [Required]
        public int ID { get; set; }

        /// <summary>
        /// The Applicant this history is for
        /// </summary>
        [Required]
        public ApplicantUser Applicant { get; set; }

        [Required]
        [StringLength(128)]
        public string EmployerName { get; set; }

        // Not required, since the applicant may not know
        [StringLength(50)]
        public string EmployerAddress { get; set; }

        [Required]
        [StringLength(30)]
        public string EmployerCity { get; set; }

        /// <summary>
        /// Not required, since the applicant may not know, or it may be irrelevant
        /// </summary>
        [StringLength(2)]
        public string EmployerState { get; set; }

        /// <summary>
        /// Not required as the applicant may not know or it may not be relevant
        /// </summary>
        [Range(0, 99999)]
        public int EmployerZip { get; set; }

        [Required]
        [StringLength(32)]
        public string EmployerCountry { get; set; }

        [RegularExpression(@"\d{3}-\d{3}-\d{4}")]
        public string EmployerPhone { get; set; }

        [Required]
        [StringLength(128)]
        public string SupervisorName { get; set; }

        /// <summary>
        /// Not Required
        /// </summary>
        [StringLength(128)]
        public decimal StartingSalary { get; set; }

        /// <summary>
        /// Not Required
        /// </summary>
        [StringLength(128)]
        public decimal EndingSalary { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(512)]
        public string ReasonForLeaving { get; set; }

        [Required]
        [StringLength(512)]
        public string Responsibilities { get; set; }

    }
}
