using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotNET.Domain.Entities;
using dotNET.Dto;

namespace dotNET.Application.Infrastructure
{
    public interface IItemsDataApp : IAppService
    {
        Task<R> DeleteAsync(long key);
        Task<ItemsData> GetAsync(long key);
        /// <summary>
        /// 部门列表  
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        Task<List<ItemsData>> GetListAsync(ItemsDataOption option);

        /// <summary>
        /// 返回部门目录
        /// </summary>
        /// <param name="isSaas"></param>
        /// <returns></returns>
        Task<List<ItemsData>> GetItemsDataListAsync();

        Task<R> CreateAsync(ItemsData moduleEntity);

        Task<R> UpdateAsync(ItemsData moduleEntity);
    }
}
