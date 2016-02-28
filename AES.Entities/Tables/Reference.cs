using System;
using System.ComponentModel.DataAnnotations;

namespace AES.Entities.Tables
{
    public class Reference
    {
        [Key]
        [Required]
        public int ID { get; set; }

        // The applicant who this reference is for
        [Required]
        public ApplicantUser Applicant { get; set; }

        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        [Required]
        [StringLength(32)]
        public string Phone { get; set; }

        [Required]
        [StringLength(128)]
        public string Company { get; set; }

        [Required]
        [StringLength(128)]
        public string Title { get; set; }
    }
}
