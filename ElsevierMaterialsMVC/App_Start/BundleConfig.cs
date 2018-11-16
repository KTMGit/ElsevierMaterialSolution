using System.Web;
using System.Web.Optimization;

namespace ElsevierMaterialsMVC
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                  "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.unobtrusive*", "~/Scripts/jquery.sorttable.js"
                      ));
           
            
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                     "~/Content/fonts.css",
                     "~/Content/Layout.css",
                     "~/Content/Skin.css",
                     "~/Content/imagesD3.css"   
                ));


            //bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));


            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"
                        ));

            bundles.Add(new ScriptBundle("~/commonScripts").Include("~/Scripts/common.js",
                "~/Scripts/Search/search.js",
                  "~/Scripts/MaterialDetails/materialDetails.js",
                   "~/Scripts/Search/advSearch.js",
                   "~/Scripts/Comparison/D3/d3min.js",
                   "~/Scripts/Comparison/D3/RadarChart.js",
                   "~/Scripts/Comparison/D3/d3-transform.js"
                ));

            bundles.Add(new ScriptBundle("~/jsTree").Include("~/Scripts/dist/jstree.min.js"));
            bundles.Add(new StyleBundle("~/jsTreeCss").Include("~/Scripts/dist/themes/default/style.min.css"));

            bundles.Add(new ScriptBundle("~/Scripts/mtree").Include("~/Scripts/mtree/mtree.js"));
            bundles.Add(new StyleBundle("~/Scripts/mtreecss").Include("~/Scripts/mtree/mtree.css"));

        }
    }
}