using System;
using System.ComponentModel.DataAnnotations;

namespace AES.Entities.Tables
{
    public class JobHistory
    {
        [Key]
        [Required]
        public int ID { get; set; }

        [Required]
        public ApplicantUser Applicant { get; set; }

        [Required]
        [StringLength(128)]
        public string EmployerName { get; set; }

        // Not required, since the applicant may not know
        [StringLength(128)]
        public string EmployerAddress { get; set; }

        [Required]
        [StringLength(128)]
        public string EmployerCity { get; set; }

        // Not required, since the applicant may not know, or it may be irrelevant
        [StringLength(64)]
        public string EmployerState { get; set; }

        // Not required, since the applicant may not know, or it may be irrelevant
        [StringLength(16)]
        public string EmployerZip { get; set; }

        [Required]
        [StringLength(128)]
        public string EmployerCountry { get; set; }

        // Not required, since the applicant may not know
        [StringLength(32)]
        public string EmployerPhone { get; set; }

        [Required]
        [StringLength(128)]
        public string SupervisorName { get; set; }

        // Not Required
        [StringLength(128)]
        public string StartingSalary { get; set; }

        // Not Required
        [StringLength(128)]
        public string EndingSalary { get; set; }

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
