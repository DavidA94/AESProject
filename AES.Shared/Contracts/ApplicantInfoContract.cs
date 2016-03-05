﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AES.Shared.Contracts
{
    [DataContract]
    public class ApplicantInfoContract
    {
        public ApplicantInfoContract() { }

        public ApplicantInfoContract(string first, string last, string ssn, DateTime dob, DateTime startCall, DateTime endCall)
        {
            if(dob.Year < 1970)
            {
                throw new ArgumentException("DOB Year cannot be before 1970.");
            }

            FirstName = first;
            LastName = last;
            SSN = ssn;
            DOB = dob;
            StartCallTime = startCall;
            EndCallTime = endCall;
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
        public DateTime StartCallTime { get; set; }


        [DataMember]
        public DateTime EndCallTime { get; set; }

        [DataMember]
        public UserInfoContract UserInfo { get; set; }
        public AvailabilityContract Availability { get; set; }
        public List<EducationHistory> Education { get; set; }
        public List<ReferenceContract> References { get; set; }
        public List<JobHistoryContract> PastJobs { get; set; }
    }
}