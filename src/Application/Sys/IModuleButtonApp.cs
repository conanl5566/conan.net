using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotNET.Domain.Entities;
using dotNET.Dto;
namespace dotNET.Application.Infrastructure
{
    public interface IModuleButtonApp : IAppService
    {
        /// <summary>
        /// Saas模块按钮  
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        Task<List<ModuleButton>> GetSaasModuleListAsync(ModuleButtonOption option = null);

        /// <summary>
        /// 代理模块按钮
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        Task<List<ModuleButton>> GetAgentModuleListAsync(ModuleButtonOption option = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ModuleButton> GetAsync(long id);

        /// <summary>
        /// 按Id删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<R> DeleteAsync(long id);

        /// <summary>
        /// 按ModuleId删除
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        Task<R> DeleteListAsync(long moduleId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleButton"></param>
        /// <returns></returns>
        Task<R> CreateAsync(ModuleButton moduleButton);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleButton"></param>
        /// <returns></returns>
        Task<R> UpdateAsync(ModuleButton moduleButton);
    }
}