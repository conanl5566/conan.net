#region using
using System.Collections.Generic;
using System.Threading.Tasks;
using dotNET.Domain.Entities;
using dotNET.Application.Infrastructure;
using dotNET.EFCoreRepository;
using Microsoft.EntityFrameworkCore;
using Hangfire;
#endregion
namespace dotNET.Application.App
{
    public class AreaListApp : AppService, IAreaListApp
    {
        private readonly IBaseRepository<AreaList> _areaListBaseRepository;
        private readonly IDepartmentApp _departmentApp;
        public AreaListApp(IBaseRepository<AreaList> areaListBaseRepository, IDepartmentApp departmentApp)
        {
            _areaListBaseRepository = areaListBaseRepository;
            _departmentApp = departmentApp;
        }


        [Queue("critical")]
        public async Task TestAsync()
        {
            //  await  Task.Delay(1000);
            //todo
            await _areaListBaseRepository.Find(o => o.AreaType != 3).ToListAsync();
            await _departmentApp.GetDepartmentListAsync();
        }


        public async Task Test2Async()
        {
            // await Task.Delay(1000);
            //todo
            await _areaListBaseRepository.Find(o => o.AreaType != 3).ToListAsync();
            await _departmentApp.GetDepartmentListAsync();
        }

        #region 地区菜单  
        public async Task<List<AreaList>> GetMenuListAsync()
        {
            return await _areaListBaseRepository.Find(o => o.AreaType != 3).ToListAsync();
        }
        #endregion

        #region 列表
        public async Task<List<AreaList>> GetListAsync(long ParentId)
        {
            var predicate = PredicateBuilder.True<AreaList>();
            predicate = predicate.And(o => o.ParentID == ParentId);
            return await _areaListBaseRepository.Find(predicate).ToListAsync();
        }
        #endregion
    }
}