using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AES.Shared.Contracts
{
    [DataContract]
    public class EmployeeUserContract
    {
        [DataMember]
        public int Email { get; set; }

        [DataMember]
        public EmployeeRole Role { get; set; }

        [DataMember]
        public UserInfoContract UserInfo { get; set; }
    }
}
