namespace Fanex.CustomerSetting.DemoSite
{
    using System.Web.Optimization;

    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/Libraries/Modernizr/modernizr-2.8.3.js"));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                        "~/Scripts/Libraries/AngularJs/angular.js",
                        "~/Scripts/DemoForm/demo-form.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/site.css"));
        }
    }
}