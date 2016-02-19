using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AES.Entities.Tables
{
    public class ApplicantUser
    {
        [Key]
        [Required]
        public int userID { get; set; }

        [StringLength(25)]
        [Required]
        public string FirstName { get; set; }

        [StringLength(28)]
        [Required]
        public string LastName { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [MaxLength(88)]
        public string SSN { get; set; }

        [Required]
        public DateTime DOB { get; set; }

        public virtual UserInfo UserInfo { get; set; }
    }
}
