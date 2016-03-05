using AES.ApplicationSvc.Contracts;
using AES.Shared;
using AES.Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace AES.ApplicationSvc
{
    [ServiceContract]
    public interface IApplicationSvc
    {
        [OperationContract]
        bool CancelApplication(ApplicationInfoContract app);

        [OperationContract]
        UserInfoContract GetApplicantsAwaitingCalls();

        [OperationContract]
        UserInfoContract GetApplicantsAwaitingInterview(int storeID);

        [OperationContract]
        ApplicationInfoContract GetApplication(UserInfoContract user);

        [OperationContract]
        ApplicationInfoContract GetCallApplication(UserInfoContract user);

        [OperationContract]
        ApplicationInfoContract GetInterviewApplication(UserInfoContract user);

        [OperationContract]
        bool PullApplicantFromCallQueue(UserInfoContract user);

        [OperationContract]
        bool SavePartialApplication(ApplicationInfoContract app);

        [OperationContract]
        bool SetApplicationStatus(ApplicationInfoContract app, AppStatus status);

        [OperationContract]
        bool SubmitApplication(ApplicationInfoContract app);
        
    }
}
