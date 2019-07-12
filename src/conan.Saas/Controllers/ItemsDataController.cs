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
    public class ItemsDataController : CustomController
    {
        public IItemsDataApp IItemsDataApp { get; set; }

        #region tree
        [IgnoreAuthorize]
        public async Task<IActionResult> loadtree(long ParentId)
        {
            //菜单数据
            var data = await IItemsDataApp.GetItemsDataListAsync();

            var s = await Trees(data, 0, ParentId);

            string treedata = JsonHelper.SerializeObject(s, true, true); //json long 转成 string, 名称用驼峰结构输出

            return Content(treedata);


        }

        #endregion

        #region 列表

        public async Task<IActionResult> Index(ItemsDataOption filter)
        {
            ItemsDatalistmodel ItemsDatalistmodel = new ItemsDatalistmodel();
            ViewBag.filter = filter;
            List<ItemsData> modules = await IItemsDataApp.GetListAsync(filter);

            ItemsDatalistmodel.ItemsDatalist = modules.OrderBy(o => o.SortCode).ToList();
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
            var data = await IItemsDataApp.GetItemsDataListAsync();

            ItemsDataModel model = new ItemsDataModel();
            data = data.SortDepartmentsForTree().ToList();
            var selectList = new List<SelectListItem>();
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
        public async Task<IActionResult> Create(ItemsDataModel model)
        {
            if (!ModelState.IsValid)
            {

                return Operation(false, "数据验证失败;" + GetErrorFromModelStateStr(), model.GoBackUrl);
            }
            ItemsData module = MapperHelper.Map<ItemsDataModel, ItemsData>(model);
            module.Id = module.CreateId();
            module.CreatorTime = DateTime.Now;
            module.CreatorUserId = CurrentUser.Id;

            var r = await IItemsDataApp.CreateAsync(module);

            return Operation(r.IsSuc, r.IsSuc ? "数据添加成功" : r.Msg, model.GoBackUrl);
        }
        #endregion

        #region 删除
        [HttpPost]
        public async Task<IActionResult> Delete(long Id)
        {
            var r = await IItemsDataApp.DeleteAsync(Id);
            return Operation(r.IsSuc, r.IsSuc ? "数据删除成功" : r.Msg);
        }
        #endregion

        #region 修改
        public async Task<IActionResult> Edit(long Id)
        {
            ItemsDataModel model = new ItemsDataModel();
            ItemsData module = await IItemsDataApp.GetAsync(Id);
            if (module == null)
            {
                return NotFind();
            }
            model = MapperHelper.Map<ItemsData, ItemsDataModel>(module);
            model.GoBackUrl = SetingBackUrl(this.HttpContext.Request);
            var data = await IItemsDataApp.GetItemsDataListAsync();

            data = data.SortDepartmentsForTree().ToList();

            var selectList = new List<SelectListItem>();
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

        [HttpPost]
        public async Task<IActionResult> Edit(ItemsDataModel model)
        {
            if (!ModelState.IsValid)
            {
                return Operation(false, GetErrorFromModelStateStr());
            }
            ItemsData m = await IItemsDataApp.GetAsync(model.Id);
            if (m == null)
            {
                return Operation(false, "数据不存在或已被删除");
            }
            m = MapperHelper.Map<ItemsDataModel, ItemsData>(model, m);
            var r = await IItemsDataApp.UpdateAsync(m);

            return Operation(r.IsSuc, r.IsSuc ? "数据编辑成功" : r.Msg, model.GoBackUrl);


        }
        #endregion

        #region 内部方法


        private async Task<List<TreeModel>> Trees(List<ItemsData> data, long parentnodes, long sid)
        {
            var treeList = new List<TreeModel>();
            foreach (ItemsData item in data.Where(o => o.ParentId == parentnodes))
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
    }
}