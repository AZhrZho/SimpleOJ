using SimpleServerOJ.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServerOJ.Application
{
    class ApiReturn
    {
        public static HttpResponseArgs Return(ApiReturn data)
        {
            return new HttpResponseArgs(Newtonsoft.Json.Linq.JObject.FromObject(data), ResponseType.Json);
        }
        public HttpCode Code { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
    enum HttpCode
    {
        Success=200,
        Error=400
    }
}
