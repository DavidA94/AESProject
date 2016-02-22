using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AES.SecuritySvc.Contracts
{
    public class UserInfoContract
    {
        public string Nickname { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public int Zip { get; set; }

        public string Phone { get; set; }

        public decimal SalaryExpectation { get; set; }
    }
}
