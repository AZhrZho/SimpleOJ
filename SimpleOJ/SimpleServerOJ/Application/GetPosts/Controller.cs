using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleServerOJ.Net;

namespace SimpleServerOJ.Application.GetPosts
{
    class Controller : IApplication
    {
        public string Name => "posts";

        public HttpResponseArgs Handle(HttpArgs args)
        {
            return ApiReturn.Return(new ApiReturn
            {
                Code = HttpCode.Success,
                Message = "获取提交记录成功",
                Data = new Model().Select(null)
            });
        }
    }
}
