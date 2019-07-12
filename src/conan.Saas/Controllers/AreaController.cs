#region using
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
#endregion
// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860
namespace conan.Saas.Controllers
{
    public class AreaController : CustomController
    {
        #region ini
        public IAreaListApp AreaListApp { get; set; }
        #endregion

        #region tree
        [IgnoreAuthorize]
        public async Task<IActionResult> loadtree(long ParentId)
        {
            var data = await AreaListApp.GetMenuListAsync();
            data = data.OrderBy(o => o.SortID).ToList();
            var trees = await Trees(data, 0, ParentId);
            string treedata = JsonHelper.SerializeObject(trees, true, true); //json long 转成 string, 名称用驼峰结构输出
            return Content(treedata);
        }

        #endregion

        #region Index
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<IActionResult> Index(areaOption filter)
        {
            ViewBag.filter = filter;
            AreaListlistmodel ItemsDatalistmodel = new AreaListlistmodel();
            List<AreaList> modules = await AreaListApp.GetListAsync(filter.ParentId ?? 0);
            ItemsDatalistmodel.AreaListlist = modules.ToList();
            ViewBag.ItemsData = ItemsDatalistmodel;
            return View();
        }

        #endregion

        #region 内部方法
        private async Task<List<TreeModel>> Trees(List<AreaList> data, long parentnodes, long sid)
        {
            var treeList = new List<TreeModel>();
            foreach (AreaList item in data.Where(o => o.ParentID == parentnodes))
            {
                long a = data.Where(o => o.Id == sid).FirstOrDefault()?.ParentID ?? 0;
                TreeModel treeModel = new TreeModel
                {
                    Id = item.Id,
                    Value = item.Id.ToString(),
                    Img = "fa fa-navicon",
                    Text = item.AreaName,
                    Parentnodes = parentnodes,
                    Showcheck = false,
                    Complete = false,
                    Isexpand = (0 == parentnodes && (sid == item.Id || (a == item.Id))) ? true : false,
                    Checkstate = (sid != 0 && sid == item.Id) ? 1 : 0,
                    HasChildren = data.Count(o => o.ParentID == item.Id) > 0
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