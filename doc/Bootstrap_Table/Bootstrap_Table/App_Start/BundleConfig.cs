using System.Web;
using System.Web.Optimization;

namespace Bootstrap_Table
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //1.jquery
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            //2.bootstrap
            bundles.Add(new StyleBundle("~/Content/css/bootstrap").Include(
                        "~/Content/bootstrap/bootstrap.css"));
            bundles.Add(new ScriptBundle("~/Content/js/bootstrap").Include(
                        "~/Content/bootstrap/bootstrap.js"));

            //3.bootstrap-table
            bundles.Add(new StyleBundle("~/Content/bootstrap-table/css").Include(
                        "~/Content/bootstrap-table/bootstrap-table.css"));
            bundles.Add(new ScriptBundle("~/Content/bootstrap-table/js").Include(
                        "~/Content/bootstrap-table/bootstrap-table.js",
                        "~/Content/bootstrap-table/locale/bootstrap-table-zh-CN.js"));

            //4.页面js
            bundles.Add(new ScriptBundle("~/Script/Home/Index").Include(
                        "~/Scripts/Home/Index.js"));

            bundles.Add(new ScriptBundle("~/Script/Role/Index").Include(
                        "~/Scripts/Role/Index.js"));

            bundles.Add(new ScriptBundle("~/Script/Role/Index2").Include(
                        "~/Scripts/Role/Index2.js"));

            //表格行样式
            bundles.Add(new ScriptBundle("~/Script/TableStyle/Index").Include(
                        "~/Scripts/TableStyle/Index.js"));

            //表格行内编辑
            bundles.Add(new ScriptBundle("~/Script/Editable/Index").Include(
                        "~/Scripts/Editable/Index.js"));

            //表格导出
            bundles.Add(new ScriptBundle("~/Script/Export/Index").Include(
                        "~/Scripts/Export/Index.js"));

            //表格行列合并
            bundles.Add(new ScriptBundle("~/Script/GroupColumns/Index").Include(
                        "~/Scripts/GroupColumns/Index.js"));
        }
    }
}
