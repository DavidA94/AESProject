using System;
using System.ComponentModel.DataAnnotations;

namespace AES.Entities.Tables
{
    public class EducationHistory
    {
        [Key]
        [Required]
        public int ID { get; set; }

        [Required]
        public ApplicantUser Applicant { get; set; }

        [Required]
        [StringLength(128)]
        public string SchoolName { get; set; }

        // Not required as the applicant may not know
        [StringLength(128)]
        public string SchoolAddress { get; set; }

        [Required]
        [StringLength(128)]
        public string SchoolCity { get; set; }

        // Not required as the applicant may not know or it may not be relevant
        [StringLength(64)]
        public string SchoolState { get; set; }

        // Not required as the applicant may not know or it may not be relevant
        [StringLength(16)]
        public string SchoolZip { get; set; }

        [Required]
        [StringLength(128)]
        public string SchoolCountry { get; set; }

        // Not required as the applicant may not know
        [StringLength(32)]
        public string SchoolPhone { get; set; }

        [Required]
        public double YearsAddented { get; set; }

        // Can be used to determine if the applicant has graduated or will graduate (it would be a future date)
        public DateTime GraduationDate { get; set; }

        [Required]
        [StringLength(128)]
        public string Major { get; set; }

        [Required]
        [StringLength(128)]
        public string Degree { get; set; }

    }
}
