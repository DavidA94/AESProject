using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AES.Shared.Contracts
{
    [DataContract]
    public class EmployeeUserContract
    {
        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public EmployeeRole Role { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public UserInfoContract UserInfo { get; set; }
    }
}
