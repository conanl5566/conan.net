using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bootstrap_Table.Controllers
{
    /// <summary>
    /// 表格样式
    /// </summary>
    public class TableStyleController : Controller
    {
        // GET: TableStyle
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult GetOrder(int limit, int offset, string orderno, string statu)
        {
            var lstRes = new List<DTO_ORDER>();
            for (var i = 0; i < 50; i++)
            {
                var oModel = new DTO_ORDER();
                oModel.ORDER_ID = Guid.NewGuid().ToString();
                oModel.ORDER_NO = DateTime.Now.ToString("yyyyMMdd") + i;
                oModel.ORDER_TYPE = "销售车";
                oModel.ORDER_STATUS = i <3 ? "待排产" : (i < 5 ? "已删除" : "正常");
                oModel.REMARK = "暂无备注信息";
                lstRes.Add(oModel);
            }

            var total = lstRes.Count;
            var rows = lstRes.Skip(offset).Take(limit).ToList();
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
        }
    }

    public class DTO_ORDER
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public string ORDER_ID { set; get; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string ORDER_NO { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public string ORDER_TYPE { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public string ORDER_STATUS { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string REMARK { get; set; }



    }
}