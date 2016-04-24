using AES.Entities.Tables;
using AES.Shared;
using System.Runtime.Serialization;

namespace AES.OpeningsSvc.Contracts
{
    [DataContract]
    public class JobOpeningContract
    {
        public JobOpeningContract() { }

        public JobOpeningContract(JobOpening opening)
        {
            ID = opening.Job.JobID;
            ShortDescription = opening.Job.ShortDescription;
            LongDescription = opening.Job.LongDescription;
            title = opening.Job.Title;
            StoreManagerNotes = opening.StoreManagerNotes;
            Status = opening.Status;
        }

        [DataMember]
        public string ShortDescription { get; set; }

        [DataMember]
        public string LongDescription { get; set; }

        [DataMember]
        public string title { get; set; }

        [DataMember]
        public string StoreManagerNotes { get; set; }

        [DataMember]
        public OpeningStatus Status { get; set; }

        [DataMember]
        public int ID { get; set; }
    }
}
