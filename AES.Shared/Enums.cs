using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AES.Shared
{
    /// <summary>
    /// Indicates where an application is in the process
    /// </summary>
    public enum AppStatus {
        /// <summary>
        /// When the application has not been submitted yet
        /// </summary>
        PARTIAL,

        /// <summary>
        /// When the automatic system has rejected the applicant
        /// </summary>
        AUTO_REJECT,

        /// <summary>
        /// When the application has been saved, but the applicant has not been called
        /// </summary>
        WAITING_CALL,

        /// <summary>
        /// Inidcates that the user is currently in a call
        /// </summary>
        IN_CALL,

        /// <summary>
        /// When the applicant has passed the phone screening, and is waiting for an interview
        /// </summary>
        WAITING_INTERVIEW,

        /// <summary>
        /// Indicates that an interview with this person has been completed
        /// </summary>
        INTERVIEW_COMPLETE,

        /// <summary>
        /// When the applicant was denied during the phone interview
        /// </summary>
        CALL_DENIED,

        /// <summary>
        /// When the applicant has been approved by the hiring manager
        /// </summary>
        APPROVED,

        /// <summary>
        /// When the applicant has been denied by the hiring manager
        /// </summary>
        DENIED
    }

    /// <summary>
    /// Indicates what type of question is being asked
    /// </summary>
    public enum QuestionType {
        /// <summary>
        /// Short answer (textbox)
        /// </summary>
        SHORT,

        /// <summary>
        /// Multiple choice, pick 1 (radio buttons)
        /// </summary>
        RADIO,

        /// <summary>
        /// Multiple choice, pick n (Checkboxes)
        /// </summary>
        CHECKBOX
    }

    /// <summary>
    /// The type of degree obtained from a school
    /// </summary>
    public enum DegreeType {
        [Display(Name = "No Degree")] NONE,
        [Display(Name = "High School Diploma")] HS_DIPLOMA,
        [Display(Name = "Associates")] AA,
        [Display(Name = "Batchelors")] BA,
        [Display(Name = "Masters")] MA,
        [Display(Name = "Doctorate")] PHD,
        [Display(Name = "Medical Doctor")] MD,
        [Display(Name = "Certificate")] CERTIFICATE
    };

    public enum AppSvcResponse { GOOD, BAD_USER, BAD_JOB, BAD_QUESTION, ERROR };

    public enum EmployeeRole
    {
        HiringManager,
        HqQStaffingExpert,
        HqHiringSpecialist,
        StoreManager
    }
}
