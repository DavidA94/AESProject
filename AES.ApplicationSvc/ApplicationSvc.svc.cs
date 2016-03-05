using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using AES.ApplicationSvc.Contracts;
using AES.Shared;
using AES.Shared.Contracts;

namespace AES.ApplicationSvc
{
    public class ApplicationSvc : IApplicationSvc
    {
        public bool CancelApplication(ApplicationInfoContract app)
        {
            throw new NotImplementedException();
        }

        public UserInfoContract GetApplicantsAwaitingCalls()
        {
            throw new NotImplementedException();
        }

        public UserInfoContract GetApplicantsAwaitingInterview(int storeID)
        {
            throw new NotImplementedException();
        }

        public ApplicationInfoContract GetApplication(UserInfoContract user)
        {
            throw new NotImplementedException();
        }

        public ApplicationInfoContract GetCallApplication(UserInfoContract user)
        {
            throw new NotImplementedException();
        }

        public ApplicationInfoContract GetInterviewApplication(UserInfoContract user)
        {
            throw new NotImplementedException();
        }

        public bool PullApplicantFromCallQueue(UserInfoContract user)
        {
            throw new NotImplementedException();
        }

        public bool SavePartialApplication(ApplicationInfoContract app)
        {
            

            return false;
        }

        public bool SetApplicationStatus(ApplicationInfoContract app, AppStatus status)
        {
            throw new NotImplementedException();
        }

        public bool SubmitApplication(ApplicationInfoContract app)
        {
            throw new NotImplementedException();
        }
    }
}
