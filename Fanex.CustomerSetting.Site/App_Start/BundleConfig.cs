namespace Fanex.CustomerSetting.Site
{
    using System.Web.Optimization;

    /// <summary>
    /// BundleConfig class.
    /// </summary>
    public static class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/customer-setting").IncludeDirectory("~/app/customerSetting", "*.js", true));
        }
    }
}