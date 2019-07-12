#region using
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using dotNET.Application.Infrastructure;
using conan.Saas.Web.Model;
using dotNET.Domain.Entities;
using conan.Saas.Framework;
using dotNET.Core;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Linq;
#endregion
namespace conan.Saas.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [IgnoreAuthorize]
    public class AccountController : Controller
    {
        #region ini
        public IUserApp UserApp { get; set; }
        private readonly ILogger _logger;
        public AccountController( ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<AccountController>();
        }
        #endregion

        #region Login
        /// <summary>
        /// 
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAttribute]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAttribute]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { state = "error", message = "数据验证失败" });
            }
            string ip = GetRemoteIpAddress();
            var r = await UserApp.SaasLoginAsync(model.Account, model.Password, ip);
            if (!string.IsNullOrEmpty(r.Error))
            {
                return Json(new { state = "error", message = r.Error });
            }
            var claims = new List<Claim>
                                        {
                                            new Claim(ClaimTypes.UserData, getCurrentUser(r.User, ip).ToString()),
                                        };
            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.Now.AddMinutes(120)
            };
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            return Json(new { state = "success", message = "登录成功。", returnUrl = RedirectToLocal(returnUrl) });
        }

        #endregion

        #region LogOff
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            if (User.Identity.IsAuthenticated)
            {
                string userdata = User.Claims.FirstOrDefault(o => o.Type == ClaimTypes.UserData).Value;
                await UserApp.LogOffAsync(CurrentUser.FromJson(userdata));
            }
            await HttpContext.SignOutAsync(
             CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(AccountController.Login), "Account");
        }
        #endregion

        #region Nofind
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bakurl"></param>
        /// <returns></returns>
        public async Task<IActionResult> Nofind(string bakurl)
        {
            ViewBag.bakurl = bakurl;
            return View();
        }
        #endregion

        #region Helpers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        CurrentUser getCurrentUser(User user, string ip)
        {
            var operatorMode = new CurrentUser
            {
                Avatar = user.Avatar,
                IsSys = user.IsSys,
                Id = user.Id,
                RoleId = user.RoleId,
                RealName = user.Account,
                UserType = "Saas",
                AgentId = 0,
                LoginIPAddress = ip
            };
            return operatorMode;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetRemoteIpAddress()
        {
            string FORWARDED_FOR = HttpContext.Request.Headers["HTTP_X_FORWARDED_FOR"].ToString();
            string REMOTE_ADDR = HttpContext.Connection.RemoteIpAddress.ToString();
            string result = !string.IsNullOrEmpty(FORWARDED_FOR) ? FORWARDED_FOR : REMOTE_ADDR;
            if (string.IsNullOrEmpty(result) || result == "::1") //|| !Utils.IsIP(result)
                return "127.0.0.1";
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        private string RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return returnUrl;
            }
            else
            {
                return "/Home/Index";
            }
        }
        #endregion
    }
}
