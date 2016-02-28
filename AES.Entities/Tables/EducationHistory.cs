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
        [StringLength(64)]
        public string SchoolName { get; set; }

        /// <summary>
        /// Not required as the applicant may not know
        /// </summary>
        [StringLength(50)]
        public string SchoolAddress { get; set; }

        [Required]
        [StringLength(30)]
        public string SchoolCity { get; set; }

        /// <summary>
        /// Not required as the applicant may not know or it may not be relevant
        /// </summary>
        [StringLength(2)]
        public string SchoolState { get; set; }

        /// <summary>
        /// Not required as the applicant may not know or it may not be relevant
        /// </summary>
        [Range(0,99999)]
        public int SchoolZip { get; set; }

        [Required]
        [StringLength(32)]
        public string SchoolCountry { get; set; }

        [RegularExpression(@"\d{3}-\d{3}-\d{4}")]
        public string SchoolPhone { get; set; }

        [Required]
        public double YearsAddented { get; set; }

        /// <summary>
        /// Can be used to determine if the applicant has graduated or will graduate (it would be a future date)
        /// </summary>
        public DateTime GraduationDate { get; set; }

        [Required]
        [StringLength(64)]
        public string Major { get; set; }

        [Required]
        [StringLength(64)]
        public string Degree { get; set; }

    }
}
