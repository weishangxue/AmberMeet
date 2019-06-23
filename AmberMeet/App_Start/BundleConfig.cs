using System.Web.Optimization;

namespace AmberMeet
{
    public class BundleConfig
    {
        // 有关捆绑的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bootcss
            bundles.Add(new StyleBundle("~/content/my.boot").Include(
                "~/AppFrontend/bower_components/zui/css/zui.min.css",
                "~/AppFrontend/bower_components/zui/css/zui-theme.min.css",
                "~/AppFrontend/bower_components/zui-docs/css/doc.min.css",
                "~/AppFrontend/css/angel.css"));
            //login css
            bundles.Add(new StyleBundle("~/content/my.login").Include(
                "~/AppFrontend/bower_components/zui/css/zui.css",
                "~/AppFrontend/css/login.style.css"));
            //jquery
            bundles.Add(new ScriptBundle("~/js/my.jquery").Include(
                "~/AppFrontend/bower_components/jquery/jquery.min.js"));
            bundles.Add(new ScriptBundle("~/js/my.jquery.ex").Include(
                "~/AppFrontend/bower_components/jquery/jquery-ui.custom.js",
                "~/AppFrontend/bower_components/jquery/jquery.form.js",
                "~/AppFrontend/bower_components/jquery/jquery.validate.js",
                "~/AppFrontend/bower_components/jquery/jquery.fileupload.js",
                "~/AppFrontend/bower_components/jquery/jquery.iframe-transport.js"));
            //bootui
            bundles.Add(new ScriptBundle("~/js/my.bootui").Include(
                "~/AppFrontend/bower_components/html5shiv/html5shiv.min.js",
                "~/AppFrontend/bower_components/zui/js/zui.js",
                "~/AppFrontend/bower_components/zui-datetimepicker/datetimepicker.min.js",
                "~/AppFrontend/bower_components/prettify/prettify.js"));
            //angel js core
            bundles.Add(new ScriptBundle("~/js/my.jqcustom").Include(
                "~/AppFrontend/angel-js/angel.jquery.custom.js"));
            bundles.Add(new ScriptBundle("~/js/my.core").Include(
                "~/AppFrontend/angel-js/angel.js",
                "~/AppFrontend/angel-js/angel.validate.jquery.custom.js",
                "~/AppFrontend/angel-js/angel.validator.js",
                "~/AppFrontend/angel-js/angel.ajax.js",
                "~/AppFrontend/angel-js/angel.masterpage.js",
                "~/AppFrontend/angel-js/angel.confirmation.js",
                "~/AppFrontend/angel-js/angel.prompt.js"));
            //my.grid
            bundles.Add(new StyleBundle("~/content/my.grid").Include(
                "~/AppFrontend/bower_components/bootstrap-glyphicons/bootstrap.glyphicons.css",
                "~/AppFrontend/bower_components/jqgrid/css/ui.jqgrid-bootstrap4.css"));
            bundles.Add(new ScriptBundle("~/js/my.grid").Include(
                "~/AppFrontend/bower_components/jqgrid/grid.locale-cn.js",
                "~/AppFrontend/bower_components/jqgrid/jquery.jqGrid.min.js",
                "~/AppFrontend/angel-js/angel.grid.js"));
        }
    }
}