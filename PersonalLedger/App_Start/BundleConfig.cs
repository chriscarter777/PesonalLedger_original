using System.Web;
using System.Web.Optimization;

namespace PersonalLedger
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //must load react before react-dom
            bundles.Add(new ScriptBundle("~/bundles/react").Include(
                        "~/Scripts/react_*",
                        "~/Scripts/react-dom_*"));

              bundles.Add(new StyleBundle("~/Content/styles/css").Include(
                      "~/Content/themes/base/*.css.css",
                      "~/Content/styles/bootstrap.css",
                      "~/Content/styles/CSC.css",
                      "~/Content/styles/Site.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
