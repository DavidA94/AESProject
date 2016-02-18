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

        [RegularExpression(@"\d{3}-\d{2}-\d{4}")]
        [Required]
        [Index(IsUnique = true)]
        public string SSN { get; set; }

        [Required]
        public DateTime DOB { get; set; }

        public virtual UserInfo UserInfo { get; set; }
    }
}
