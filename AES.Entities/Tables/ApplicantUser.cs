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
        [StringLength(24, MinimumLength = 24)]
        public string SSN { get; set; }

        [Required]
        public DateTime DOB { get; set; }

        public virtual UserInfo UserInfo { get; set; }

        // Only one per applicant
        public virtual Availability Availability { get; set; }

        public DateTime CallStartTime { get; set; }
        public DateTime CallEndTime { get; set; }
    }
}
