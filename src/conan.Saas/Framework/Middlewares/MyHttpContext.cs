﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace conan.Saas
{
    /// <summary>
    /// 获取当前上下文  中间件
    /// </summary>
    public static class MyHttpContext
    {
        public static IServiceProvider ServiceProvider;

        static MyHttpContext()
        { }


        public static HttpContext Current
        {
            get
            {
                object factory = ServiceProvider.GetService(typeof(Microsoft.AspNetCore.Http.IHttpContextAccessor));

                HttpContext context = ((IHttpContextAccessor)factory).HttpContext;
                return context;
            }
        }
    }
}
