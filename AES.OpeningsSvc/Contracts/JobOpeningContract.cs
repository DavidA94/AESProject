﻿using AES.Entities.Tables;
using System.Runtime.Serialization;

namespace AES.OpeningsSvc.Contracts
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
