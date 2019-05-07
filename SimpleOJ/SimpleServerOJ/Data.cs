using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleServerOJ.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServerOJ
{
    class Data
    {
        public static DirectoryInfo AnswerPath = new DirectoryInfo("exam");
        public static List<ProblemInfo> Problems { get; set; }
        public static string GetSno(string session)
        {
            var sno = CacheManager.GetCache(session);
            if (sno == null) return string.Empty;
            return sno.ToString();
        }

        public static List<ProblemInfo> GetProblems()
        {
            var list = new List<ProblemInfo>();
            foreach (var directory in AnswerPath.GetDirectories())
            {
                using (var fs = new FileStream(directory.GetFiles("config.json").First().FullName, FileMode.Open))
                {
                    var sr = new StreamReader(fs);
                    var js = sr.ReadToEnd();
                    JObject jo = JObject.Parse(js);
                    list.Add(new ProblemInfo
                    {
                        ID = Convert.ToInt32(directory.Name),
                        Name = jo["Name"].ToString(),
                        TimeLimit = Convert.ToInt32(jo["TimeLimit"]),
                        Discription = jo["Discription"].ToString(),
                        SampleInput = jo["SampleInput"].ToString(),
                        SampleOutput = jo["SampleOutput"].ToString()
                    });
                    sr.Close();
                }
            }
            return list;
        }

        public static void AddProblem(string _in,string _out, ProblemInfo info)
        {
            var index = GetProblemCount() + 1;
            var directory = new DirectoryInfo(AnswerPath.FullName + @"\" + index);
            var file_in = new FileInfo(directory.FullName + @"\in.dat");
            var file_out = new FileInfo(directory.FullName + @"\out.dat");
            var file_conf = new FileInfo(directory.FullName + @"\config.json");
            JObject jo = JObject.FromObject(info);
            var json = jo.ToString();
            directory.Create();
            using(var fs=new FileStream(file_in.FullName, FileMode.Create))
            {
                var sw = new StreamWriter(fs);
                sw.WriteLine(_in);
                sw.Flush();
                sw.Close();
            }
            using (var fs = new FileStream(file_out.FullName, FileMode.Create))
            {
                var sw = new StreamWriter(fs);
                sw.WriteLine(_out);
                sw.Flush();
                sw.Close();
            }
            using (var fs = new FileStream(file_conf.FullName, FileMode.Create))
            {
                var sw = new StreamWriter(fs);
                sw.WriteLine(json);
                sw.Flush();
                sw.Close();
            }
        }

        private static int GetProblemCount()
        {
            return AnswerPath.GetDirectories().Count();
        }
    }



    public class ProblemInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int TimeLimit { get; set; }
        public string Discription { get; set; }
        public string SampleInput { get; set; }
        public string SampleOutput { get; set; }
        public override string ToString()
        {
            return ID + ":" + Name;
        }
    }
}
