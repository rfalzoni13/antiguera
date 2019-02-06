using System.Web.Optimization;

namespace Antiguera.Administrador
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //Scripts
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jqueryvalidate/jquery.validate*"));

            // Use a versão em desenvolvimento do Modernizr para desenvolver e aprender. Em seguida, quando estiver
            // pronto para a produção, utilize a ferramenta de build em https://modernizr.com para escolher somente os testes que precisa.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/Scripts").Include(
                      "~/Scripts/site.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/admin-lte").Include(
                      "~/admin-lte/js/adminlte.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/bootstrap").Include(
                      "~/Scripts/bootstrap/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/bootstrap-datepicker").Include(
                      "~/Scripts/bootstrap-datepicker/bootstrap-datepicker.js"));


            //Styles
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/Site.css"));

            bundles.Add(new StyleBundle("~/Content/css/Login").Include(
                      "~/Content/Login.css"));

            bundles.Add(new StyleBundle("~/bundles/bootstrap").Include(
                      "~/Content/bootstrap/bootstrap.css"));

            bundles.Add(new StyleBundle("~/bundles/admin-lte").Include(
                "~/admin-lte/css/AdminLTE.css",
                "~/Content/font-awesome.css"));

            bundles.Add(new StyleBundle("~/bundles/bootstrap-datepicker").Include(
                      "~/Content/bootstrap-datepicker/bootstrap-datepicker.css"));

            bundles.Add(new StyleBundle("~/bundles/admin-lte/skins").Include(
                "~/admin-lte/css/skins/skin-blue.css",
                "~/admin-lte/css/skins/skin-blue-light.css",
                "~/admin-lte/css/skins/skin-black.css",
                "~/admin-lte/css/skins/skin-black-light.css",
                "~/admin-lte/css/skins/skin-green.css",
                "~/admin-lte/css/skins/skin-green-light.css",
                "~/admin-lte/css/skins/skin-purple.css",
                "~/admin-lte/css/skins/skin-purple-light.css",
                "~/admin-lte/css/skins/skin-red.css",
                "~/admin-lte/css/skins/skin-red-light.css",
                "~/admin-lte/css/skins/skin-yellow.css",
                "~/admin-lte/css/skins/skin-yellow-light.css"));
        }
    }
}