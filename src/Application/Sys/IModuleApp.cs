using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotNET.Domain.Entities;
using dotNET.Dto;

namespace dotNET.Application.Infrastructure
{
    public interface IModuleApp : IAppService
    {
        Task<R> DeleteAsync(long key);

        Task<Module> GetAsync(long key);

        /// <summary>
        /// Saas模块  
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        Task<List<Module>> GetSaasModuleListAsync(ModuleOption option = null);

        /// <summary>
        /// 代理模块
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        Task<List<Module>> GetAgentModuleListAsync(ModuleOption option = null);

        /// <summary>
        /// 返回菜单目录
        /// </summary>
        /// <param name="isSaas"></param>
        /// <returns></returns>
        Task<List<Module>> GetMenuCatalogListAsync();
        Task<R> CreateAsync(Module moduleEntity);
        Task<R> UpdateAsync(Module moduleEntity);
    }
}