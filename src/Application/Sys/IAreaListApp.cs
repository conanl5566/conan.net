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
    /// 地区
    /// </summary>
    public interface IAreaListApp : IAppService
    {
        Task TestAsync();

        Task Test2Async();

        /// <summary>
        /// 地区菜单
        /// </summary>
        /// <returns></returns>
        Task<List<AreaList>> GetMenuListAsync();

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        Task<List<AreaList>> GetListAsync(long ParentId);
    }
}