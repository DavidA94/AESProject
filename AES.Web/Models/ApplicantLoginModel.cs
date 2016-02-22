using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AES.Web.Models
{
    public class ApplicantLoginModel
    {
        public ApplicantLoginModel()
        {
            FirstName = "Annonymous";
        }

        [StringLength(25)]
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(28)]
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"\d{3}-\d{2}-\d{4}", ErrorMessage = "The Social Security Number must be in the format xxx-xx-xxxx")]
        [Display(Name = "SSN", Description = "Social Security Number")]
        public string SSN { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DOB { get; set; }
    }
}
