using AES.Shared;
using AES.Shared.Contracts;
using System.ServiceModel;

namespace AES.ManagementSvc
{
    [ServiceContract]
    public interface IManagementSvc
    {
        [OperationContract]
        StoreContract[] GetStoreList();

        [OperationContract]
        EmployeeUserContract[] GetUserList();

        [OperationContract]
        bool UpdateStoreInfo(StoreContract store);

        [OperationContract]
        bool UpdateUserInfo(EmployeeUserContract user);

        [OperationContract]
        bool CreateNewStore(StoreContract store);

        [OperationContract]
        bool CreateNewUser(EmployeeUserContract user, string password);
    }
}
