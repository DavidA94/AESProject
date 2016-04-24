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
        List<JobOpeningContract> GetAllOpenings(int StoreID);

        [OperationContract]
        List<JobOpeningContract> GetApprovedOpenings(int StoreID);

        [OperationContract]
        bool RequestOpening(int StoreID, JobOpeningContract opening);

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
