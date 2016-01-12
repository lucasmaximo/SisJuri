using System.Web;
using System.Web.Optimization;

namespace Sisjuri
{
    public class BundleConfig
    {
        // Para obter mais informações sobre agrupamento, visite http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       "~/Content/assets/js/jquery-1.11.3.min.js"
                       ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use a versão em desenvolvimento do Modernizr para desenvolver e aprender com ela. Após isso, quando você estiver
            // pronto para produção, use a ferramenta de compilação em http://modernizr.com para selecionar somente os testes que você precisa.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Content/assets/js/analytics2.js",
                      "~/Content/assets/js/jquery-1.11.3.min.js",
                      "~/Content/assets/js/jquery.maskedinput.min.js",
                      "~/Content/assets/js/bootstrap.js",
                      "~/Content/assets/js/jquery-ui.custom.min.js",
                      "~/Content/assets/js/jquery-ui.js",
                      "~/Content/assets/js/fullcalendar.min.js",
                      "~/Content/assets/js/jquery.slimscroll.min.js",
                      "~/Content/assets/js/raphael-min.js",
                      "~/Content/assets/js/morris.min.js",
                      "~/Content/assets/js/moment.min.js",
                      "~/Content/assets/js/daterangepicker.js",
                      "~/Content/assets/js/jquery-jvectormap-1.2.2.min.js",
                      "~/Content/assets/js/jquery-jvectormap-world-merc-en.js",
                      "~/Content/assets/js/gdp-data.js",
                      "~/Content/assets/js/jquery.flot.js",
                      "~/Content/assets/js/jquery.flot.min.js",
                      "~/Content/assets/js/jquery.flot.pie.min.js",
                      "~/Content/assets/js/jquery.flot.stack.min.js",
                      "~/Content/assets/js/jquery.flot.resize.min.js",
                      "~/Content/assets/js/jquery.flot.time.min.js",
                      "~/Content/assets/js/jquery.flot.threshold.js",
                      "~/Content/assets/js/scripts.js",
                      "~/Content/assets/js/jquery.unobtrusive-ajax.js",
                      "~/Content/assets/js/dropzone.js",
                      "~/Content/assets/js/jquery.unobtrusive-ajax.min.js",
                      "~/Content/assets/fancybox/lib/jquery.mousewheel-3.0.6.pack.js",
                      "~/Content/assets/fancybox/source/jquery.fancybox.js",
                      "~/Content/assets/fancybox/source/jquery.fancybox.pack.js",
                      "~/Content/assets/fancybox/source/helpers/jquery.fancybox-buttons.js",
                      "~/Content/assets/fancybox/source/helpers/jquery.fancybox-media.js",
                      "~/Content/assets/fancybox/source/helpers/jquery.fancybox-thumbs.js"
                      //"~/Content/assets/js/jquery.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/assets/css/bootstrap.css",
                      "~/Content/assets/css/font-awesome/css/font-awesome.min.css",
                      "~/Content/assets/css/layout.css",
                      "~/Content/assets/css/elements.css",
                      "~/Content/assets/css/fullcalendar.css",
                      "~/Content/assets/css/fullcalendar.print.css",
                      "~/Content/assets/css/calendar.css",
                      "~/Content/assets/css/morris.css",
                      "~/Content/assets/css/daterangepicker.css",
                      "~/Content/assets/css/jquery-jvectormap-1.2.2.css",
                      "~/Content/assets/css/font-awesome-ie7.css",
                      "~/Content/assets/css/googleapis.css",
                      "~/Content/assets/css/dropzone.css",
                      "~/Content/assets/fancybox/source/jquery.fancybox.css",
                      "~/Content/assets/fancybox/source/helpers/jquery.fancybox-buttons.css",
                      "~/Content/assets/fancybox/source/helpers/jquery.fancybox-thumbs.css"
                      ));
        }
    }
}
