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
        [Index(IsUnique = true)]
        [StringLength(100)]
        public string PasswordHash { get; set; }

        [Required]
        [StringLength(Encryption.saltLengthLimit)]
        public string Salt { get; set; }

        [Required]
        public EmployeeRole Role { get; set; }

        public virtual UserInfo UserInfo { get; set; }
    }
}
