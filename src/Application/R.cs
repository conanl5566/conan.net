using dotNET.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotNET.Application
{
   // public enum RState { Err = 0, Suc = 1 }
    public class R :BackUrl
    {

        public static (bool Ok,string Error) Ok()
        {
            return (true, string.Empty);
        }
        public static (bool Ok, string Error) Ko(string err)
        {
            return (false, err);
        }


        // bool isSuc = false;
        public static R Suc(dynamic data = null, string msg = "操作成功")
        {
            return new R { IsSuc = true, Msg = msg, Code = "0001", Data = data };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="code">code为能为0001</param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static R Err(string code = "0000", string msg="操作失败", dynamic data = null)
        {
            if (code == "0001")
                throw new Exception("Code 为能设为0001");

            return new R { Msg = msg, Code = code, Data = data, IsSuc = false };
        }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuc { get; set; }

        /// <summary>
        /// 错误编码 Code=0001 成功，Code=0000 默认错误编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 错误描述
        /// </summary>
        public string Msg { get; set; } 

        /// <summary>
        /// 
        /// </summary>
        public dynamic Data { get; set; }

    }

    public class R<T>
    {
        // bool isSuc = false;
        public static R Suc(T data , string msg = "操作成功")
        {
            return new R { IsSuc = true, Msg = msg, Code = "0001", Data = data };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="code">code为能为0001</param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static R Err(string code = "0000", string msg = "操作失败8787", dynamic data = null)
        {
            if (code == "0001")
                throw new Exception("Code 为能设为0001");

            return new R { Msg = msg, Code = code, Data = data, IsSuc = false };
        }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuc { get; set; }

        /// <summary>
        /// 错误编码 Code=0001 成功，Code=0000 默认错误编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 错误描述
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public T Data { get; set; }
    }
}
