using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bootstrap_Table.Controllers
{
    public class RoleController : Controller
    {
        // GET: Role
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Index2()
        {
            return View();
        }

        public JsonResult Get(int limit, int offset)
        {
            var lstRole = new List<Dto_Role>();
            for (var i = 0; i < 20; i++)
            {
                var oModel = new Dto_Role();
                oModel.ROLE_ID = Guid.NewGuid().ToString();
                oModel.ROLE_NAME = "模块管理员" + i;
                oModel.DESCRIPTION = "某一个模块的管理员" + i ;
                oModel.CREATETIME = DateTime.Now.ToString();
                oModel.MODIFYTIME = DateTime.Now.ToString();
                oModel.ROLE_DEFAULTURL = "/Home/Index";
                lstRole.Add(oModel);
            }

            return Json(lstRole, JsonRequestBehavior.AllowGet);
        }

        //Table查询分页方法
        [HttpGet]
        public JsonResult GetRole(int limit, int offset, string filter)
        {
            if (!string.IsNullOrEmpty(filter))
            {
                var oJson = Newtonsoft.Json.JsonConvert.DeserializeObject<Dto_Role>(filter);
            }
            var lstRole = new List<Dto_Role>();
            for (var i = 0; i < 20; i++)
            {
                var oModel = new Dto_Role();
                oModel.ROLE_ID = Guid.NewGuid().ToString();
                oModel.ROLE_NAME = "模块管理员" + i ;
                oModel.DESCRIPTION = "某一个模块的管理员" + i ;
                oModel.CREATETIME = DateTime.Now.ToString();
                oModel.MODIFYTIME = DateTime.Now.ToString();
                oModel.ROLE_DEFAULTURL = "/Home/Index";
                lstRole.Add(oModel);
            }
            var total = lstRole.Count;
            var rows = lstRole.Skip(offset).Take(limit).ToList();
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
        }

        //取当前角色的菜单
        public object GetMenuRole(string strRoleId)
        {
            //var lstMenuRole = MenuRoleManager.Find(x => x.ROLE_ID == strRoleId && x.ROLE_TYPE == "menu");
            //return new { MenuRole = lstMenuRole };
            return new { };
        }

        //编辑角色信息
        [HttpPost]
        public object GetEdit(string strPostData)
        {
            //var oRole = Newtonsoft.Json.JsonConvert.DeserializeObject<DTO_TR_SYS_ROLE>(strPostData);
            //if (string.IsNullOrEmpty(oRole.ROLE_ID))
            //{
            //    oRole = RoleManager.Add(oRole);
            //}
            //else
            //{
            //    RoleManager.Update(oRole);
            //}
            //return oRole;
            return new { };
        }

        //删除角色
        [HttpPost]
        public object Delete(string strPostData)
        {
            //var lstRole = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DTO_TR_SYS_ROLE>>(strPostData);
            //RoleManager.Delete(lstRole);
            return new object();
        }

        //设置角色权限
        public object PowerSet(string jdata)
        {
            //dynamic dData = jdata;
            //var strJson = Convert.ToString(dData.str_Selected_MenuId.Value);
            //var strRoleID = Convert.ToString(dData.strRoleID.Value);
            //var arrMenuIds = strJson.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            ////var lstSelectedMenu = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DTO_TR_SYS_MENU>>(strJson);

            //MenuManager.UpdateRoleMenu(strRoleID, arrMenuIds);

            return new object();
        }

        public object GetParentMenu()
        {


            //var lstMenu = MenuManager.Find(x => x.MENU_LEVEL == "1");

            //var lstRes = RoleManager.Find().ToList();
            //var oRes = new PageRowData();
            //oRes.rows = lstRes.Skip(offset).Take(limit).ToList();
            //oRes.total = lstRes.Count;
            return new { }; ;
        }

        public object GetChildrenMenu(string strParentID)
        {
            //var lstMenu = MenuManager.Find().Where(x => x.PARENT_ID == strParentID).ToList();

            //var lstRes = RoleManager.Find().ToList();
            //var oRes = new PageRowData();
            //oRes.rows = lstRes.Skip(offset).Take(limit).ToList();
            //oRes.total = lstRes.Count;
            return new { }; ; ;
        }

        
    }
}