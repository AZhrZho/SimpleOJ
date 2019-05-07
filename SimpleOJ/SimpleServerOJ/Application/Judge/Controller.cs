using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleServerOJ.Net;

namespace SimpleServerOJ.Application.Judge
{
    class JudgeController : IApplication
    {
        public string Name => "judge";

        public HttpResponseArgs Handle(HttpArgs args)
        {
            var model = new JudgeModel();
            var code = (string)args.GetPostValue()["code"];
            var session = (string)args.GetPostValue()["session"];
            var id = args.GetArgValue("id");        //题目ID
            var language = args.GetArgValue("lan"); //语言
            var sno = Data.GetSno(session);
            var source = Handler.SaveAsFile(code, id, "12345", language);
            var exe = Handler.Compiler(source, language);
            if (!exe.Exists)
            {
                model.Insert(new Dictionary<string, object>
                {
                    {"problem",id },
                    {"language",language },
                    {"runtime",0 },
                    {"result",JudgeResult.EA },
                    {"sno", sno}
                });
            }
            var reslut = Handler.Judge(id, exe);
            model.Insert(new Dictionary<string, object>
                {
                    {"problem",id },
                    {"language",language },
                    {"runtime",reslut.Time },
                    {"result",reslut.Result },
                    {"sno", sno}
                });
            Console.WriteLine(Program.TimeLabel()+"用户{0}提交了问题{1}的答案，运行结果{2}，用时{3}ms.", sno, id, reslut.Result.ToString(), reslut.Time);
            return ApiReturn.Return(new ApiReturn()
            {
                Code = HttpCode.Success,
                Message = reslut.Pass ? "运行通过" : "答案错误",
                Data = reslut
            });

        }

    }
}
