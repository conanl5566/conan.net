using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using conan.Saas.Framework;
using dotNET.Application.Infrastructure;
using dotNET.Domain.Entities;
using conan.Saas.Model;
using dotNET.Core;
using dotNET.Dto;
// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860
namespace conan.Saas.Controllers
{
    [IgnoreAuthorize]
    public class ModuleButtonController : CustomController
    {
        public IModuleButtonApp ModuleButtonApp { get; set; }

        // 列表
        // GET: /<controller>/
        public async Task<IActionResult> Index(ModuleButtonOption option)
        {
            //返回json
            if (Request.IsAjaxRequest())
            {
                var modules = await ModuleButtonApp.GetSaasModuleListAsync(option);
                return Json(new { rows = modules });
            }
            ViewData["ModuleId"] = option.ModuleId;
            ViewData["ParentId"] = option.ParentId;
            return View();
        }

        #region 添加
        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        public IActionResult Create(long moduleId, long parentId)
        {
            ViewData["ModuleId"] = moduleId;
            ViewData["ParentId"] = parentId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ModuleButtonModel model, long moduleId, long parentId)
        {
            if (!ModelState.IsValid)
            {
                return Operation(false, "数据验证失败");
            }
            ModuleButton module = MapperHelper.Map<ModuleButtonModel, ModuleButton>(model);

            module.ParentId = parentId;
            module.ModuleId = moduleId;
            var r = await ModuleButtonApp.CreateAsync(module);

            return Operation(r.IsSuc, r.IsSuc ? "数据添加成功" : r.Msg);
        }
        #endregion

        #region 删除
        [HttpPost]
        public async Task<IActionResult> Delete(long Id)
        {
            var r = await ModuleButtonApp.DeleteAsync(Id);
            return Operation(r.IsSuc, r.IsSuc ? "数据删除成功" : r.Msg);
        }
        #endregion

        #region 修改
        public async Task<IActionResult> Edit(long Id)
        {
            ModuleButton module = await ModuleButtonApp.GetAsync(Id);
            if (module == null)
            {
                return NotFind();
            }
            ViewData["Model"] = JsonHelper.SerializeObject(module, false, true);//json 名称用驼峰结构输出
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ModuleButtonModel model)
        {
            if (!ModelState.IsValid)
            {
                return Operation(false, GetErrorFromModelStateStr());
            }

            ModuleButton m = await ModuleButtonApp.GetAsync(model.Id);
            if (m == null)
            {
                return Operation(false, "数据不存在或已被删除");
            }

            m = MapperHelper.Map<ModuleButtonModel, ModuleButton>(model, m);
            var r = await ModuleButtonApp.UpdateAsync(m);

            return Operation(r.IsSuc, r.Msg);
        }
        #endregion
    }
}