using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleServerOJ.Net;

namespace SimpleServerOJ.Application.GetProblems
{
    class Controller : IApplication
    {
        public string Name => "list";

        public HttpResponseArgs Handle(HttpArgs args)
        {
            return ApiReturn.Return(new ApiReturn()
            {
                Code = HttpCode.Success,
                Message = "获取题目信息成功",
                Data = Data.Problems
            });
        }
    }
}
