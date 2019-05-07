using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleServerOJ.Net;

namespace SimpleServerOJ.Application.Register
{
    class RegisterController : IApplication
    {
        public string Name => "register";

        public HttpResponseArgs Handle(HttpArgs args)
        {
            var model = new RegisterModel();
            var sno = args.GetPostValue()["sno"].ToString();
            var pw = args.GetPostValue()["pw"].ToString();
            var register = model.Register(sno, pw);
            Console.WriteLine(Program.TimeLabel()+"用户{0}已注册.", sno);
            return ApiReturn.Return(new ApiReturn()
            {
                Code = HttpCode.Success,
                Message = register ? "注册成功" : "账号已存在",
                Data = register
            });
        }
    }
}
