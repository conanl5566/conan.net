using Microsoft.AspNetCore.Authorization;
using Swashbuckle.Swagger.Model;
using Swashbuckle.SwaggerGen.Generator;
using System.Collections.Generic;
using System.Linq;

namespace conan.Api
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpHeaderOperation : IOperationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<IParameter>();
            }
            var actionAttrs = context.ApiDescription.GetActionAttributes();
            var isAuthorized = actionAttrs.Any(a => a.GetType() == typeof(BasicAuth));
            if (isAuthorized == false) //提供action都没有权限特性标记，检查控制器有没有
            {
                var controllerAttrs = context.ApiDescription.GetControllerAttributes();
                isAuthorized = controllerAttrs.Any(a => a.GetType() == typeof(BasicAuth));
            }
            var isAllowAnonymous = actionAttrs.Any(a => a.GetType() == typeof(AllowAnonymousAttribute));
            if (isAuthorized && isAllowAnonymous == false)
            {
                operation.Parameters.Add(new NonBodyParameter()
                {
                    Name = "Authorization",  //添加Authorization头部参数
                    In = "header",
                    Type = "string",
                    Required = false
                });
            }
        }
    }
}