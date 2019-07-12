using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using conan.Api.Dto;
using conan.Api.Code;

namespace conan.Api.Controllers
{
    /// <summary>
    /// 测试
    /// </summary>

    [Route("api/Values")]
    public class ValuesController : DefaultController
    {

        /// <summary>
        /// 帐号注册
        /// </summary>
        /// <param name="model">测试</param>
        /// <returns>测试</returns>
        [BasicAuth]
        [ModelValidationAttribute]
        [HttpPost, Route("register")]
        public R<Login> register([FromBody]Register model)
        {
            //Convert.ToInt16("q");
            UserData ud = UserData.CurrentUser(this.HttpContext);
            //  return R<Login>.Err("验证码错误");
            Login Login = new Login();
            Login.Account = model.Account + "-" + model.Code;
            return R<Login>.Suc(Login);

        }
        /// <summary>
        /// 检测帐号是不已存在  
        /// </summary>
        /// <param name="account">(必填)帐号或手机号    Data=true 已存在,Data=false 不存在</param>
        /// <returns>测试</returns>
        [HttpGet, Route("existAccount")]
        public R<bool> ExistAccount([FromQuery] string account)
        {
            return R<bool>.Suc(true);
        }
    }
}