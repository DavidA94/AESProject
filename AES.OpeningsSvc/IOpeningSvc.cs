using AES.OpeningsSvc.Contracts;
using AES.Shared;
using System.Collections.Generic;
using System.ServiceModel;

namespace AES.OpeningsSvc
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IOpeningSvc
    {
        [OperationContract]
        List<JobOpeningContract> GetApprovedOpenings(int StoreID);

        [OperationContract]
        bool RequestOpenings(int StoreID, JobOpeningContract opening, int number = 1);

        [OperationContract]
        List<JobOpeningContract> GetPendingOpenings(int StoreID);

        [OperationContract]
        List<JobOpeningContract> GetRejectedOpenings(int StoreID);

        [OperationContract]
        bool ApproveOpening(JobOpeningContract opening, string notes = "");

        [OperationContract]
        bool RejectOpening(JobOpeningContract opening, string notes = "");

    }



}
