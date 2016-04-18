using AES.Entities.Tables;
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
        }

        [DataMember]
        public string ShortDescription { get; set; }

        [DataMember]
        public string LongDescription { get; set; }

        [DataMember]
        public string title { get; set; }

        [DataMember]
        public int ID { get; set; }
    }
}
