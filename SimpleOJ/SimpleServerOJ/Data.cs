using SimpleServerOJ.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServerOJ
{
    class Data
    {
        public static string GetSno(string session)
        {
            var sno = CacheManager.GetCache(session); 
            if (sno == null) return string.Empty;
            return sno.ToString();
        }
    }
}
