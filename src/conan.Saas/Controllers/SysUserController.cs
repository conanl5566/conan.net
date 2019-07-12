#region using
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using conan.Saas.Framework;
using dotNET.Application.Infrastructure;
using dotNET.Dto;
using conan.Saas.Model;
using dotNET.Core;
using dotNET.Domain.Entities;
using conan.Saas.Web.Model;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Rendering;

#endregion
namespace conan.Saas.Controllers
{
    public class SysUserController : CustomController
    {

        public IUserApp UserApp { get; set; }
        public IRoleApp RoleApp { get; set; }
        public IRoleAuthorizeApp RoleAuthorizeApp { get; set; }
        public SiteConfig Config;
        public IDepartmentApp DepartmentApp { get; set; }
        public SysUserController(IOptions<SiteConfig> option)
        {
            Config = option.Value;
            DefaultPageSize = ZConvert.StrToInt(Config.Configlist.FirstOrDefault(o => o.Key == "pagesize").Values);

        }

        /// <summary>
        /// 权限菜单
        /// </summary>
        /// <returns></returns>
        [IgnoreAuthorize]
        [HttpGet]
        public async Task<ActionResult> GetClientsDataJson()
        {
            List<RoleAuthorize> RoleAuthorizelist = new List<RoleAuthorize>();
            if (CurrentUser.IsSys)
                return Json(new { IsSucceeded = true, Message = "超级管理员" });
            var list = await RoleAuthorizeApp.GetListAsync(CurrentUser.RoleId, 1);

            return Json(new { IsSucceeded = false, Message = list });

        }

        #region Index
        // GET: /<controller>/
        public async Task<IActionResult> Index(UserOption filter, int? page)
        {

            ViewBag.filter = filter;
            var currentPageNum = page.HasValue ? page.Value : 1;
            var result = await UserApp.GetPageAsync(currentPageNum, DefaultPageSize, filter);
            var model = new BaseListViewModel<UserSunpleDto>();
            model.list = result.Data;
            model.Paging.CurrentPage = currentPageNum;
            model.Paging.ItemsPerPage = DefaultPageSize;
            model.Paging.TotalItems = result.ItemCount;
            return View(model);
        }
        #endregion

        #region Create
        public async Task<IActionResult> Create()
        {
            UserCreateModel model = new UserCreateModel();
            var data = await RoleApp.GetListAsync(new RoleOption { });
            List<SelectListItem> selects = data.Select(o => new SelectListItem { Value = o.Id.ToString(), Text = o.Name }).ToList();
            model.Rolelist = selects;
            model.GoBackUrl = SetingBackUrl(this.HttpContext.Request);
            var deptdata = await DepartmentApp.GetDepartmentListAsync();
            deptdata = deptdata.SortDepartmentsForTree().ToList();
            var selectList = new List<SelectListItem>();
            foreach (var c in deptdata)
                selectList.Add(new SelectListItem()
                {
                    Value = c.Id.ToString(),
                    Text = c.GetFormattedBreadCrumb(deptdata),
                    Selected = c.Id == model.DepartmentId
                });

            model.deptlist = selectList;

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Create(UserCreateModel model)
        {
            if (!ModelState.IsValid)
            {

                return Operation(false, "数据验证失败;" + GetErrorFromModelStateStr(), model.GoBackUrl);
            }

            User user = MapperHelper.Map<UserCreateModel, User>(model);
            user.Password = dotNET.Core.Security.MD5Encrypt.MD5(user.Password);

            if (!string.IsNullOrWhiteSpace(user.Avatar))
            {
                var saveUrl = IMGOperate.BaseSave(ImagePathType.员工头像, user.Avatar);
                user.Avatar = saveUrl;
            }

            var r = await UserApp.InsertAsync(user, CurrentUser);

            return Operation(r.IsSuc, r.IsSuc ? "数据添加成功" : r.Msg, model.GoBackUrl);
        }

        #endregion

        #region 删除
        [HttpPost]
        public async Task<IActionResult> Delete(long Id)
        {
            var r = await UserApp.DeleteAsync(Id, 0, CurrentUser);
            return Operation(r.IsSuc, r.IsSuc ? "数据删除成功" : r.Msg);
        }


        /// <summary>
        /// 修改用户登录状态
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> updatestatus(long Id)
        {
            var r = await UserApp.updatestatus(Id, CurrentUser);
            return Operation(r.IsSuc, r.IsSuc ? "数据修改成功" : r.Msg);
        }
        #endregion

        #region 修改
        public async Task<IActionResult> Edit(long Id)
        {
            UserEditModel model = new UserEditModel();
            User user = await UserApp.GetAsync(Id);
            if (user == null)
            {
                return NotFind();
            }
            model = MapperHelper.Map<User, UserEditModel>(user);
            model.GoBackUrl = SetingBackUrl(this.HttpContext.Request);
            var data = await RoleApp.GetListAsync(new RoleOption { });
            List<SelectListItem> selects = data.Select(o => new SelectListItem { Value = o.Id.ToString(), Text = o.Name }).ToList();
            model.Rolelist = selects;

            var deptdata = await DepartmentApp.GetDepartmentListAsync();
            deptdata = deptdata.SortDepartmentsForTree().ToList();
            var selectList = new List<SelectListItem>();

            foreach (var c in deptdata)
                selectList.Add(new SelectListItem()
                {
                    Value = c.Id.ToString(),
                    Text = c.GetFormattedBreadCrumb(deptdata),
                    Selected = c.Id == model.DepartmentId
                });

            model.deptlist = selectList;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return Operation(false, GetErrorFromModelStateStr());
            }

            if (!string.IsNullOrWhiteSpace(model.Avatar))
            {
                if (model.Avatar.Contains("["))
                {
                    var saveUrl = IMGOperate.BaseSave(ImagePathType.员工头像, model.Avatar);
                    model.Avatar = saveUrl;
                }

            }

            User m = await UserApp.GetAsync(model.Id);

            if (m == null)
            {
                return Operation(false, "数据不存在或已被删除");
            }

            m = MapperHelper.Map<UserEditModel, User>(model, m);

            var r = await UserApp.UpdateUserInfoAsync(m, CurrentUser);
            return Operation(r.IsSuc, r.IsSuc ? "数据编辑成功" : r.Msg, model.GoBackUrl);
        }
        #endregion

        #region ChangePassword
        [IgnoreAuthorize]
        public IActionResult ChangePassword()
        {
            ViewBag.RealName = CurrentUser.RealName;
            return View();
        }

        [HttpPost]
        [IgnoreAuthorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return Operation(false, GetErrorFromModelStateStr());
            }
            var r = await UserApp.ChangePasswordAsync(CurrentUser.Id, model.Password, model.NewPassword, 0, CurrentUser);

            return Operation(r.IsSuc, r.IsSuc ? "密码修改成功" : r.Msg);
        }


        [HttpPost]
        //[IgnoreAuthorize]
        public async Task<IActionResult> resertPassword(string Id)
        {
            var r = await UserApp.  //ChangePasswordAsync(CurrentUser.Id, model.Password, model.NewPassword, 0, CurrentUser);
            ResetPasswordAsync(ZConvert.StrToLong(Id), "123456", 0, CurrentUser);
            return Operation(r.IsSuc, r.IsSuc ? "重置密码成功" : r.Msg);
        }

        #endregion
    }
}