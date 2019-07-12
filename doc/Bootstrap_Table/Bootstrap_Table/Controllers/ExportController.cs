using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bootstrap_Table.Controllers
{
    /// <summary>
    /// 表格导出
    /// </summary>
    public class ExportController : Controller
    {
        // GET: Export
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetDepartment(int limit, int offset, string departmentname, string statu)
        {
            var lstRes = new List<Department>();
            for (var i = 0; i < 50; i++)
            {
                var oModel = new Department();
                oModel.ID = Guid.NewGuid().ToString();
                oModel.Name = "销售部" + i;
                oModel.Level = i.ToString();
                oModel.Desc = "暂无描述信息";
                lstRes.Add(oModel);
            }

            return Json(lstRes, JsonRequestBehavior.AllowGet);
        }
    }
}