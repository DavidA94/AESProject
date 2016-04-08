using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using AES.Web.ApplicationService;
using AES.Shared.Contracts;
using AES.Shared;

namespace AES.Web.Models
{
    public interface IApplicationViewModel
    {
        void AddData(IApplicationViewModel data, ref ApplicationInfoContract info);
        void AddData(IEnumerable<IApplicationViewModel> data, ref ApplicationInfoContract info);
    }
    

    public class ProfileViewModel : IApplicationViewModel
    {
        #region Data

        [StringLength(25)]
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(25)]
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        [Display(Name = "Nickname")]
        [StringLength(25)]
        public string Nickname { get; set; }

        [Display(Name = "Address")]
        [StringLength(50)]
        public string Address { get; set; }

        [Display(Name = "City")]
        [StringLength(30)]
        public string City { get; set; }

        [Display(Name = "State")]
        [StringLength(2)]
        public string State { get; set; }

        [Display(Name = "ZIP")]
        [Range(0, 99999)]
        public int? Zip { get; set; }

        [Display(Name = "Phone")]
        [RegularExpression(@"\d{3}-\d{3}-\d{4}", ErrorMessage = "Phone number must be in the format ###-###-####")]
        [DisplayFormat(DataFormatString = "{0:###-###-####", ApplyFormatInEditMode = true)]
        public string Phone { get; set; }

        [Display(Name = "Salary")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? SalaryExpectation { get; set; }

        [Display(Name = "From")]
        [DataType(DataType.Time)]
        public TimeSpan StartCallTime { get; set; }

        [Display(Name = "To")]
        [DataType(DataType.Time)]
        public TimeSpan EndCallTime { get; set; }

        #endregion

        public void AddData(IApplicationViewModel data, ref ApplicationInfoContract info)
        {
            info.UserInfo = new UserInfoContract()
            {
                Address = Address,
                City = City,
                EndCallTime = EndCallTime,
                Nickname = Nickname ?? "",
                Phone = Phone,
                SalaryExpectation = Convert.ToDecimal(SalaryExpectation),
                StartCallTime = StartCallTime,
                State = State,
                Zip = (int)Zip
            };
        }

        public void AddData(IEnumerable<IApplicationViewModel> data, ref ApplicationInfoContract info)
        {
            if(data.Count() > 0)
            {
                data.First().AddData(data.First(), ref info);
            }
        }
    }

    public class AvailabilityViewModel : IApplicationViewModel
    {
        #region Data

        [Display(Name = "Sunday From")]
        [DataType(DataType.Time)]
        public TimeSpan SundayStart { get; set; }

        [Display(Name = "To")]
        [DataType(DataType.Time)]
        public TimeSpan SundayEnd { get; set; }


        [Display(Name = "Monday From")]
        [DataType(DataType.Time)]
        public TimeSpan MondayStart { get; set; }

        [Display(Name = "To")]
        [DataType(DataType.Time)]
        public TimeSpan MondayEnd { get; set; }

        
        [Display(Name = "Tuesday From")]
        [DataType(DataType.Time)]
        public TimeSpan TuesdayStart { get; set; }

        [Display(Name = "To")]
        [DataType(DataType.Time)]
        public TimeSpan TuesdayEnd { get; set; }


        [Display(Name = "Wednesday From")]
        [DataType(DataType.Time)]
        public TimeSpan WednesdayStart { get; set; }

        [Display(Name = "To")]
        [DataType(DataType.Time)]
        public TimeSpan WednesdayEnd { get; set; }


        [Display(Name = "Thursday From")]
        [DataType(DataType.Time)]
        public TimeSpan ThursdayStart { get; set; }

        [Display(Name = "To")]
        [DataType(DataType.Time)]
        public TimeSpan ThursdayEnd { get; set; }
        

        [Display(Name = "Friday From")]
        [DataType(DataType.Time)]
        public TimeSpan FridayStart { get; set; }

        [Display(Name = "To")]
        [DataType(DataType.Time)]
        public TimeSpan FridayEnd { get; set; }


        [Display(Name = "Saturday From")]
        [DataType(DataType.Time)]
        public TimeSpan SaturdayStart { get; set; }

        [Display(Name = "To")]
        [DataType(DataType.Time)]
        public TimeSpan SaturdayEnd { get; set; }

        #endregion

        public void AddData(IApplicationViewModel data, ref ApplicationInfoContract info)
        {
            info.Availability = new AvailabilityContract()
            {
                SundayStart = SundayStart,
                SundayEnd = SundayEnd,
                MondayStart = MondayStart,
                MondayEnd = MondayEnd,
                TuesdayStart = TuesdayStart,
                TuesdayEnd = TuesdayEnd,
                WednesdayStart = WednesdayStart,
                WednesdayEnd = WednesdayEnd,
                ThursdayStart = ThursdayStart,
                ThursdayEnd = ThursdayEnd,
                FridayStart = FridayStart,
                FridayEnd = FridayEnd,
                SaturdayStart = SaturdayStart,
                SaturdayEnd = SaturdayEnd
            };
        }

        public void AddData(IEnumerable<IApplicationViewModel> data, ref ApplicationInfoContract info)
        {
            if (data.Count() > 0)
            {
                data.First().AddData(data.First(), ref info);
            }
        }
    }

    public class WorkHistoryViewModel : IApplicationViewModel
    {
        #region Data

        [StringLength(25)]
        [Required]
        [Display(Name = "Employer")]
        public string Employer { get; set; }

        [StringLength(50)]
        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [StringLength(25)]
        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [StringLength(25)]
        [Required]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required]
        [Display(Name = "Zip Code")]
        public int? Zip { get; set; }

        [Required]
        [StringLength(32)]
        [Display(Name = "Employer Country")]
        public string EmployerCountry { get; set; }

        [StringLength(200)]
        [Display(Name = "Reason For Leaving")]
        public string ReasonForLeaving { get; set; }

        [StringLength(200)]
        [Display(Name = "Responsibilities")]
        public string Responsibilities { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? WorkedFrom { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? WorkedTo { get; set; }

        [Required]
        [Display(Name = "Phone", Description = "Phone Number")]
        [RegularExpression(@"\d{3}-\d{3}-\d{4}", ErrorMessage = "Phone number must be in the format ###-###-####")]
        [DisplayFormat(DataFormatString = "{0:###-###-####", ApplyFormatInEditMode = true)]
        public string Phone { get; set; }

        [StringLength(25)]
        [Required]
        [Display(Name = "Supervisor", Description = "Supervisor's Name")]
        public string Supervisor { get; set; }

        [Display(Name = "Starting Salary")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Currency)]
        public decimal? StartingSalary { get; set; }

        [Display(Name = "Ending Salary")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Currency)]
        public decimal? EndingSalary { get; set; }

        #endregion

        public void AddData(IApplicationViewModel data, ref ApplicationInfoContract info)
        {
            AddData(new List<IApplicationViewModel> { this }, ref info);
        }

        public void AddData(IEnumerable<IApplicationViewModel> data, ref ApplicationInfoContract info)
        {
            var jobs = new List<JobHistoryContract>();

            foreach(WorkHistoryViewModel job in data)
            {
                jobs.Add(new JobHistoryContract()
                {
                    EmployerAddress = job.Address,
                    EmployerCity = job.City,
                    EmployerCountry = job.EmployerCountry,
                    EmployerName = job.Employer,
                    EmployerPhone = job.Phone,
                    EmployerState = job.State,
                    EmployerZip = (int)job.Zip,
                    EndDate = job.WorkedTo == null ? new DateTime(1970, 1, 1) : (DateTime)job.WorkedTo,
                    EndingSalary = Convert.ToDecimal(job.EndingSalary),
                    ReasonForLeaving = job.ReasonForLeaving,
                    Responsibilities = job.Responsibilities,
                    StartDate = job.WorkedFrom == null ? new DateTime(1970, 1, 1) : (DateTime)job.WorkedFrom,
                    StartingSalary = Convert.ToDecimal(job.StartingSalary),
                    SupervisorName = job.Supervisor
                });
            }

            info.Jobs = jobs.ToArray();
        }
    }

    public class EducationViewModel : IApplicationViewModel
    {
        #region Data

        [StringLength(50)]
        [Required]
        [Display(Name = "Institution Name")]
        public string InstitutionName { get; set; }

        [StringLength(50)]
        [Required]
        [Display(Name = "Street")]
        public string Address { get; set; }

        [StringLength(25)]
        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [StringLength(25)]
        [Required]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required]
        [Display(Name = "Zip Code")]
        public int? Zip { get; set; }

        [Required]
        [StringLength(32)]
        public string Country { get; set; }

        [Display(Name = "Years Attended")]
        public double? YearAttended { get; set; }
        
        [Required]
        [Display(Name = "Degree")]
        public DegreeType Degree { get; set; }

        [StringLength(50)]
        [Required]
        [Display(Name = "Major")]
        public string Major { get; set; }

        [Display(Name = "Graduation Date")]
        [DataType(DataType.Date)]
        public DateTime? GraduationDate { get; set; }

        #endregion

        public void AddData(IApplicationViewModel data, ref ApplicationInfoContract info)
        {
            AddData(new List<IApplicationViewModel> { this }, ref info);
        }

        public void AddData(IEnumerable<IApplicationViewModel> data, ref ApplicationInfoContract info)
        {
            List<EducationHistoryContract> eds = new List<EducationHistoryContract>();

            foreach(EducationViewModel ed in data)
            {
                eds.Add(new EducationHistoryContract()
                {
                    Degree = ed.Degree,
                    Graduated = ed.GraduationDate != null ? (DateTime)ed.GraduationDate : new DateTime(1970, 1, 1),
                    Major = ed.Major,
                    SchoolAddress = ed.Address,
                    SchoolCity = ed.City,
                    SchoolCountry = ed.Country,
                    SchoolName = ed.InstitutionName,
                    SchoolState = ed.State,
                    SchoolZIP = (int)ed.Zip,
                    YearsAttended = Convert.ToDouble(ed.YearAttended)
                });
            }

            info.Educations = eds.ToArray();
        }
    }

    public class ReferencesViewModel : IApplicationViewModel
    {
        #region Data

        [StringLength(50)]
        [Required]
        [Display(Name = "Name", Description = "Name of the person reference")]
        public string ReferenceName { get; set; }

        [Required]
        [Display(Name = "Phone", Description = "Phone Number")]
        [RegularExpression(@"\d{3}-\d{3}-\d{4}", ErrorMessage = "Phone number must be in the format ###-###-####")]
        public string Phone { get; set; }

        [StringLength(50)]
        [Display(Name = "Company", Description = "Company Name")]
        public string Company { get; set; }

        [StringLength(50)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        #endregion

        public void AddData(IApplicationViewModel data, ref ApplicationInfoContract info)
        {
            AddData(new List<IApplicationViewModel> { this }, ref info);
        }

        public void AddData(IEnumerable<IApplicationViewModel> data, ref ApplicationInfoContract info)
        {
            List<ReferenceContract> refs = new List<ReferenceContract>();

            foreach(ReferencesViewModel r in data){
                refs.Add(new ReferenceContract()
                {
                    Company = r.Company == null ? "" : r.Company,
                    Name = r.ReferenceName,
                    Phone = r.Phone,
                    Title = r.Title
                });
            }

            info.References = refs.ToArray();
        }
    }

    public class QuestionnaireViewModel : IApplicationViewModel
    {
        #region Data

        public QuestionType Type { get; set; }

        public int QuestionID { get; set; }

        public string Question { get; set; }

        public string RadioOption { get; set; }

        public List<string> Options { get; set; }
        
        public List<bool> MC_Answers { get; set; }

        public string ShortAnswer { get; set; }

        #endregion

        public void AddData(IApplicationViewModel data, ref ApplicationInfoContract info)
        {
            AddData(new List<IApplicationViewModel> { this }, ref info);
        }

        public void AddData(IEnumerable<IApplicationViewModel> data, ref ApplicationInfoContract info)
        {
            List<QAContract> QAs = new List<QAContract>();

            foreach(QuestionnaireViewModel a in data)
            {
                QAs.Add(new QAContract()
                {
                    MC_Answers = a.MC_Answers == null ? new bool[] { false, false, false, false } : a.MC_Answers.ToArray(),
                    Options = a.Options == null ? new string[] { "", "", "", "" } : a.Options.ToArray(),
                    Question = a.Question,
                    QuestionID = a.QuestionID,
                    ShortAnswer = a.ShortAnswer ?? "",
                    Type = a.Type,
                });
            }

            info.QA = QAs.ToArray();
        }
    }

}