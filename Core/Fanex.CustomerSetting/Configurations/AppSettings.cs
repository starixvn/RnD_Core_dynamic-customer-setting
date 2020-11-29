namespace Fanex.CustomerSetting.Configurations
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    public static class AppSettings
    {
        public static IEnumerable<string> SupportPT3DecimalSites => ConfigurationManager.AppSettings["SupportPT3DecimalSites"].Split(',').Select(site =>site.Trim());

        public static IEnumerable<string> Y7Sites => ConfigurationManager.AppSettings["Y7Sites"].Split(',').Select(site => site.Trim());
    }
}
