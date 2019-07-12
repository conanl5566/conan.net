using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using dotNET.Application;
using dotNET.Core;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using dotNET.Application.Infrastructure;
using System;

namespace conan.Saas.Framework
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class IgnoreAuthorizeAttribute : ActionFilterAttribute
    {
        public IgnoreAuthorizeAttribute() { }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class AllowAttribute : ActionFilterAttribute
    {
        public AllowAttribute() { }
    }

}
