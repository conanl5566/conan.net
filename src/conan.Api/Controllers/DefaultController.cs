using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using dotNET.Core;
using Microsoft.AspNetCore.Cors;
using conan.Api.Code;

namespace conan.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Produces("application/json")]
    [OperateTrackAttribute]
    [CustomExceptionFilterAttribute]
    public class DefaultController : Controller
    {
    }
}