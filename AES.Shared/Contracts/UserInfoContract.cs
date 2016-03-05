using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AES.Shared.Contracts
{
    [DataContract]
    public class UserInfoContract
    {
        [DataMember]
        public string Nickname { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string State { get; set; }

        [DataMember]
        public int Zip { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public decimal SalaryExpectation { get; set; }
    }
}
