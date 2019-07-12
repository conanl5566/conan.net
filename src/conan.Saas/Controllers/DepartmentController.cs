#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using conan.Saas.Framework;
using dotNET.Application.Infrastructure;
using dotNET.Domain.Entities;
using conan.Saas.Model;
using dotNET.Core;
using dotNET.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
#endregion
// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860
namespace conan.Saas.Controllers
{
    public class DepartmentController : CustomController
    {
        #region ini
        public IDepartmentApp IDepartmentApp { get; set; }
        #endregion

        #region tree
        [IgnoreAuthorize]
        public async Task<IActionResult> loadtree(long ParentId)
        {
            //菜单数据
            var data = await IDepartmentApp.GetDepartmentListAsync();
            List<TreeModel> TreeModellist = new List<TreeModel>();
            var s = await Trees(data, 0, ParentId);
            TreeModel treeModel = new TreeModel
            {
                Id = 0,
                Value = "0",
                Img = "",
                Text = "根节点",
                Parentnodes = -1,
                Showcheck = false,
                Complete = false,
                Isexpand = true,
                Checkstate = 0,
                HasChildren = true,
                ChildNodes = s
            };
            TreeModellist.Add(treeModel);
            string treedata = JsonHelper.SerializeObject(TreeModellist, true, true); //json long 转成 string, 名称用驼峰结构输出
            return Content(treedata);
        }

        #endregion

        #region 内部方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parentnodes"></param>
        /// <param name="sid"></param>
        /// <returns></returns>
        private async Task<List<TreeModel>> Trees(List<Department> data, long parentnodes, long sid)
        {
            var treeList = new List<TreeModel>();
            foreach (Department item in data.Where(o => o.ParentId == parentnodes))
            {
                TreeModel treeModel = new TreeModel
                {
                    Id = item.Id,
                    Value = item.Id.ToString(),
                    Img = "",
                    Text = item.Name,
                    Parentnodes = parentnodes,
                    Showcheck = false,
                    Complete = false,
                    Isexpand = true,
                    Checkstate = (sid != 0 && sid == item.Id) ? 1 : 0,
                    HasChildren = data.Count(o => o.ParentId == item.Id) > 0
                };
                treeModel.HasChildren = false;
                treeModel.ChildNodes = await Trees(data, item.Id, sid);
                if (treeModel.ChildNodes.Count > 0)
                {
                    treeModel.Complete = true;
                    treeModel.HasChildren = true;
                }
                treeList.Add(treeModel);
            }
            return treeList;
        }
        #endregion

        #region 列表
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<IActionResult> Index(DepartmentOption filter)
        {
            Departmentlistmodel ItemsDatalistmodel = new Departmentlistmodel();
            ViewBag.filter = filter;
            List<Department> modules = await IDepartmentApp.GetListAsync(filter);
            ItemsDatalistmodel.Departmentlist = modules;
            ViewBag.ItemsData = ItemsDatalistmodel;
            return View();
        }

        #endregion

        #region 添加
        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Create()
        {
            var data = await IDepartmentApp.GetDepartmentListAsync();
            DepartmentModel model = new DepartmentModel();
            data = data.SortDepartmentsForTree().ToList();
            var selectList = new List<SelectListItem>();
            selectList.Add(new SelectListItem()
            {
                Value = "0",
                Text = "顶级节点",
                Selected = false
            });
            foreach (var c in data)
                selectList.Add(new SelectListItem()
                {
                    Value = c.Id.ToString(),
                    Text = c.GetFormattedBreadCrumb(data),
                    Selected = c.Id == model.ParentId
                });
            model.pids = selectList;
            model.GoBackUrl = SetingBackUrl(this.HttpContext.Request);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DepartmentModel model)
        {
            if (!ModelState.IsValid)
            {
                return Operation(false, "数据验证失败;" + GetErrorFromModelStateStr(), model.GoBackUrl);
            }
            Department module = MapperHelper.Map<DepartmentModel, Department>(model);
            module.Id = module.CreateId();
            module.CreatorTime = DateTime.Now;
            module.CreatorUserId = CurrentUser.Id;
            var r = await IDepartmentApp.CreateAsync(module);
            return Operation(r.IsSuc, r.IsSuc ? "数据添加成功" : r.Msg, model.GoBackUrl);
        }
        #endregion

        #region 删除
        [HttpPost]
        public async Task<IActionResult> Delete(long Id)
        {
            var r = await IDepartmentApp.DeleteAsync(Id);
            return Operation(r.IsSuc, r.IsSuc ? "数据删除成功" : r.Msg);
        }
        #endregion

        #region 修改
        public async Task<IActionResult> Edit(long Id)
        {
            DepartmentModel model = new DepartmentModel();
            Department module = await IDepartmentApp.GetAsync(Id);
            if (module == null)
            {
                return NotFind();
            }
            model = MapperHelper.Map<Department, DepartmentModel>(module);
            model.GoBackUrl = SetingBackUrl(this.HttpContext.Request);
            var data = await IDepartmentApp.GetDepartmentListAsync();
            data = data.SortDepartmentsForTree().ToList();
            var selectList = new List<SelectListItem>();
            selectList.Add(new SelectListItem()
            {
                Value = "0",
                Text = "顶级节点",
                Selected = false
            });
            foreach (var c in data)
                selectList.Add(new SelectListItem()
                {
                    Value = c.Id.ToString(),
                    Text = c.GetFormattedBreadCrumb(data),
                    Selected = c.Id == model.ParentId
                });

            model.pids = selectList;
            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Edit(DepartmentModel model)
        {
            if (!ModelState.IsValid)
            {
                return Operation(false, "数据验证失败;" + GetErrorFromModelStateStr(), model.GoBackUrl);
            }
            Department m = await IDepartmentApp.GetAsync(model.Id);
            if (m == null)
            {
                return Operation(false, "数据不存在或已被删除");
            }
            m = MapperHelper.Map<DepartmentModel, Department>(model, m);
            var r = await IDepartmentApp.UpdateAsync(m);
            return Operation(r.IsSuc, r.Msg, model.GoBackUrl);
        }
        #endregion
    }
}
