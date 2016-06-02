using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using AES.Shared;

namespace AES.Web.Models
{
    public class EmployeeLoginModel
    {
        [StringLength(50)]
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(100)]
        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [StringLength(100)]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }

        [StringLength(100, MinimumLength = 8)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [StringLength(100)]
        [Compare("NewPassword", ErrorMessage = "Password fields do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmNewPassword { get; set; }
    }
}