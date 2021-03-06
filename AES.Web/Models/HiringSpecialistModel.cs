﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AES.Web.Models
{
    public class HiringSpecialistModel
    {
        [StringLength(25)]
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(25)]
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "ETA")]
        [DataType(DataType.Time)]
        public TimeSpan ETA { get; set; }

        [Display(Name = "Applicant ID")]
        public int ApplicantID { get; set; }

        public int ReturnedID { get; set; }
    }
}