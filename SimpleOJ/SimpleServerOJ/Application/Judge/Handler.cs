using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServerOJ.Application.Judge
{
    static class Handler
    {
        public static string CodePathRoot = "code";
        public static DirectoryInfo CompilerPath = new DirectoryInfo(@"MinGW\bin");
        public static DirectoryInfo ExecutePath = new DirectoryInfo("exe");
        public static DirectoryInfo AnswerPath = new DirectoryInfo("exam");
        public static FileInfo SaveAsFile(string code, string id, string sno, string fix)
        {
            var path = string.Format(@"{0}\{1}_{2}_{3}.{4}", CodePathRoot, id, sno, DateTime.Now.ToString("HHmmss"), fix);
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(code);
            sw.Flush();
            sw.Close();
            return new FileInfo(path);
        }
        public static FileInfo Compiler(FileInfo source, string fix)
        {
            switch (fix)
            {
                case "c":
                case "cpp":
                    Process process = new Process();
                    process.StartInfo = new ProcessStartInfo
                    {
                        Arguments = string.Format("{0} -o {1}", source.FullName, ExecutePath.FullName + @"\" + source.Name.Replace(source.Extension, string.Empty)),
                        FileName = CompilerPath.FullName + @"\g++.exe",
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Minimized,
                        UseShellExecute = true,
                        WorkingDirectory = CompilerPath.FullName
                    };
                    process.Start();
                    process.WaitForExit();
                    break;
                default:
                    throw new Exception("No such compiler.");
            }
            return new FileInfo(ExecutePath.FullName + @"\" + source.Name.Replace(source.Extension, string.Empty) + ".exe"); 
        }

        public static JudgeModel Judge(string id, FileInfo file)
        {
            var problem = ReadProblem(id);
            if (!file.Exists) return new JudgeModel()
            {
                Pass = false,
                Result = JudgeResult.WA,
                Time = 0
            };
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                FileName = file.FullName,
                WindowStyle = ProcessWindowStyle.Minimized,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardInput = true
            };
            var ua = new StringBuilder();
            p.OutputDataReceived += (s, e) =>
            {
                if (e.Data == null) return;
                ua.Append(e.Data + Environment.NewLine);
            };
            var start = DateTime.Now;
            p.Start();
            p.BeginOutputReadLine();
            p.StandardInput.WriteLineAsync(problem.In);
            p.WaitForExit();
            var t = (int)(DateTime.Now - start).TotalMilliseconds;
            var uas = ua.ToString();
            var pass = problem.Out == uas ? true : false;
            return new JudgeModel()
            {
                Pass = pass,
                Result = pass ? JudgeResult.AC : JudgeResult.WA,
                Time = t
            };
        }
        public static ProblemInfo ReadProblem(string id) 
        {
            var _out = "";
            var _in = "";
            {
                FileStream fs = new FileStream(AnswerPath.FullName + @"\" + id + @"\out.dat", FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                _out = sr.ReadToEnd();
                fs.Close();
                sr.Close();
            }
            {
                FileStream fs = new FileStream(AnswerPath.FullName + @"\" + id + @"\in.dat", FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                _in = sr.ReadToEnd();
                fs.Close();
                sr.Close();
            }
            return new ProblemInfo(_in, _out);
        }
    }
    class ProblemInfo
    {
        public string In { get; set; }
        public string Out { get; set; }
        public ProblemInfo(string _in,string _out)
        {
            In = _in;
            Out = _out;
        }
    }
}
