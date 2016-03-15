using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AES.Shared.Contracts
{
    public class JobHistoryContract
    {
        public string EmployerName { get; set; }
        public string EmployerAddress { get; set; }
        public string EmployerCity { get; set; }
        public string EmployerState { get; set; }
        public int EmployerZip { get; set; }
        public string EmployerCountry { get; set; }
        public string EmployerPhone { get; set; }
        public string SupervisorName { get; set; }
        public decimal StartingSalary { get; set; }
        public decimal EndingSalary { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ReasonForLeaving { get; set; }
        public string Responsibilities { get; set; }
    }
}
