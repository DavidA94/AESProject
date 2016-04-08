﻿using System;
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
        public virtual ApplicantUser Applicant { get; set; }

        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"\d{3}-\d{3}-\d{4}")]
        public string Phone { get; set; }

        [StringLength(64)]
        public string Company { get; set; }

        [StringLength(64)]
        public string Title { get; set; }
    }
}
