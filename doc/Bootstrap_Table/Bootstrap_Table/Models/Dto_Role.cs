using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Bootstrap_Table
{
    [DataContract]
    public class Dto_Role
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        [DataMember]
        public string ROLE_ID { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        [DataMember]
        public string ROLE_NAME { get; set; }
        /// <summary>
        /// 角色描述
        /// </summary>
        [DataMember]
        public string DESCRIPTION { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [DataMember]
         public string CREATETIME { get; set; }
        /// <summary>
        /// 修改日期
        /// </summary>
        [DataMember]
         public string MODIFYTIME { get; set; }
        /// <summary>
        /// 默认页面
        /// </summary>
        [DataMember]
         public string ROLE_DEFAULTURL { get; set; }

        [DataMember]
        public string ROLE_DEFAULTURL_WEB { get; set; }
    }
}