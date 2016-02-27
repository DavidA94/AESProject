using AES.Entities.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AES.OpeningSvc.Contracts
{
    [DataContract]
    public class JobOpeningContract
    {
        public JobOpeningContract() { }

        public JobOpeningContract(JobOpening opening)
        {
            ID = opening.ID;
            shortDesc = opening.Job.descShort;
            longDesc = opening.Job.descLong;
        }

        [DataMember]
        public string shortDesc { get; set; }

        [DataMember]
        public string longDesc { get; set; }

        [DataMember]
        public int ID { get; set; }
    }
}
