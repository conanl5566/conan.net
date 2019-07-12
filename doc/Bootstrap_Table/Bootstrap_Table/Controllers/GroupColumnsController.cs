using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bootstrap_Table.Controllers
{
    /// <summary>
    /// 表格行列合并
    /// </summary>
    public class GroupColumnsController : Controller
    {
        // GET: GroupColumns
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetReport(int limit, int offset)
        {
            var lstRes = new List<Report>();
            Random rand = new Random();
            for (var i = 0; i < 50; i++)
            {
                var oModel = new Report();
                oModel.JanCount = rand.Next(10, 99);
                oModel.FebCount = rand.Next(10, 99);
                oModel.MarCount = rand.Next(10, 99);
                oModel.AprCount = rand.Next(10, 99);
                oModel.MayCount = rand.Next(10, 99);
                oModel.JunCount = rand.Next(10, 99);
                oModel.JulCount = rand.Next(10, 99); ;
                oModel.AguCount = rand.Next(10, 99);
                oModel.SepCount = rand.Next(10, 99);
                oModel.OctCount = rand.Next(10, 99);
                oModel.NovCount = rand.Next(10, 99); ;
                oModel.DecCount = rand.Next(10, 99);
                oModel.FirstQuarter = oModel.JanCount + oModel.FebCount + oModel.MarCount;
                oModel.SecondQuarter = oModel.AprCount + oModel.MayCount + oModel.JunCount;
                oModel.ThirdQuarter = oModel.JulCount + oModel.AguCount + oModel.SepCount;
                oModel.ForthQuarter = oModel.OctCount + oModel.NovCount + oModel.DecCount;
                oModel.TotalCount = oModel.FirstQuarter + oModel.SecondQuarter + oModel.ThirdQuarter + oModel.ForthQuarter;
                lstRes.Add(oModel);
                
            }

            var total = lstRes.Count;
            var rows = lstRes.Skip(offset).Take(limit).ToList();
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);

          
        }
    }

    public class Report
    {
        public int JanCount { get; set; }

        public int FebCount { get; set; }

        public int MarCount { get; set; }

        public int AprCount { get; set; }

        public int MayCount { get; set; }

        public int JunCount { get; set; }

        public int JulCount { get; set; }

        public int AguCount { get; set; }

        public int SepCount { get; set; }

        public int OctCount { get; set; }

        public int NovCount { get; set; }

        public int DecCount { get; set; }

        public int FirstQuarter { get; set; }

        public int SecondQuarter { get; set; }

        public int ThirdQuarter { get; set; }

        public int ForthQuarter { get; set; }

        public int TotalCount { get; set; }

        
    }
}