#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotNET.Core;
using dotNET.Core.Security;
using dotNET.Domain.Entities;
using dotNET.Application.Infrastructure;
using dotNET.Dto;
using dotNET.EFCoreRepository;
using Microsoft.EntityFrameworkCore;
using Hangfire;
#endregion

namespace dotNET.Application.App
{
    public class UserApp : IAppService, IUserApp
    {
        #region 注入
        public IBaseRepository<User> UserRep { get; set; }
        public IBaseRepository<Role> RoleRep { get; set; }
        public IBaseRepository<Department> DepartmentRep { get; set; }
        public IOperateLogApp OperateLogApp { get; set; }
        public IAreaListApp AreaListApp { get; set; }
        #endregion

        #region 修改登录状态
        /// <summary>
        /// 修改登录状态
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="agentId"></param>
        /// <param name="curUser"></param>
        /// <returns></returns>
        public async Task<R> updatestatus(long Id, CurrentUser curUser)
        {
            var entry = await GetAsync(Id);
            if (entry == null)
                return R.Err(msg: "该用户不存在");
            int s = 1;
            if (entry.State == 1)
            {
                s = 0;
            }
            await UserRep.UpdateAsync(u => u.Id == Id, u => new User { State = s });
            return R.Suc();

        }
        #endregion

        #region 用户退出操作
        /// <summary>
        /// 用户退出操作
        /// </summary>
        /// <param name="curUser">登录用户信息</param>
        /// <returns></returns>
        public async Task LogOffAsync(CurrentUser curUser)
        {
            await OperateLogApp.CustomLogAsync(curUser, "用户退出", curUser.RealName + "进行了退出操作");
        }
        #endregion

        /// <summary>
        ///  根据账号模糊查询获取列表
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public async Task<List<IdAccountDto>> SelectDataAsync(string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return new List<IdAccountDto>();

            var result = UserRep.Find(o => o.Account.Contains(q.Trim()));

            if (result == null || result.Count() == 0)
            {
                return new List<IdAccountDto>();
            }
            return result.Select(o => new IdAccountDto() { Id = o.Id, Account = o.Account }).ToList();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="rowsPrePage"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<PageResult<UserSunpleDto>> GetPageAsync(int pageNumber, int rowsPrePage, UserOption filter)
        {
            List<UserSunpleDto> data = new List<UserSunpleDto>();
            PageResult<UserSunpleDto> list = new PageResult<UserSunpleDto>();
            string orderby = " id desc";
            var predicate = PredicateBuilder.True<User>();
            predicate = predicate.And(o => o.DeleteMark == null);
            if (!string.IsNullOrWhiteSpace(filter.Account))
            {
                predicate = predicate.And(o => o.Account == filter.Account);
            }
            if (!string.IsNullOrWhiteSpace(filter.RealName))
            {
                predicate = predicate.And(o => o.RealName == filter.RealName);
            }
            var tlist = await UserRep.Find(pageNumber, rowsPrePage, orderby, predicate).ToListAsync() ?? new List<User>();
            data = MapperHelper.MapList<User, UserSunpleDto>(tlist);
            List<long> roleIds = tlist.Select(o => o.RoleId).Distinct().ToList();
            if (roleIds.Count() > 0)
            {
                var roles = await RoleRep.Find(o => roleIds.Contains(o.Id)).ToListAsync();
                foreach (var d in data)
                {
                    var r = roles.FirstOrDefault(o => o.Id == d.RoleId);
                    d.RoleName = r?.Name;
                }
            }
            List<long?> DepartmentIds = tlist.Select(o => o.DepartmentId).Distinct().ToList();
            DepartmentIds.Remove(null);
            if (DepartmentIds.Count() > 0)
            {
                var Departments = await DepartmentRep.Find(o => DepartmentIds.Contains(o.Id)).ToListAsync();
                foreach (var d in data)
                {
                    var r = Departments.FirstOrDefault(o => o.Id == d.DepartmentId);
                    d.deptname = r?.Name;
                }
            }
            list.Data = data.ToList();
            int total = await UserRep.GetCountAsync(predicate);
            list.ItemCount = total;
            return list;
        }


        /// <summary>
        /// 添加管理人员
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<R> InsertAsync(User entity, CurrentUser curUser)
        {
            entity.Id = entity.CreateId();
            entity.UserSecretkey = "";
            entity.CreatorTime = DateTime.Now;
            entity.CreatorUserId = curUser.Id;
            var r = await InsertAsync(entity);
            if (!string.IsNullOrEmpty(r.Error))
            {
                return R.Err(msg: r.Error);
            }
            await OperateLogApp.InsertLogAsync<User>(curUser, "添加用户", entity);
            return R.Suc();
        }

        /// <summary>
        /// 添加（判断Account是否已存在）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>-1： 失败 ， 0：已存在 ，1：成功</returns>
        public async Task<(string Error, User user)> InsertAsync(User entity)
        {
            //已存在
            int count = await UserRep.GetCountAsync(o => o.Account == entity.Account);
            if (count > 0)
            {
                return ("帐号已存在", null);
            }
            await UserRep.AddAsync(entity);

            return (string.Empty, null);
        }


        /// <summary>
        ///  更新用户信息
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="curUser"></param>
        /// <returns></returns>
        public async Task<R> UpdateUserInfoAsync(User entity, CurrentUser curUser)
        {
            await UserRep.UpdateAsync(entity);

            return R.Suc();
        }

        /// <summary>
        /// 删除管理人员
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<R> DeleteAsync(long Id, long agentId, CurrentUser curUser)
        {
            var r = await DeleteAsync(Id, agentId, curUser.Id);
            if (!string.IsNullOrEmpty(r.Error))
            {
                return R.Err(msg: r.Error);
            }
            return R.Suc();
        }

        /// <summary>
        /// 删除（假删除）
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="agentId"></param>
        /// <param name="optId"></param>
        /// <returns>-1： 失败 ， 0：不存在 ，1：成功</returns>
        public async Task<(string Error, User User)> DeleteAsync(long Id, long agentId, long optId)
        {
            User user = await UserRep.FindSingleAsync(o => o.Id == Id);
            if (user == null || user.DeleteMark == true)
            {
                return ("帐号不存在", null);
            }
            user.DeleteMark = true;
            user.DeleteTime = DateTime.Now;
            user.DeleteUserId = optId;
            await UserRep.UpdateAsync(user);

            return (string.Empty, user);
        }

        /// <summary>
        /// Saas后台管理登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<(string Error, User User)> SaasLoginAsync(string account, string password, string ip = "")
        {

            User user = await UserRep.FindSingleAsync(o => o.Account == account);
            if (user == null)
            {
                return ($"帐号不存在", null);
            }
            if (user.State == 0)
            {
                return ($"帐号禁止登录", null);
            }
            if (user.Password != password)
            {
                return ($"密码不正确", null);
            }
            CurrentUser curUser = new CurrentUser
            {
                Id = user.Id,
                RealName = user.Account,
                LoginIPAddress = ip

            };
            await OperateLogApp.CustomLogAsync(curUser, "用户登录", user.RealName + "进行了登录操作");
            await UserRep.UpdateAsync(o => o.Id == user.Id, o => new User() { LastLoginTime = DateTime.Now });
            return (string.Empty, user);
        }

        private Exception Exception()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="password">旧密码</param>
        /// <param name="newPassword">新密码</param>
        /// <returns></returns>
        public async Task<R> ChangePasswordAsync(long Id, string password, string newPassword, long agentId, CurrentUser curUser)
        {
            User user = await UserRep.FindSingleAsync(o => o.Id == Id);
            if (user == null || user.DeleteMark == true)
            {
                return R.Err("1001", $"帐号({Id})不存在");
            }
            if (user.Password != MD5Encrypt.MD5(password))
            {
                return R.Err("1003", $"原密码不正确");
            }
            newPassword = MD5Encrypt.MD5(newPassword);
            await UserRep.UpdateAsync(o => o.Id == Id, o => new User() { Password = newPassword });

            return R.Suc();
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="newPassword"></param>
        /// <param name="agentId"></param>
        /// <returns></returns>
        public async Task<R> ResetPasswordAsync(long Id, string password, long agentId, CurrentUser curUser)
        {
            User user = await UserRep.FindSingleAsync(o => o.Id == Id);
            if (user == null)
            {
                return R.Err("1001", $"帐号({Id})不存在");
            }
            password = MD5Encrypt.MD5(password);
            await UserRep.UpdateAsync(o => o.Id == Id, o => new User() { Password = password });

            return R.Suc();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<User> GetAsync(long Id)
        {
            return await UserRep.FindSingleAsync(o => o.Id == Id);
        }
    }
}