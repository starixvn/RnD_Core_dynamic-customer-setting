namespace Fanex.CustomerSetting.Configurations
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    public static class AppSettings
    {
        public static IEnumerable<string> SitesAllowUncheckStakeLevelOnMember => (ConfigurationManager.AppSettings["SitesAllowUncheckStakeLevelOnMember"] ?? string.Empty).Split(',').Select(site => site.Trim());

        public static IEnumerable<string> SupportPT3DecimalSites => (ConfigurationManager.AppSettings["SupportPT3DecimalSites"] ?? "789y").Split(',').Select(site => site.Trim());

        public static IEnumerable<string> Y7Sites => (ConfigurationManager.AppSettings["Y7Sites"] ?? string.Empty).Split(',').Select(site => site.Trim());

        public static byte SettingStatus => ConfigurationManager.AppSettings["SettingStatus"] == null ? (byte)0 : byte.Parse(ConfigurationManager.AppSettings["SettingStatus"]);

        public static string LoggingDefaultCategory => ConfigurationManager.AppSettings["Fanex.Logging:DefaultCategory"];

        public static string LogLibs => ConfigurationManager.AppSettings["LogLibs"];

        public static string RabbitMQConnectionStrings => ConfigurationManager.AppSettings["Fanex.Logging:RabbitMQConnectionStrings"];

        public static string SentryBackendUrl => ConfigurationManager.AppSettings["Fanex.Logging:SentryBackendUrl"];

        public static string SentryUrl => ConfigurationManager.AppSettings["Fanex.Logging:SentryUrl"];

        public static string RavenLibPath => ConfigurationManager.AppSettings["RavenLibPath"];
    }
}
