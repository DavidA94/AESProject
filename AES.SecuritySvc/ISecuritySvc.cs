using AES.Shared.Contracts;
using System.ServiceModel;

namespace AES.SecuritySvc
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface ISecuritySvc
    {

        [OperationContract]
        ApplicantInfoContract ValidateUser(ApplicantInfoContract userInfo);

        [OperationContract]
        EmployeeUserContract ValidateEmployeeUser(EmployeeCredentialsContract credentials);

        [OperationContract]
        bool CreateEmployee(EmployeeUserContract employeeInfo, string password);

        [OperationContract]
        bool UpdateUserPassword(EmployeeCredentialsContract credentials, string newPassword);

        [OperationContract]
        bool ForgotPassword(EmployeeCredentialsContract user);
    }
    
}
