using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AES.Web.Models
{
    public class EmployeeLoginModel
    {
        public EmployeeLoginModel()
        {
            Email = "Something";
        }

        [StringLength(50)]
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(100)]
        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}