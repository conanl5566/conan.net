using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bootstrap_Table.Controllers
{
    /// <summary>
    /// 表格行编辑
    /// </summary>
    public class EditableController : Controller
    {
        // GET: Editable
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

            var total = lstRes.Count;
            var rows = lstRes.Skip(offset).Take(limit).ToList();
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Edit(string strJson)
        {
            //反序列化之后更新

            return Json(new { }, JsonRequestBehavior.AllowGet);
        }
    }
}