#region using
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using dotNET.Application.Infrastructure;
using dotNET.Application;
using dotNET.Core;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using dotNET.Application.App;
#endregion

namespace conan.Saas.Framework
{
    /// <summary>
    /// 权限
    /// </summary>
    public class MvcMenuFilter : ActionFilterAttribute, IActionFilter
    {
        public IRoleAuthorizeApp _roleAuthorizeApp;
        //IRoleAuthorizeApp roleAuthorizeApp
        public MvcMenuFilter(IRoleAuthorizeApp roleAuthorizeApp)
        {
            _roleAuthorizeApp = roleAuthorizeApp;
        }
        #region 权限控制
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionDescriptor.FilterDescriptors.Count(o => o.Filter.GetType().Name == "AllowAttribute") == 0)
            {
                string path = "";
                string controller = context.RouteData.Values["controller"].ToString();
                string action = context.RouteData.Values["action"].ToString();
                string area = "";
                try
                {
                    area = context.RouteData.Values["area"].ToString();
                }
                catch
                {
                    area = "";
                }
                if (string.IsNullOrWhiteSpace(area))
                {
                    path = "/" + controller + "/" + action;
                }
                else
                {
                    path = "/" + area + "/" + controller + "/" + action;
                }
                var currentUser = await getAuthorization(context);
                if (currentUser == null)
                {
                    context.Result =
                   new RedirectResult("/account/login?returnUrl=" + path);
                }
                bool ignore = context.ActionDescriptor.FilterDescriptors.Count(o => o.Filter.GetType().Name == "IgnoreAuthorizeAttribute") > 0;
                if (ignore)
                {
                }
                else
                {
                    if (currentUser.IsSys)
                    {
                    }
                    else
                    {
                       // var _roleAuthorizeApp = AutofacExt.GetFromFac<IRoleAuthorizeApp>();
                        bool b = await _roleAuthorizeApp.ActionValidate(currentUser.RoleId, path);
                        if (!b)

                        {
                            if (context.HttpContext.Request.IsAjaxRequest())
                            {
                                JsonResult json = new JsonResult(new { IsSucceeded = false, Message = "您没有操作权限" });
                                context.Result = json;
                            }
                            else
                            {
                                context.Result = new RedirectResult("/account/Nofind?bakurl=" + context.HttpContext.Request.Headers["Referer"].FirstOrDefault());

                            }
                        }
                    }

                }
            }

            await base.OnActionExecutionAsync(context, next);

        }
        #endregion

        #region 当前用户
        /// <summary>
        /// 当前用户
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        private async Task<CurrentUser> getAuthorization(ActionExecutingContext filterContext)
        {
            var result = await filterContext.HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (result.Succeeded)
            {
                return CurrentUser.FromJson(result.Principal.Claims.FirstOrDefault(o => o.Type == ClaimTypes.UserData).Value);
            }
            return null;
        }
        #endregion
    }
}