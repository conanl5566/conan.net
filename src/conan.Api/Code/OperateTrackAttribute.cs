using dotNET.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IO;
using System.Text;

namespace conan.Api
{
    /// <summary>
    /// 拦截器
    /// </summary>
    public class OperateTrackAttribute : ActionFilterAttribute
    {
        private static readonly string key = "enterTime";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //记录进入请求的时间
            context.HttpContext.Items[key] = DateTime.Now.ToBinary();
            base.OnActionExecuting(context);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            object beginTime = null;
            if (context.HttpContext.Items.TryGetValue(key, out beginTime))
            {
                DateTime time = DateTime.FromBinary(Convert.ToInt64(beginTime));
                HttpRequest request = context.HttpContext.Request;
                //var log = new
                //{
                //    requestUri = request.Url.AbsoluteUri,
                //    method = request.HttpMethod,
                //    actionName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName,
                //    controllerName = actionExecutedContext.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                //    enterTime = time,//获取action开始执行的时间
                //    costTime = (DateTime.Now - time).TotalMilliseconds,//获取执行action的耗时
                //    token = request.Headers["Authorization"],
                //    version = request.Headers["version"],
                //    client = request.Headers["client"],
                //    package = request.Headers["package"],
                //    //navigator = request.UserAgent,
                //    ip = request.UserHostAddress,//获取访问的ip
                //    //browser = request.Browser.Browser + " - " + request.Browser.Version + " - " + request.Browser.Type,
                //    //获取request提交的参数
                //    paramaters = GetRequestValues(actionExecutedContext),//JsonConvert.SerializeObject(request.QueryString),//JsonConvert.SerializeObject(actionExecutedContext.ActionContext.ActionDescriptor.GetParameters()),
                //    //获取response响应的结果
                //    executeResult = GetResponseValues(actionExecutedContext),// "",//JsonConvert.SerializeObject(actionExecutedContext.Response.RequestMessage),
                //};
                //NLogger.Info(JsonHelper.SerializeObject(log), "【数据请求】");

                StringBuilder sb = new StringBuilder();
                sb.AppendLine();
                sb.AppendLine($"Url：{request.Path}");
                sb.AppendLine($"Method：{request.Method}");
                sb.AppendLine($"Controller：{context.Controller.ToString()}");
                sb.AppendLine($"Action：{context.ActionDescriptor.DisplayName}");
                sb.AppendLine($"EnterTime：{time}");
                sb.AppendLine($"CostTime：{(DateTime.Now - time).TotalMilliseconds}");
                //sb.AppendLine("--------------------");
                if (context.HttpContext.Request != null && context.HttpContext.Request.Headers != null && request.Headers["Authorization"].Count > 0)
                    //   return true;

                    // if ( context.HttpContext.Request.Headers["Authorization"]. != null)
                    sb.AppendLine($"Token：{request.Headers["Authorization"]}");
                sb.AppendLine($"Version：{request.Headers["version"]}");
                sb.AppendLine($"Client：{request.Headers["client"]}");
                sb.AppendLine($"Package：{request.Headers["package"]}");
                //sb.AppendLine("--------------------");
                if (request.Method == "POST")
                    sb.AppendLine($"Paramaters：{GetRequestValues(context)}");

                sb.AppendLine($"返回结果：{GetResponseValues(context)}");
                sb.AppendLine();
                sb.AppendLine();

                NLogger.Info(sb.ToString(), "【数据请求】");
            }
            base.OnActionExecuted(context);
        }

        /// <summary>
        /// 读取request 的提交内容
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <returns></returns>
        public string GetRequestValues(ActionExecutedContext actionExecutedContext)
        {
            Stream stream = actionExecutedContext.HttpContext.Request.Body;
            stream.Position = 0;
            Encoding encoding = Encoding.UTF8;
            /*
                这个StreamReader不能关闭，也不能dispose， 关了就傻逼了
                因为你关掉后，后面的管道  或拦截器就没办法读取了
            */
            var reader = new StreamReader(stream, encoding);
            string result = reader.ReadToEnd();
            /*
            这里也要注意：   stream.Position = 0;
            当你读取完之后必须把stream的位置设为开始
            因为request和response读取完以后Position到最后一个位置，交给下一个方法处理的时候就会读不到内容了。
            */
            stream.Position = 0;
            return result;
        }

        /// <summary>
        /// 读取action返回的result
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <returns></returns>
        public string GetResponseValues(ActionExecutedContext actionExecutedContext)
        {
            try
            {
                Stream stream = actionExecutedContext.HttpContext.Response.Body;
                Encoding encoding = Encoding.UTF8;
                /*
                这个StreamReader不能关闭，也不能dispose， 关了就傻逼了
                因为你关掉后，后面的管道  或拦截器就没办法读取了
                */
                var reader = new StreamReader(stream, encoding);
                string result = reader.ReadToEnd();
                /*
                这里也要注意：   stream.Position = 0; 
                当你读取完之后必须把stream的位置设为开始
                因为request和response读取完以后Position到最后一个位置，交给下一个方法处理的时候就会读不到内容了。
                */
                stream.Position = 0;
                return result;
            }
            catch
            {
                return string.Empty;
            }
        }


    }
}
