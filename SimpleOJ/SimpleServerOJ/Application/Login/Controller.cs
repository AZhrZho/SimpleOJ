using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleServerOJ.Net;

namespace SimpleServerOJ.Application.Login
{
    class LoginController : IApplication
    {
        public string Name => "login";

        public HttpResponseArgs Handle(HttpArgs args)
        {
            var model = new LoginModel();
            var sno = args.GetPostValue()["sno"].ToString();
            var pw = args.GetPostValue()["pw"].ToString();
            var session = model.Login(sno, pw);
            if (session == string.Empty)
            {
                return ApiReturn.Return(new ApiReturn()
                {
                    Code = HttpCode.Success,
                    Message = "登陆失败",
                    Data = string.Empty
                });
            }
            Console.WriteLine(Program.TimeLabel()+"用户{0}已登录.", sno);
            return ApiReturn.Return(new ApiReturn()
            {
                Code = HttpCode.Success,
                Message = "登陆成功",
                Data = session
            });
        }
    }
}
