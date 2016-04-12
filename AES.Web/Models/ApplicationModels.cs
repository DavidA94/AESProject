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

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }


        [Display(Name = "Nickname")]
        [StringLength(25)]
        public string Nickname { get; set; }

        [Required]
        [Display(Name = "Address")]
        [StringLength(50)]
        public string Address { get; set; }

        [Required]
        [Display(Name = "City")]
        [StringLength(30)]
        public string City { get; set; }

        [Required]
        [Display(Name = "State")]
        [StringLength(2)]
        public string State { get; set; }

        [Required]
        [Display(Name = "ZIP")]
        [Range(0, 99999)]
        public int? Zip { get; set; }

        [Required]
        [Display(Name = "Phone")]
        [RegularExpression(@"\d{3}-\d{3}-\d{4}", ErrorMessage = "Phone number must be in the format ###-###-####")]
        [DisplayFormat(DataFormatString = "{0:###-###-####", ApplyFormatInEditMode = true)]
        public string Phone { get; set; }

        [Display(Name = "Salary")]
        // [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
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

        [Required]
        [Display(Name = "Sunday From")]
        [DataType(DataType.Time)]
        public TimeSpan SundayStart { get; set; }

        [Required]
        [Display(Name = "To")]
        [DataType(DataType.Time)]
        public TimeSpan SundayEnd { get; set; }


        [Required]
        [Display(Name = "Monday From")]
        [DataType(DataType.Time)]
        public TimeSpan MondayStart { get; set; }

        [Required]
        [Display(Name = "To")]
        [DataType(DataType.Time)]
        public TimeSpan MondayEnd { get; set; }


        [Required]
        [Display(Name = "Tuesday From")]
        [DataType(DataType.Time)]
        public TimeSpan TuesdayStart { get; set; }

        [Required]
        [Display(Name = "To")]
        [DataType(DataType.Time)]
        public TimeSpan TuesdayEnd { get; set; }


        [Required]
        [Display(Name = "Wednesday From")]
        [DataType(DataType.Time)]
        public TimeSpan WednesdayStart { get; set; }

        [Required]
        [Display(Name = "To")]
        [DataType(DataType.Time)]
        public TimeSpan WednesdayEnd { get; set; }


        [Required]
        [Display(Name = "Thursday From")]
        [DataType(DataType.Time)]
        public TimeSpan ThursdayStart { get; set; }

        [Required]
        [Display(Name = "To")]
        [DataType(DataType.Time)]
        public TimeSpan ThursdayEnd { get; set; }


        [Required]
        [Display(Name = "Friday From")]
        [DataType(DataType.Time)]
        public TimeSpan FridayStart { get; set; }

        [Required]
        [Display(Name = "To")]
        [DataType(DataType.Time)]
        public TimeSpan FridayEnd { get; set; }


        [Required]
        [Display(Name = "Saturday From")]
        [DataType(DataType.Time)]
        public TimeSpan SaturdayStart { get; set; }

        [Required]
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

        [Required]
        [StringLength(128)]
        [Display(Name = "Employer")]
        public string EmployerName { get; set; }

        [Required]
        [StringLength(128)]
        [Display(Name = "Supervisor", Description = "Supervisor's Name")]
        public string SupervisorName { get; set; }

        [Display(Name = "Phone", Description = "Phone Number")]
        [RegularExpression(@"\d{3}-\d{3}-\d{4}", ErrorMessage = "Phone number must be in the format ###-###-####")]
        [DisplayFormat(DataFormatString = "{0:###-###-####", ApplyFormatInEditMode = true)]
        public string EmployerPhone { get; set; }

        [StringLength(50)]
        [Display(Name = "Address")]
        public string EmployerAddress { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "City")]
        public string EmployerCity { get; set; }

        [Required]
        [StringLength(2)]
        [Display(Name = "State")]
        public string EmployerState { get; set; }

        [Display(Name = "Zip Code")]
        [Range(0, 99999)]
        public int? EmployerZip { get; set; }

        [Required]
        [StringLength(32)]
        [Display(Name = "Employer Country")]
        public string EmployerCountry { get; set; }

        [Required]
        [StringLength(512)]
        [Display(Name = "Reason For Leaving")]
        public string ReasonForLeaving { get; set; }

        [Required]
        [StringLength(512)]
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

        [Display(Name = "Starting Salary")]
        //[DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Currency)]
        public decimal? StartingSalary { get; set; }

        [Display(Name = "Ending Salary")]
        //[DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
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
                    EmployerAddress = job.EmployerAddress ?? "",
                    EmployerCity = job.EmployerCity,
                    EmployerCountry = job.EmployerCountry,
                    EmployerName = job.EmployerName,
                    EmployerPhone = job.EmployerPhone,
                    EmployerState = job.EmployerState,
                    EmployerZip = job.EmployerZip != null ? (int)job.EmployerZip : 0,
                    EndDate = job.WorkedTo == null ? new DateTime(1970, 1, 1) : (DateTime)job.WorkedTo,
                    EndingSalary = Convert.ToDecimal(job.EndingSalary),
                    ReasonForLeaving = job.ReasonForLeaving,
                    Responsibilities = job.Responsibilities,
                    StartDate = job.WorkedFrom == null ? new DateTime(1970, 1, 1) : (DateTime)job.WorkedFrom,
                    StartingSalary = Convert.ToDecimal(job.StartingSalary),
                    SupervisorName = job.SupervisorName
                });
            }

            info.Jobs = jobs.ToArray();
        }
    }

    public class EducationViewModel : IApplicationViewModel
    {
        #region Data

        [StringLength(64)]
        [Required]
        [Display(Name = "Institution Name")]
        public string SchoolName { get; set; }

        [StringLength(50)]
        [Display(Name = "Street")]
        public string SchoolAddress { get; set; }

        [StringLength(30)]
        [Required]
        [Display(Name = "City")]
        public string SchoolCity { get; set; }

        [StringLength(2)]
        [Required]
        [Display(Name = "State")]
        public string SchoolState { get; set; }

        [Display(Name = "Zip Code")]
        public int? SchoolZip { get; set; }

        [Required]
        [StringLength(32)]
        public string SchoolCountry { get; set; }

        [Required]
        [Display(Name = "Years Attended")]
        public double? YearAttended { get; set; }
        
        [Required]
        [Display(Name = "Degree")]
        public DegreeType Degree { get; set; }

        [StringLength(64)]
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
                    SchoolAddress = ed.SchoolAddress ?? "",
                    SchoolCity = ed.SchoolCity,
                    SchoolCountry = ed.SchoolCountry,
                    SchoolName = ed.SchoolName,
                    SchoolState = ed.SchoolState,
                    SchoolZIP = ed.SchoolZip != null ? (int)ed.SchoolZip : 0,
                    YearsAttended = Convert.ToDouble(ed.YearAttended)
                });
            }

            info.Educations = eds.ToArray();
        }
    }

    public class ReferencesViewModel : IApplicationViewModel
    {
        #region Data

        [StringLength(64)]
        [Required]
        [Display(Name = "Name", Description = "Name of the person reference")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Phone", Description = "Phone Number")]
        [RegularExpression(@"\d{3}-\d{3}-\d{4}", ErrorMessage = "Phone number must be in the format ###-###-####")]
        public string Phone { get; set; }

        [StringLength(64)]
        [Display(Name = "Company", Description = "Company Name")]
        public string Company { get; set; }

        [StringLength(64)]
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
                    Company = r.Company ?? "",
                    Name = r.Name,
                    Phone = r.Phone,
                    Title = r.Title ?? ""
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