using AES.Shared.Contracts;
using AES.Web.ApplicationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AES.Web.Models
{
    public class FullApplicationModel
    {
        public ProfileViewModel Profile { get; set; }
        public AvailabilityViewModel Availibility { get; set; }
        public List<WorkHistoryViewModel> WorkHistory { get; set; }
        public List<EducationViewModel> Education { get; set; }
        public List<ReferencesViewModel> References { get; set; }
        public List<QuestionnaireViewModel> Questionnaire { get; set; }

        public int ApplicantID { get; set; }
        public string Notes { get; set; }
    }
}