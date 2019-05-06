using SimpleServerOJ.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleServerOJ.Application
{
    class Application
    {
        static Dictionary<string, Type> appTypes = new Dictionary<string, Type>();
        static string appKey = "app";
        public static bool LoadApp(Type appType)
        {
            if (!typeof(IApplication).IsAssignableFrom(appType)) return false;
            var construct = appType.GetConstructor(new Type[0]);
            var app = (IApplication)construct.Invoke(null);
            if (appTypes.ContainsKey(app.Name)) return false;
            appTypes.Add(app.Name, appType);
            return true;
        }
        public static bool UnloadApp(string appName)
        {
            if (appTypes.ContainsKey(appName))
                appTypes.Remove(appName);
            else
                return false;
            return true;
        }
        public static void SetAppKeyString(string keystr = "app")
        {
            appKey = keystr;
        }
        public static HttpResponseArgs Handle(HttpArgs args)
        {
            if (!appTypes.ContainsKey(args.GetArgValue(appKey))) return null;
            var construct = appTypes[args.GetArgValue(appKey)].GetConstructor(new Type[0]);
            var app = (IApplication)construct.Invoke(null);
            return app.Handle(args);
        }
    }

    interface IApplication
    {
        string Name { get; }
        HttpResponseArgs Handle(HttpArgs args);
    }
}
