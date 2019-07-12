using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotNET.Domain.Entities;
using dotNET.Dto;

namespace dotNET.Application.Infrastructure
{
    public interface IDepartmentApp : IAppService
    {
        Task<R> DeleteAsync(long key);

        Task<Department> GetAsync(long key);

        /// <summary>
        /// 部门列表  
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        Task<List<Department>> GetListAsync(DepartmentOption option);

        /// <summary>
        /// 返回部门目录
        /// </summary>
        /// <param name="isSaas"></param>
        /// <returns></returns>
        Task<List<Department>> GetDepartmentListAsync();

        Task<R> CreateAsync(Department moduleEntity);

        Task<R> UpdateAsync(Department moduleEntity);
    }
}
