using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace SimpleServerOJ.Net
{
    class HttpServer
    {
        private HttpListener httpListener;
        /// <summary>
        /// Http请求传递的参数
        /// </summary>
        /// <param name="args"></param>
        /// <returns>回应参数,无需回应时应返回null</returns>
        public delegate HttpResponseArgs HttpGetRequestDelegate(HttpArgs args);
        public event HttpGetRequestDelegate HttpGotRequest;
        /// <summary>
        /// 使用指定端口构造Http服务器
        /// </summary>
        /// <param name="port">监听端口</param>
        public HttpServer(int port)
        {
            httpListener = new HttpListener();
            httpListener.Prefixes.Add(string.Format("http://+:{0}/", port));
        }
        /// <summary>
        /// 启动Http服务器
        /// </summary>
        public void Start()
        {
            httpListener.Start();
            httpListener.BeginGetContext(new AsyncCallback(GetContext), httpListener);
        }
        private void GetContext(IAsyncResult ar)
        {

            HttpListener httpListener = ar.AsyncState as HttpListener;
            HttpListenerContext context = httpListener.EndGetContext(ar);
            httpListener.BeginGetContext(new AsyncCallback(GetContext), httpListener);
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            Dictionary<string, string> getArgs = new Dictionary<string, string>();
            JObject postArgs = null;
            string[] keys = request.QueryString.AllKeys;
            if (keys.Count() > 0)
            {
                foreach (var key in keys)
                {
                    getArgs.Add(key, request.QueryString[key]);
                }
            }
            if (request.HttpMethod == "POST")
            {
                using (Stream stream = request.InputStream)
                {
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    string body = reader.ReadToEnd();
                    string jsonText = "{}";
                    if (body.IndexOf("<xml>") == 0)
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(body);
                        jsonText = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.None, true);
                    }
                    if (body.IndexOf("{") == 0)
                    {
                        jsonText = body;
                    }
                    postArgs = JObject.Parse(jsonText);
                }
            }
            var callbackValue = HttpGotRequest?.Invoke(new HttpArgs(getArgs, postArgs));
            #region 消息回应
            if (callbackValue == null) return;
            response.ContentType = "json";
            response.ContentEncoding = Encoding.UTF8;
            using (Stream output = response.OutputStream)
            {
                byte[] buffer = Encoding.UTF8.GetBytes(callbackValue.ToString());
                output.Write(buffer, 0, buffer.Length);
            }
            #endregion


        }
    }
}
