﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AES.Shared.Contracts
{
    [DataContract]
    public class ApplicantInfoContract
    {
        public ApplicantInfoContract() { }

        public ApplicantInfoContract(string first, string last, string ssn, DateTime dob)
        {
            if(dob.Year < 1970)
            {
                throw new ArgumentException("DOB Year cannot be before 1970.");
            }

            FirstName = first;
            LastName = last;
            SSN = ssn;
            DOB = dob;
        }

        [DataMember]
        public int? UserID { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string SSN { get; set; }

        [DataMember]
        public DateTime DOB { get; set; }

        [DataMember]
        public UserInfoContract UserInfo { get; set; }

        [DataMember]
        public List<ApplicantInfoContract> Applications { get; set; }

        [DataMember]
        public AvailabilityContract Availability { get; set; }

        [DataMember]
        public List<EducationHistoryContract> Education { get; set; }

        [DataMember]
        public List<ReferenceContract> References { get; set; }

        [DataMember]
        public List<JobHistoryContract> PastJobs { get; set; }
    }
}
