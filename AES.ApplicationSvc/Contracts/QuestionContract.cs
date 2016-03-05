using AES.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AES.ApplicationSvc.Contracts
{
    public class QuestionContract
    {
        public QuestionContract()
        {
            Options = new List<string>();
        }

        public int QuestionID { get; set; }

        public string QuestionTest { get; set; }
        public QuestionType Type { get; set; }
        public List<string> Options { get; set; }
    }
}
