using System.Configuration;

namespace Fanex.CustomerSetting.DemoSite.App_Start
{
    public static class AppSettings
    {

        public static string LoggingDefaultCategory => ConfigurationManager.AppSettings["Fanex.Logging:DefaultCategory"];

        public static string LogLibs => ConfigurationManager.AppSettings["LogLibs"];

        public static string RabbitMQConnectionStrings => ConfigurationManager.AppSettings["Fanex.Logging:RabbitMQConnectionStrings"];

        public static string SentryBackendUrl => ConfigurationManager.AppSettings["Fanex.Logging:SentryBackendUrl"];

        public static string SentryUrl => ConfigurationManager.AppSettings["Fanex.Logging:SentryUrl"];

        public static string RavenLibPath => ConfigurationManager.AppSettings["RavenLibPath"];
    }
}
