#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using conan.Saas.Framework;
using dotNET.Dto;
using dotNET.Core;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
#endregion
// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace conan.Saas.Controllers
{
    public class ErrorController : CustomController
    {
        #region Index
        // GET: /<controller>/
        public async Task<IActionResult> Index(ErrorLogOption filter)
        {
            ViewBag.filter = filter;
            List<ErrorLogDto> Model = new List<ErrorLogDto>();
            var contentRoot = Directory.GetCurrentDirectory();
            var webRoot = Path.Combine(contentRoot, "logs\\Error");
            List<string> filelist = new List<string>();
            new ServiceCollection()
            .AddSingleton<IFileProvider>(new PhysicalFileProvider(webRoot))
             .AddSingleton<IFileManager, FileManager>()
             .BuildServiceProvider()
            .GetService<IFileManager>()
           .ShowStructure((layer, name) => filelist.Add(name));
            List<ErrorLogDto> result = new List<ErrorLogDto>();
            if (filelist != null && filelist.Count > 0)
            {
                int i = 1;
                foreach (var item in filelist.OrderByDescending(o => o))
                {
                    ErrorLogDto m = new ErrorLogDto();
                    m.Id = i;
                    m.filename = item;
                    m.CreatorTime = ZConvert.StrToDateTime(item.Replace(".log", ""), DateTime.Now);
                    result.Add(m);
                    i++;
                }
            }
            if (filter.kCreatorTime != null && filter.kCreatorTime.HasValue)
            {
                result = result.Where(o => o.CreatorTime >= filter.kCreatorTime.Value).ToList();
            }
            if (filter.eCreatorTime != null && filter.eCreatorTime.HasValue)
            {
                result = result.Where(o => o.CreatorTime <= filter.eCreatorTime.Value).ToList();
            }
            Model = result.OrderByDescending(o => o.CreatorTime).ToList();
            return View(Model);
        }

        #endregion

        #region 查看
        [IgnoreAuthorize]
        public async Task<IActionResult> Details(string Id)
        {
            var contentRoot = Directory.GetCurrentDirectory();
            var webRoot = Path.Combine(contentRoot, "logs\\Error");
            string content = new ServiceCollection()
              .AddSingleton<IFileProvider>(new PhysicalFileProvider(webRoot))
              .AddSingleton<IFileManager, FileManager>()
              .BuildServiceProvider()
             .GetService<IFileManager>()
              .ReadAllTextAsync(Id).Result;
            ViewBag.content = content;
            ViewBag.filename = Id;
            ViewBag.GoBackUrl = SetingBackUrl(this.HttpContext.Request);
            return View();
        }
        #endregion
    }
}