using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AES.Web.Models
{
    public interface ApplicationViewModel
    {

    }
    public class ProfileViewModel : ApplicationViewModel
    {
        [StringLength(25)]
        [Required]
        [Display(Name = "Fist Name")]
        public string FirstName { get; set; }

        [StringLength(25)]
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "SSN", Description = "Social Security Number")]
        public string SSN { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        public DateTime DOB { get; set; }

        public AvailabilityViewModel AvailabilityViewModel { get; set; }
        public WorkHistoryViewModel WorkHistoryViewModel { get; set; }
        public EducationViewModel EducationViewModel { get; set; }
        public ReferencesViewModel ReferencesViewModel { get; set; }
        public QuestionnaireViewModel QuestionnaireViewModel { get; set; }

    }

    public class AvailabilityViewModel : ApplicationViewModel
    {
        [Required]
        [Display(Name = "Monday")]
        public bool MondayCheckBox { get; set; }

        [Display(Name = "Monday From")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh/mm/ss}")]
        public TimeSpan MondayStartTime { get; set; }

        [Display(Name = "Monday To")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh/mm/ss}")]
        public TimeSpan MondayEndTime { get; set; }

        [Required]
        [Display(Name = "Tuesday")]
        public bool TuesdayCheckBox { get; set; }

        [Display(Name = "Tuesday From")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh/mm/ss}")]
        public TimeSpan TuesdayStartTime { get; set; }

        [Display(Name = "Tuesday To")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh/mm/ss}")]
        public TimeSpan TuesdayEndTime { get; set; }

        [Required]
        [Display(Name = "Wednesday")]
        public bool WednesdayCheckBox { get; set; }

        [Display(Name = "Wednesday From")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh/mm/ss}")]
        public TimeSpan WednesdayStartTime { get; set; }

        [Display(Name = "Wednesday To")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh/mm/ss}")]
        public TimeSpan WednesdayEndTime { get; set; }

        [Required]
        [Display(Name = "Thursday")]
        public bool ThursdayCheckBox { get; set; }

        [Display(Name = "Thursday From")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh/mm/ss}")]
        public TimeSpan ThursdayStartTime { get; set; }

        [Display(Name = "Thursday To")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh/mm/ss}")]
        public TimeSpan ThursdayEndTime { get; set; }

        [Required]
        [Display(Name = "Friday")]
        public bool FridayCheckBox { get; set; }

        [Display(Name = "Friday From")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh/mm/ss}")]
        public TimeSpan FridayStartTime { get; set; }

        [Display(Name = "Friday To")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh/mm/ss}")]
        public TimeSpan FridayEndTime { get; set; }

        [Required]
        [Display(Name = "Saturday")]
        public bool SaturdayCheckBox { get; set; }

        [Display(Name = "Saturday From")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh/mm/ss}")]
        public TimeSpan SaturdayStartTime { get; set; }

        [Display(Name = "Saturday To")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh/mm/ss}")]
        public TimeSpan SaturdayEndTime { get; set; }

        [Required]
        [Display(Name = "Sunday")]
        public bool SundayCheckBox { get; set; }

        [Display(Name = "Sunday From")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh/mm/ss}")]
        public TimeSpan SundayStartTime { get; set; }

        [Display(Name = "Sunday To")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh/mm/ss}")]
        public TimeSpan SundayEndTime { get; set; }
    }

    public class WorkHistoryViewModel : ApplicationViewModel
    {
        [StringLength(25)]
        [Required]
        [Display(Name = "Employer")]
        public string Employer { get; set; }

        [StringLength(50)]
        [Required]
        [Display(Name = "Street")]
        public string Street { get; set; }

        [StringLength(25)]
        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [StringLength(25)]
        [Required]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required]
        [Display(Name = "Zip")]
        public int Zip { get; set; }

        [StringLength(50)]
        [Required]
        [Display(Name = "Position")]
        public string Position { get; set; }

        [StringLength(200)]
        [Display(Name = "Reason For Leaving")]
        public string ReasonForLeaving { get; set; }

        [StringLength(200)]
        [Display(Name = "Responsibilities")]
        public string Responsibilities { get; set; }

        [Required]
        [Display(Name = "Worked From")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime WorkedFrom { get; set; }

        [Required]
        [Display(Name = "Worked To")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime WorkedTo { get; set; }

        [Required]
        [StringLength(13)]
        [Display(Name = "Phone", Description = "Phone Number")]
        public string Phone { get; set; }

        [StringLength(25)]
        [Required]
        [Display(Name = "Supervisor", Description = "Supervisor Name")]
        public string Supervisor { get; set; }

        [Display(Name = "Starting Salary")]
        public double StartingSalary { get; set; }

        [Display(Name = "Ending Salary")]
        public double EndingSalary { get; set; }

        [Required]
        [Display(Name = "May we contact your employer?")]
        public bool ContactEmployerCheckBox { get; set; }
    
    }

    public class EducationViewModel : ApplicationViewModel
    {
        [StringLength(50)]
        [Required]
        [Display(Name = "Institution Name")]
        public string InstitutionName { get; set; }

        [StringLength(50)]
        [Required]
        [Display(Name = "Street")]
        public string Street { get; set; }

        [StringLength(25)]
        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [StringLength(25)]
        [Required]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required]
        [Display(Name = "Zip")]
        public int Zip { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [StringLength(50)]
        [Required]
        [Display(Name = "Degree")]
        public string Degree { get; set; }

        [StringLength(50)]
        [Required]
        [Display(Name = "Major")]
        public string Major { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Graduation Date")]
        public DateTime GraduationDate { get; set; }
    }

    public class ReferencesViewModel : ApplicationViewModel
    {
        [StringLength(50)]
        [Required]
        [Display(Name = "Name", Description = "Name of the person reference")]
        public string ReferenceName { get; set; }

        [StringLength(13)]
        [Required]
        [Display(Name = "Phone", Description = "Phone Number")]
        public string Phone { get; set; }

        [StringLength(50)]
        [Required]
        [Display(Name = "Company", Description = "Company Name")]
        public string Company { get; set; }

        [StringLength(50)]
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }
    }

    public class QuestionnaireViewModel : ApplicationViewModel
    {
        [StringLength(50)]
        [Required]
        [Display(Name = "Question")]
        public string Question { get; set; }

        [StringLength(50)]
        [Required]
        [Display(Name = "Option 1")]
        public string Option1 { get; set; }

        [Required]
        [Display(Name = "Answer 1")]
        public bool Answer1 { get; set; }

        [StringLength(50)]
        [Required]
        [Display(Name = "Option 2")]
        public string Option2 { get; set; }

        [Required]
        [Display(Name = "Answer 2")]
        public bool Answer2 { get; set; }

        [StringLength(50)]
        [Required]
        [Display(Name = "Option 3")]
        public string Option3 { get; set; }

        [Required]
        [Display(Name = "Answer 3")]
        public bool Answer3 { get; set; }

        [StringLength(50)]
        [Required]
        [Display(Name = "Option 4")]
        public string Option4 { get; set; }

        [Required]
        [Display(Name = "Answer 4")]
        public bool Answer4 { get; set; }

        [Display(Name = "ShortAnswer")]
        public string ShortAnswer { get; set; }
    }

}