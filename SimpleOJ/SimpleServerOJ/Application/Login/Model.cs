using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServerOJ.Application.Login
{
    class LoginModel : BaseModel
    {
        protected override string TableName => "user";

        public string Session { get; set; }

        public string Login(string sno,string pw)
        {
            var login = Select(new Dictionary<string, object>
            {
                {"sno",sno },
                {"pw",pw }
            });
            if (login.Count == 0) return string.Empty;
            var session = System.Guid.NewGuid().ToString();
            Net.CacheManager.SetCache(session, sno, 300);
            return session;
        }
    }
}
