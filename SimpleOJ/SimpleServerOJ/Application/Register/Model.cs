using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServerOJ.Application.Register
{
    class RegisterModel : BaseModel
    {
        protected override string TableName => "user";
        public bool Register(string sno,string pw)
        {
            var register = Select("sno", sno);
            if (register.Count != 0)
            {
                return false;
            }
            Insert(new Dictionary<string, object>()
            {
                {"sno",sno },
                {"pw",pw }
            });
            return true;
        }
    }
}
