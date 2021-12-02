namespace Fanex.CustomerSetting.DemoSite.LoggingConfig.LogEngines
{
    using Fanex.CustomerSetting.DemoSite.App_Start;
    using Fanex.Logging;
    using Fanex.Logging.Sentry.RavenClient;
    using System.Web;

    public class SentryLogEngine : LogEngineFactory
    {
        public ILogEngine Create()
        {
            var sentryOptions = new SentryEngineOptions
            {
                Dsn = new SharpRaven.Dsn(AppSettings.SentryBackendUrl),
                FrontEndDsn = new SharpRaven.Dsn(AppSettings.SentryUrl)
            };

            return new SentryLogging(
                sentryOptions,
                $"{HttpRuntime.AppDomainAppVirtualPath.TrimEnd(new[] { '/' })}{AppSettings.RavenLibPath}");
        }
    }
}
