using dotNET.Domain.Entities;
using dotNET.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace dotNET.Application.Infrastructure
{
    public interface IRoleAuthorizeApp : IAppService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<R> CreateAsync(RoleAuthorize entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        Task CreateAsync(List<RoleAuthorize> entitys);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task DeleteAsync(long Id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        Task DeleteAsync(IEnumerable<long> Ids);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ObjectId"></param>
        /// <param name="ObjectType">对象分类 1-角色 2-部门 3-用户</param>
        /// <returns></returns>
        Task<List<RoleAuthorize>> GetListAsync(long ObjectId, int ObjectType);
        Task<List<Module>> GetModuleList(long roleId, bool bSys);
        Task<List<ModuleButton>> GetButtonList(long roleId, bool bSys);
        Task<bool> ActionValidate(long roleId, string action);
    }
}