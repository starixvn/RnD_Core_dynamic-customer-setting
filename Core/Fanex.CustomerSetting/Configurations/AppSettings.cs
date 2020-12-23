namespace Fanex.CustomerSetting.Configurations
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    public static class AppSettings
    {
        public static IEnumerable<string> SupportPT3DecimalSites => (ConfigurationManager.AppSettings["SupportPT3DecimalSites"] ?? "789y").Split(',').Select(site => site.Trim());

        public static IEnumerable<string> Y7Sites => ConfigurationManager.AppSettings["Y7Sites"].Split(',').Select(site => site.Trim());

        public static byte SettingStatus => ConfigurationManager.AppSettings["SettingStatus"] == null ? (byte)0 : byte.Parse(ConfigurationManager.AppSettings["SettingStatus"]);
    }
}
