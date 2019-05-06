using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServerOJ.Application.Judge
{
    class JudgeModel : BaseModel
    {
        protected override string TableName => "judge";
        public int Time { get; set; }
        public bool Pass { get; set; }
        public JudgeResult Result { get; set; }
    }
    enum JudgeResult
    {
        AC,
        WA
    }
}
