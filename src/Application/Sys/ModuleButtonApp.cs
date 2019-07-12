using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dotNET.Application.Infrastructure;
using dotNET.Domain.Entities;
using dotNET.Dto;
using dotNET.EFCoreRepository;
using Microsoft.EntityFrameworkCore;

namespace dotNET.Application.App
{
    public class ModuleButtonApp : IAppService, IModuleButtonApp
    {
        #region 注入
        public IBaseRepository<ModuleButton> ModuleButtonRep { get; set; }
        #endregion

        /// <summary>
        /// Saas模块按钮  
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public async Task<List<ModuleButton>> GetSaasModuleListAsync(ModuleButtonOption option = null)
        {
            return await GetListAsync(option);
        }

        /// <summary>
        /// 代理模块按钮
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public async Task<List<ModuleButton>> GetAgentModuleListAsync(ModuleButtonOption option = null)
        {
            return await GetListAsync(option);
        }
        private async Task<List<ModuleButton>> GetListAsync(ModuleButtonOption option)
        {
            var predicate = PredicateBuilder.True<ModuleButton>();


            if (option != null)
            {
                if (option.ModuleId.HasValue && option.ModuleId != 0)
                {
                    predicate = predicate.And(o => o.ModuleId == option.ModuleId.Value);

                }
                if (option.ParentId.HasValue)
                {
                    predicate = predicate.And(o => o.ParentId == option.ParentId.Value);


                }
            }
            return await ModuleButtonRep.Find(predicate).ToListAsync();
        }

        public async Task<ModuleButton> GetAsync(long id)
        {
            return await ModuleButtonRep.FindSingleAsync(o => o.Id == id);
        }

        public async Task<R> DeleteAsync(long id)
        {
            if (await ModuleButtonRep.GetCountAsync(o => o.ParentId == id) > 0)
            {
                return R.Err(msg: "删除失败！操作的对象包含了下级数据。");
            }
            await ModuleButtonRep.DeleteAsync(o => o.Id == id);
            return R.Suc();
        }

        public async Task<R> DeleteListAsync(long moduleId)
        {
            await ModuleButtonRep.DeleteAsync(o => o.ModuleId == moduleId);

            return R.Suc();

        }

        public async Task<R> CreateAsync(ModuleButton moduleButton)
        {
            moduleButton.Id = moduleButton.CreateId();
            moduleButton.CreatorTime = DateTime.Now;
            await ModuleButtonRep.AddAsync(moduleButton);
            return R.Suc();

        }

        public async Task<R> UpdateAsync(ModuleButton moduleButton)
        {
            await ModuleButtonRep.UpdateAsync(moduleButton);
            return R.Suc();

        }
    }
}
