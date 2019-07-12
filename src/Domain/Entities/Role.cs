/**************************************************************************
 * 作者：X   
 * 日期：2017.01.18   
 * 描述：
 * 修改记录：    
 * ***********************************************************************/
using dotNET.Dto;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotNET.Domain.Entities
{
    [Table("Roles")]
    public class Role  : Entity, IEntity
    {

        [Description("名称")]
        public string Name { get; set; }

        [Description("允许编辑")]
        public bool? AllowEdit { get; set; } = false;

        [Description("允许删除")]
        public bool? AllowDelete { get; set; } = false;

        [Description("排序")]
        public int? SortCode { get; set; } = 0;

        [Description("描述")]
        public string Description { get; set; }

        [Description("创建时间")]
        public DateTime CreatorTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 生成ID
        /// </summary>
        /// <returns></returns>
     
        public long CreateId()
        {
            return base.CreateId(EntityEnum.Role);
        }
    }
}
