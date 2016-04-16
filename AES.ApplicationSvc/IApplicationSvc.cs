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
        List<ApplicantInfoContract> GetApplicantsAwaitingCalls(DateTime currentDateTime);

        [OperationContract]
        List<ApplicantInfoContract> GetApplicantsAwaitingInterview(int storeID);

        [OperationContract]
        ApplicationInfoContract GetApplication(ApplicantInfoContract user);

        [OperationContract]
        ApplicationInfoContract GetCallApplication(ApplicantInfoContract user);

        [OperationContract]
        ApplicationInfoContract GetInterviewApplication(UserInfoContract user);

        [OperationContract]
        AppSvcResponse SavePartialApplication(ApplicationInfoContract app);

        [OperationContract]
        bool SetApplicationStatus(ApplicationInfoContract app, AppStatus status);

        [OperationContract]
        bool CallApplicant(int applicantID);

        [OperationContract]
        bool ApplicantDidNotAnswer(int applicantID);

        [OperationContract]

        bool SavePhoneInterview(int applicantID, string notes, bool approved);

        [OperationContract]
        bool SubmitApplication(ApplicantInfoContract user);
        
    }
}
