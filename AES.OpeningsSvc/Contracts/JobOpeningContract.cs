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
            JobID = opening.Job.JobID;
            ShortDescription = opening.Job.ShortDescription;
            LongDescription = opening.Job.LongDescription;
            title = opening.Job.Title;
            StoreManagerNotes = opening.StoreManagerNotes;
            RequestNotes = opening.RequestNotes;
            OpeningID = opening.ID;
            Positions = opening.Positions;
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
        public string RequestNotes { get; set; }

        [DataMember]
        public int JobID { get; set; }

        [DataMember]
        public int Positions { get; set; }

        [DataMember]
        public int OpeningID { get; set; }
    }
}
