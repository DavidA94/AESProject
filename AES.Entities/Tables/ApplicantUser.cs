using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AES.Entities.Tables
{
    public class ApplicantUser
    {

        public ApplicantUser()
        {
            References = new HashSet<Reference>();
            EducationHistory = new HashSet<EducationHistory>();
            EmploymentHistory = new HashSet<JobHistory>();
            Applications = new HashSet<Application>();
            DTCallStartTime = new DateTime(1970, 1, 1);
            DTCallEndTime = new DateTime(1970, 1, 1);
        }

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

        public virtual Availability Availability { get; set; }

        [NotMapped]
        public TimeSpan CallStartTime
        {
            get { return DateTimeHelper.ConvertDateTime(DTCallStartTime); }
            set { DTCallStartTime = DateTimeHelper.ConvertTimeSpan(value); }
        }

        [NotMapped]
        public TimeSpan CallEndTime
        {
            get { return DateTimeHelper.ConvertDateTime(DTCallEndTime); }
            set { DTCallEndTime = DateTimeHelper.ConvertTimeSpan(value); }
        }

        public DateTime DTCallStartTime { get; set; }

        public DateTime DTCallEndTime { get; set; }

        public virtual ICollection<Reference> References { get; set; }

        public virtual ICollection<EducationHistory> EducationHistory { get; set; }

        public virtual ICollection<JobHistory> EmploymentHistory { get; set; }

        public virtual ICollection<Application> Applications { get; set; }

    }
}
