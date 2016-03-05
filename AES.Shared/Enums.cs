using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// When the applicant has passed the phone screening, and is waiting for an interview
        /// </summary>
        WAITING_INTERVIEW,

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
}
