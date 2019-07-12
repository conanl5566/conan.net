#region using
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using conan.Saas.Framework;
using dotNET.Application.Infrastructure;
using dotNET.Dto;
using conan.Saas.Model;
using dotNET.Core;
using Microsoft.Extensions.Options;
#endregion
// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860
namespace conan.Saas.Areas.operateLog.Controllers
{
    [Area("operateLog")]
    public class HomeController : CustomController
    {
        #region ini
        public IOperateLogApp _IOperateLogApp { get; set; }
        public SiteConfig Config;
        public HomeController(IOptions<SiteConfig> option)
        {
            Config = option.Value;
            DefaultPageSize = ZConvert.StrToInt(Config.Configlist.FirstOrDefault(o => o.Key == "pagesize")?.Values);
        }

        #endregion

        #region Index
        // GET: /<controller>/
        public async Task<IActionResult> Index(OperateLogOption filter, int? page)
        {
            ViewBag.filter = filter;
            var currentPageNum = page.HasValue ? page.Value : 1;
            var result = await _IOperateLogApp.GetPageAsync(currentPageNum, DefaultPageSize, filter);
            var model = new BaseListViewModel<OperateLogDto>();
            model.list = result.Data;
            model.Paging.CurrentPage = currentPageNum;
            model.Paging.ItemsPerPage = DefaultPageSize;
            model.Paging.TotalItems = result.ItemCount;
            return View(model);
        }
        #endregion
    }
}