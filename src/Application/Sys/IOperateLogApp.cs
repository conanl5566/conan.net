using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using dotNET.Domain;
using dotNET.Domain.Entities;
using dotNET.Dto;
using dotNET.Core;

namespace dotNET.Application.Infrastructure
{
    public interface IOperateLogApp : IAppService
    {
        /// <summary>
        /// 自定义 日志内容
        /// </summary>
        /// <param name="uinfo"></param>
        /// <param name="tag"></param>
        /// <param name="content"></param>
        Task CustomLogAsync(CurrentUser curUser, string tag, string content);
        /// <summary>
        /// 对象添加 的日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="user"></param>
        /// <param name="tag"></param>
        /// <param name="t"></param>
        /// <param name="ip"></param>
        Task InsertLogAsync<T>(CurrentUser curUser, string tag, T t) where T : class, new();
        /// <summary>
        /// 对象删除 的日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="user"></param>
        /// <param name="tag"></param>
        /// <param name="t"></param>
        /// <param name="ip"></param>
        Task RemoveLogAsync<T>(CurrentUser curUser, string tag, T t) where T : class, new();
        /// <summary>
        /// 对象编辑 的日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="user"></param>
        /// <param name="tag"></param>
        /// <param name="ip"></param>
        /// <param name="before"></param>
        /// <param name="after"></param>
        Task EditLogAsync<T>(CurrentUser curUser, string tag, T before, T after) where T : class, new();
        /// <summary>
        /// 对象复制
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="RealObject"></param>
        /// <returns></returns>
        T Clone<T>(T RealObject) where T : class, new();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        Task<Page<OperateLogDto>> PagerAsync(OperateLogOption option);

        Task<PageResult<OperateLogDto>> GetPageAsync(int pageNumber, int rowsPrePage, OperateLogOption filter);
    }
}
