using AES.SecuritySvc.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace AES.SecuritySvc
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface ISecurity
    {

        [OperationContract]
        UserInfoContract ValidateUser(UserInfoContract userInfo);

        [OperationContract]
        bool Logout(UserInfoContract userInfo);
    }
    
}
