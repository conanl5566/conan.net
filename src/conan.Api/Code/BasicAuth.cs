using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Security.Principal;

namespace conan.Api
{
    /// <summary>
    /// 权限
    /// </summary>
    public class BasicAuth : ActionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request != null && context.HttpContext.Request.Headers != null && context.HttpContext.Request.Headers["Authorization"].Count > 0)
            {
                var Token = context.HttpContext.Request.Headers["Authorization"];
                if (string.IsNullOrWhiteSpace(Token))
                {
                    Meta Meta = new Meta() { State = 401, Msg = "Unauthorized" };
                    JsonResult json = new JsonResult(new
                    {
                        Meta
                    }
                    );
                    JsonSerializerSettings jsetting = new JsonSerializerSettings();
                    jsetting.NullValueHandling = NullValueHandling.Ignore;
                    jsetting.Converters.Add(
                        new Newtonsoft.Json.Converters.IsoDateTimeConverter()
                        {
                            DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
                        }
                    );
                    json.SerializerSettings = jsetting;
                    json.ContentType = "application/json; charset=utf-8";
                    context.Result = json;
                }
                else
                {
                    GenericIdentity ci = new GenericIdentity(Token);
                    ci.Label = "conan1111111";
                    context.HttpContext.User = new GenericPrincipal(ci, null);
                }
            }
            else
            {
                Meta Meta = new Meta() { State = 401, Msg = "Unauthorized" };
                JsonResult json = new JsonResult(new
                {
                    Meta
                }
                );
                JsonSerializerSettings jsetting = new JsonSerializerSettings();
                jsetting.NullValueHandling = NullValueHandling.Ignore;
                jsetting.Converters.Add(
                    new Newtonsoft.Json.Converters.IsoDateTimeConverter()
                    {
                        DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
                    }
                );
                json.SerializerSettings = jsetting;
                json.ContentType = "application/json; charset=utf-8";
                context.Result = json;
            }
            base.OnActionExecuting(context);
        }
    }
}