using AES.Entities.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AES.SecuritySvc.Contracts
{
    [DataContract]
    public class ApplicantInfoContract
    {
        public ApplicantInfoContract() { }

        public ApplicantInfoContract(string first, string last, string ssn, DateTime dob)
        {
            FirstName = first;
            LastName = last;
            SSN = ssn;
            DOB = dob;
        }

        public ApplicantInfoContract(ApplicantUser user)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            UserID = user.userID;
            DOB = user.DOB;
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

        public UserInfoContract UserInfo { get; set; }
    }
}
