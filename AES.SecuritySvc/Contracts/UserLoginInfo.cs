using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AES.SecuritySvc.Contracts
{
    [DataContract]
    public class UserLoginInfo
    {
        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string SSN { get; set; }

        [DataMember]
        public DateTime DOB { get; set; }
    }
}
