﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace conan.Saas.Framework
{
    public static class RequestExt
    {
        /// <summary>
        /// Determines whether the specified HTTP request is an AJAX request.
        /// </summary>
        /// 
        /// <returns>
        /// true if the specified HTTP request is an AJAX request; otherwise, false.
        /// </returns>
        /// <param name="request">The HTTP request.</param>
        ///  <exception cref="T:System.ArgumentNullException">
        ///  The <paramref name="request"/> 
        ///  parameter is null (Nothing in Visual Basic).</exception>
        public static bool IsAjaxRequest(this Microsoft.AspNetCore.Http.HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (request.Headers != null)
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return false;
        }
    }
}
