/**************************************************************************
 * 作者：X   
 * 日期：2017.01.19   
 * 描述：Entity
 * 修改记录：    
 * ***********************************************************************/

using Snowflake.Net.Core;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using dotNET.Dto;

namespace dotNET.Domain
{
    public abstract class Entity
    {
        static IdWorker idWorker = null;

        static DateTime sdt = DateTime.Today;

        /// <summary>
        /// 主键
        /// </summary>
        [Description("主键Id")]
        [Key, Required]
        public virtual long Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datacenterId"></param>
        /// <returns></returns>
        protected long CreateId(EntityEnum datacenterId)
        {
            if (idWorker == null)
            {
                idWorker = new IdWorker(GetWorkerId(), (int)datacenterId);
                sdt = DateTime.Today;
            }
            else if (sdt < DateTime.Now.AddDays(-1))
            {
                idWorker = new IdWorker(GetWorkerId(), (int)datacenterId);
                sdt = DateTime.Today;
            }

            return idWorker.NextId();
        }

        long GetWorkerId()
        {
            var config = new ConfigurationBuilder().Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true }).Build();
            long workerId = config.GetValue<long>("Data:WorkerId");

            if (workerId > 31)
                return 31;
            if (workerId < 0)
                return 0;

            return workerId;
        }
    }
}
