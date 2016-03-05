using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AES.ApplicationSvc.Contracts
{
    public class AnswerContract
    {
        public int QuestionId { get; set; }
        public string ShortAnswer { get; set; }
        public bool MC_Answer1 { get; set; }
        public bool MC_Answer2 { get; set; }
        public bool MC_Answer3 { get; set; }
        public bool MC_Answer4 { get; set; }
    }
}
