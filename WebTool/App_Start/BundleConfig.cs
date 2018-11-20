using System.Web;
using System.Web.Optimization;

namespace WebTool
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/ie9").Include(
                      "~/Content/bkon/js/html5shiv.min.js",
                      "~/Content/bkon/js/respond.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Content/bkon/js/jquery.min.js",
                      "~/Content/bkon/js/jquery.cookie.js",
                      "~/Content/bkon/js/bootstrap.min.js",
                      "~/Content/bkon/js/jquery.nicescroll.min.js",
                      "~/Content/bkon/js/jquery.noty.packaged.min.js",
                      "~/Content/kendo/js/kendo.all.min.js",
                      "~/Content/kendo/js/kendo.culture.zh-CN.min.js",
                      "~/Content/kendo/js/kendo.messages.zh-CN.min.js",
                      "~/Content/bkon/apps.js",
                      "~/Scripts/site.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bkon/css/bootstrap.min.css",
                      "~/Content/bkon/css/font-awesome.min.css",
                      "~/Content/bkon/css/animate.min.css",
                      "~/Content/kendo/css/kendo.common.min.css",
                      "~/Content/kendo/css/kendo.bootstrap.min.css",
                      "~/Content/bkon/css/admin/reset.css",
                      "~/Content/bkon/css/admin/layout.css",
                      "~/Content/bkon/css/admin/components.css",
                      "~/Content/bkon/css/admin/plugins.css",
                      "~/Content/bkon/themes/ajax.theme.css",
                      "~/Content/bkon/css/admin/custom.css",
                      "~/Content/site.css"));
        }
    }
}
