﻿using System;
using System.ComponentModel.DataAnnotations;

namespace AES.Entities.Tables
{
    public class Application
    {
        [Key]
        [Required]
        public int ID { get; set; }

        // Which job the application is for
        [Required]
        public Job Job { get; set; }

        // The applicant this application is for
        [Required]
        public ApplicantUser User { get; set; }

        // The date this application was submitted
        [Required]
        public DateTime Timestamp { get; set; }

    }
}
