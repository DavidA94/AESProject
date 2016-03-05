using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AES.Shared.Contracts
{
    public class EducationHistory
    {
        public string SchoolName { get; set; }
        public string SchoolAddress { get; set; }
        public string SchoolCity { get; set; }
        public string SchoolState { get; set; }
        public int SchoolZIP { get; set; }
        public string SchoolCountry { get; set; }
        public double YearsAttended { get; set; }
        public DateTime Graduated { get; set; }
        public string Degree { get; set; }
    }
}
