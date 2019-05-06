using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace SimpleServerOJ.Net
{
    /// <summary>
    /// Http参数
    /// </summary>
    class HttpArgs
    {
        protected Dictionary<string, string> getArgs;
        protected JObject postArgs_j;
        /// <summary>
        /// 创建HttpArgs消息对象
        /// </summary>
        /// <param name="get_args"></param>
        /// <param name="response"></param>
        public HttpArgs(Dictionary<string, string> get_args, JObject post_args)
        {
            getArgs = get_args;
            postArgs_j = post_args;
        }
        /// <summary>
        /// 获取指定Get参数的值
        /// </summary>
        /// <param name="key">参数的键</param>
        /// <returns></returns>
        public string GetArgValue(string key)
        {
            if (getArgs.ContainsKey(key))
            {
                return getArgs[key];
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 获取Post参数
        /// </summary>
        /// <returns></returns>
        public JObject GetPostValue()
        {
            if (postArgs_j == null) return new JObject();
            return postArgs_j;
        }
    }

    /// <summary>
    /// Http响应参数
    /// </summary>
    class HttpResponseArgs
    {
        public ResponseType ResponseType { get; set; }
        private JObject obj { get; set; }
        private string rawStr { get; set; }
        public HttpResponseArgs(JObject obj,ResponseType type)
        {
            this.obj = obj;
            ResponseType = type;
        }
        public HttpResponseArgs(string msg)
        {
            rawStr = msg;
            ResponseType = ResponseType.Raw;
        }
        public override string ToString()
        {
            if (ResponseType == ResponseType.Raw) 
            {
                return rawStr;
            }
            var jstr = obj.ToString(Newtonsoft.Json.Formatting.Indented);
            switch (ResponseType)
            {
                case ResponseType.Json:
                    return jstr;
                case ResponseType.Xml:
                    return Newtonsoft.Json.JsonConvert.DeserializeXmlNode(jstr).InnerXml;
            }
            return jstr;
            
        }
    }

    enum ResponseType
    {
        Json,
        Xml,
        Raw
    }

    enum HttpCode
    {
        Sucsess = 200,
        Accept = 203,
        Error = 400,
        NotFound = 404
    }
}
