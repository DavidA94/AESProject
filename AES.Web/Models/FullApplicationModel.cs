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
        public WorkHistoryViewModel WorkHistory { get; set; }
        public EducationViewModel Education { get; set; }
        public ReferencesViewModel References { get; set; }
        public QuestionnaireViewModel Questionnaire { get; set; }
    }
}