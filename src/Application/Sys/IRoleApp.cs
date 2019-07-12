#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotNET.Domain.Entities;
using dotNET.Core;
using dotNET.Dto;
#endregion

namespace dotNET.Application.Infrastructure
{
    /// <summary>
    /// 角色
    /// </summary>
    public interface IRoleApp : IAppService
    {

        #region 添加Role
        /// <summary>
        /// 添加Role
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="optId">操作者</param>
        /// <returns></returns>
        Task<R> CreateAsync(Role entity, List<long> permissionIds, CurrentUser currentUser);

        #endregion

        #region 修改
        /// <summary>
        ///  修改
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="CurrentUser"></param>
        /// <returns></returns>
        Task<R> UpdateAsync(Role entity, List<long> permissionIds, CurrentUser CurrentUser);

        #endregion

        #region 删除Role
        /// <summary>
        /// 删除Role
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="optId">操作者Id</param>
        /// <returns></returns>
        Task<(bool s, string msg)> DeleteAsync(long Id, CurrentUser currentUser);

        #endregion

        #region 分页
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        Task<IEnumerable<Role>> GetListAsync(RoleOption option);
        Task<PageResult<Role>> GetPageAsync(int pageNumber, int rowsPrePage, RoleOption filter);


        #endregion

        #region 名称是否存在
        /// <summary>
        /// 名称是否存在
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<bool> CheckCode(string Name, long Id);

        #endregion

        #region 获取
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<Role> GetAsync(long Id);
        #endregion

    }
}