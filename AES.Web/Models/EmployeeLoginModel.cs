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
        [StringLength(25)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(28)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [StringLength(50)]
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(100)]
        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Employee Role")]
        public EmployeeRole EmployeeRole { get; set; }
    }
}