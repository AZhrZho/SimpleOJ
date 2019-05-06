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
            var code = (string)args.GetPostValue()["code"];
            var session = args.GetPostValue()["session"];
            var id = args.GetArgValue("id");        //题目ID
            var language = args.GetArgValue("lan"); //语言
            var source = Handler.SaveAsFile(code, id, "12345", language);
            var exe = Handler.Compiler(source, language);
            var reslut = Handler.Judge(id, exe);
            return ApiReturn.Return(new ApiReturn()
            {
                Code = HttpCode.Success,
                Message = reslut.Pass ? "运行通过" : "答案错误",
                Data = reslut
            });

        }

    }
}
