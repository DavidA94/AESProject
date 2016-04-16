using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AES.Shared;

namespace AES.Entities.Tables
{
    public class EmployeeUser
    {
        [Key]
        [StringLength(50)]
        [Required]
        public string Email { get; set; }

        [Required]
        [MaxLength(200)]
        public virtual byte[] PasswordHash { get; set; }

        [Required]
        [MaxLength(Encryption.saltLengthLimit)]
        public virtual byte[] Salt { get; set; }

        [Required]
        public EmployeeRole Role { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        public virtual UserInfo UserInfo { get; set; }
    }
}
