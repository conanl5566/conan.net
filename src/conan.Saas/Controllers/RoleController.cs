using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using conan.Saas.Framework;
using dotNET.Application.Infrastructure;
using dotNET.Dto;
using dotNET.Core;
using dotNET.Domain.Entities;
using conan.Saas.Model;
using Microsoft.Extensions.Options;
// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace conan.Saas.Controllers
{
    public class RoleController : CustomController
    {
        public IRoleApp RoleApp { get; set; }
        public IRoleAuthorizeApp RoleAuthorizeApp { get; set; }
        public IModuleApp ModuleApp { get; set; }
        public IModuleButtonApp ModuleButtonApp { get; set; }
        public SiteConfig Config;
        public RoleController(IOptions<SiteConfig> option)
        {
            Config = option.Value;
            DefaultPageSize = ZConvert.StrToInt(Config.Configlist.FirstOrDefault(o => o.Key == "pagesize").Values);

        }

        //
        // GET: /<controller>/
        public async Task<IActionResult> Index(RoleOption filter, int? page)
        {

            ViewBag.filter = filter;
            var currentPageNum = page.HasValue ? page.Value : 1;
            var result = await RoleApp.GetPageAsync(currentPageNum, DefaultPageSize, filter);

            var model = new BaseListViewModel<Role>();
            model.list = result.Data;
            model.Paging.CurrentPage = currentPageNum;
            model.Paging.ItemsPerPage = DefaultPageSize;
            model.Paging.TotalItems = result.ItemCount;
            return View(model);
        }

        #region 添加
        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Create()
        {
            Rolemodel m = new Rolemodel();
            m.GoBackUrl = SetingBackUrl(this.HttpContext.Request);
            //菜单数据
            ViewData["tree"] = TreeModel.ToJson(await GetPermissionTree(null)); //json long 转成 string, 名称用驼峰结构输出
            return View(m);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Rolemodel model, List<long> permissionIds)
        {
            if (!ModelState.IsValid)
            {
                return Operation(false, "数据验证失败" + GetErrorFromModelStateStr());
            }
            Role m = MapperHelper.Map<Rolemodel, Role>(model);
            var r = await RoleApp.CreateAsync(m, permissionIds, CurrentUser);
            return Operation(r.IsSuc, r.IsSuc ? "数据添加成功" : r.Msg, model.GoBackUrl);
        }
        #endregion

        #region 删除
        [HttpPost]
        public async Task<IActionResult> Delete(long Id)
        {
            var r = await RoleApp.DeleteAsync(Id, CurrentUser);
            return Operation(r.s, r.msg);
        }
        #endregion

        #region 修改
        public async Task<IActionResult> Edit(long Id)
        {

            Role role = await RoleApp.GetAsync(Id);
            if (role == null)
            {
                return NotFind();
            }

            Rolemodel model = new Rolemodel();

            model = MapperHelper.Map<Role, Rolemodel>(role);
            model.GoBackUrl = SetingBackUrl(this.HttpContext.Request);

            ViewData["tree"] = TreeModel.ToJson(await GetPermissionTree(Id));
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Rolemodel model, List<long> permissionIds)
        {
            if (!ModelState.IsValid)
            {
                return Operation(false, GetErrorFromModelStateStr());
            }

            Role m = await RoleApp.GetAsync(model.Id);
            if (m == null)
            {
                return Operation(false, "数据不存在或已被删除");
            }

            m = MapperHelper.Map<Rolemodel, Role>(model, m);
            var r = await RoleApp.UpdateAsync(m, permissionIds, CurrentUser);

            return Operation(r.IsSuc, r.Msg, model.GoBackUrl);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        private async Task<List<TreeModel>> GetPermissionTree(long? roleId)
        {
            var moduledata = (await ModuleApp.GetSaasModuleListAsync()).OrderBy(o => o.SortCode).ToList();
            var buttondata = (await ModuleButtonApp.GetSaasModuleListAsync()).OrderBy(o => o.SortCode).ToList();
            var authorizedata = new List<RoleAuthorize>();
            if (roleId.HasValue)
            {
                authorizedata = (await RoleAuthorizeApp.GetListAsync(roleId.Value, 1)).ToList();
            }
            var treeList = new List<TreeModel>();
            foreach (Module item in moduledata)
            {
                TreeModel tree = new TreeModel();
                tree.Id = item.Id;
                tree.Text = item.FullName;
                tree.Value = item.Id.ToString();
                tree.Parentnodes = item.ParentId;
                tree.Isexpand = true;
                tree.Complete = false;
                tree.Showcheck = true;
                tree.Checkstate = authorizedata.Count(t => t.ItemId == item.Id);
                tree.HasChildren = false;
                tree.Img = item.Icon == "" ? "" : item.Icon;
                treeList.Add(tree);
            }

            foreach (ModuleButton item in buttondata)
            {
                TreeModel tree = new TreeModel();
                tree.Id = item.Id;
                tree.Text = item.FullName;
                tree.Value = item.Id.ToString();
                tree.Parentnodes = item.ParentId == 0 ? item.ModuleId : item.ParentId;
                tree.Isexpand = true;
                tree.Complete = false;
                tree.Showcheck = true;
                tree.Checkstate = authorizedata.Count(t => t.ItemId == item.Id);
                tree.HasChildren = false;
                tree.Img = item.Icon == "" ? "" : item.Icon;
                treeList.Add(tree);
            }
            return treeList;
        }
    }
}