#region using
using conan.Saas.Framework;
using dotNET.Application.Infrastructure;
using dotNET.Domain.Entities;
using dotNET.Core;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#endregion
namespace conan.Saas.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class HomeController : CustomController
    {
        #region ini
        public IRoleAuthorizeApp RoleAuthorizeApp { get; set; }
        #endregion

        #region Index
        [IgnoreAuthorize]
        public IActionResult Index()
        {
            ViewBag.RealName = CurrentUser.RealName;
            ViewBag.Avatar = CurrentUser.Avatar;
            return View();
        }
        #endregion

        #region Menu
        [IgnoreAuthorize]
        public async Task<IActionResult> Menu()
        {
            NLogger.Info( "菜单："+ JsonHelper.SerializeObject(CurrentUser));
            List<Module> modules = await RoleAuthorizeApp.GetModuleList(CurrentUser.RoleId, CurrentUser.IsSys);
            NLogger.Info(JsonHelper.SerializeObject(modules));
            return Json(ToMenu(modules, 0));
        }

        #endregion

        #region Dashboard
        [IgnoreAuthorize]
        public IActionResult Dashboard()
        {
            return View();
        }
        #endregion

        #region VisitModule
        public IActionResult VisitModule()
        {
            return Content("");
        }
        #endregion

        #region Error
        public IActionResult Error()
        {
            return View();
        }
        #endregion

        #region 转成菜单
        /// <summary>
        /// 转成菜单
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private static List<MenuModel> ToMenu(List<Module> data, long parentId)
        {
            var childNodeList = data.FindAll(t => t.ParentId == parentId).ToList();
            var list = new List<MenuModel>();
            foreach (Module item in childNodeList)
            {
                MenuModel mm = new MenuModel
                {
                    Id = item.Id.ToString(),
                    Icon = item.Icon,
                    IsOpen = false,
                    Text = item.FullName,
                    TargetType = item.Target == "iframe" ? "iframe-tab" : item.Target
                };
                if (!string.IsNullOrEmpty(item.UrlAddress))
                {
                    if (item.UrlAddress.StartsWith("/"))
                    {
                        mm.Url = item.UrlAddress.Substring(1);
                    }
                    else
                    {
                        mm.Url = item.UrlAddress;
                    }
                }

                mm.Children = ToMenu(data, item.Id);

                list.Add(mm);
            }
            return list;
        }

        #endregion
    }
}