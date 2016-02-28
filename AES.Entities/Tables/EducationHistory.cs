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

        [StringLength(128)]
        public string SchoolAddress { get; set; }

        [StringLength(128)]
        public string SchoolCity { get; set; }

        [StringLength(64)]
        public string SchoolState { get; set; }

        [StringLength(16)]
        public string SchoolZip { get; set; }

        [Required]
        [StringLength(128)]
        public string SchoolCountry { get; set; }

        [StringLength(32)]
        public string SchoolPhone { get; set; }

        [Required]
        public double YearsAddented { get; set; }

        public DateTime GraduationDate { get; set; }

        [Required]
        [StringLength(128)]
        public string Major { get; set; }

        [StringLength(128)]
        public string Degree { get; set; }

    }
}
