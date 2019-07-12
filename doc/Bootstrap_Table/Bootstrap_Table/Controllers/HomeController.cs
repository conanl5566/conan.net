using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bootstrap_Table.Controllers
{
    public class HomeController : Controller
    {
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
                oModel.Name = "销售部" + i ;
                oModel.Level = i.ToString();
                oModel.Desc = "暂无描述信息";
                lstRes.Add(oModel);
            }

            var total = lstRes.Count;
            var rows = lstRes.Skip(offset).Take(limit).ToList();
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
        }

    }
}