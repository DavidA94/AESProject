using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AES.Web.Models
{
    public class JobOpeningsViewModel
    {
        [Required]
        [Display(Name = "Job Checkbox")]
        public bool JobCheckbox { get; set; }

        [StringLength(25)]
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [StringLength(512)]
        [Required]
        [Display(Name = "Short Description")]
        public string ShortDesc { get; set; }

        [StringLength(4000)]
        [Required]
        [Display(Name = "Long Description")]
        public string LongDesc { get; set; }
    }
}