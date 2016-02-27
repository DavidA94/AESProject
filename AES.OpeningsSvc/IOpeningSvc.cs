using AES.OpeningsSvc.Contracts;
using System.Collections.Generic;
using System.ServiceModel;

namespace AES.OpeningsSvc
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IOpeningSvc
    {

        [OperationContract]
        List<JobOpeningContract> GetOpenings(int StoreID);

    }



}
