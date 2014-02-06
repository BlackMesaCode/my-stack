using System.Web.Optimization;
using BundleTransformer.Core.Orderers;
using BundleTransformer.Core.Transformers;

namespace BlackMesa.MyStack.Main.App_Start
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {

            var cssTransformer = new CssTransformer();
            var jsTransformer = new JsTransformer();
            var cssMinifier = new CssMinify();
            var jsMinifier = new JsMinify();
            var nullOrderer = new NullOrderer();

            // Script Bundles

//            const string jQueryCdnPath = "http://code.jquery.com/jquery-1.9.1.min.js";

            bundles.Add(new ScriptBundle("~/bundles/global")
                .Include("~/Content/custom/scripts/global.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin")
                //.Include("~/Content/scripts/jquery-autogrow-textarea.js")
                .Include("~/Content/scripts/ace/ace.js")
                .Include("~/Content/scripts/ace/theme-clouds.js")
                .Include("~/Content/scripts/ace/mode-html.js"));
            

            // Style Bundles

            var mainBundle = new Bundle("~/Content/global")
                .Include("~/Content/font-awesome/css/font-awesome.min.css")
                .Include("~/Content/custom/less/custom.less")
                .Include("~/Content/custom/less/shCoreDefault.less")
                .Include("~/Content/custom/less/shThemeDefault.less");
                
            mainBundle.Transforms.Add(cssTransformer);
            mainBundle.Transforms.Add(cssMinifier);
            mainBundle.Orderer = nullOrderer;
            bundles.Add(mainBundle);

            var adminBundle = new Bundle("~/Content/admin");
                
            adminBundle.Transforms.Add(cssTransformer);
            mainBundle.Transforms.Add(cssMinifier);
            adminBundle.Orderer = nullOrderer;
            bundles.Add(adminBundle);


            //BundleTable.EnableOptimizations = true;  // executing this line will force bundling and minification by overwriting whatever stands in web.config
//            #if DEBUG
//                BundleTable.EnableOptimizations = false;
//            #endif

        }
    }
}