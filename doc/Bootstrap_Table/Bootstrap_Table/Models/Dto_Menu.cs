using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Bootstrap_Table
{
    [DataContract]
    public class DTO_MENU
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        [DataMember]
        public string MENU_ID { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        [DataMember]
         public string MENU_NAME { get; set; }
        /// <summary>
        /// 菜单URL
        /// </summary>
        [DataMember]
        public string MENU_URL { get; set; }
        /// <summary>
        /// 父级菜单
        /// </summary>
        [DataMember]
        public string PARENT_ID { get; set; }
        /// <summary>
        /// 菜单级别
        /// </summary>
        [DataMember]
        public string MENU_LEVEL { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [DataMember]
        public string SORT_ORDER { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [DataMember]
        public string STATUS { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
         public string REMARK { get; set; }
        /// <summary>
        /// 菜单图标名称
        /// </summary>
        [DataMember]
        public string MENU_ICO { get; set; }

        [DataMember]
        public string MENU_URL_WEB { get; set; }

        [DataMember]
        public string MENU_ICO_WEB { get; set; }
    }
}