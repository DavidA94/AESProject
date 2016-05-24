using AES.ApplicationSvc.Contracts;
using AES.Shared;
using AES.Shared.Contracts;
using System;
using System.Collections.Generic;
using System.ServiceModel;

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
        List<ApplicantInfoContract> GetApplicantsAwaiting(int storeID, AppStatus status);

        [OperationContract]
        ApplicationInfoContract GetApplication(int userID, AppStatus userAppStatus);

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

        bool SaveInterview(int applicantID, string notes, AppStatus status);

        [OperationContract]
        bool SetIntervieweeApplicantStatus(int applicantID, bool approved);

        [OperationContract]
        bool SubmitApplication(ApplicantInfoContract user);
        
    }
}
