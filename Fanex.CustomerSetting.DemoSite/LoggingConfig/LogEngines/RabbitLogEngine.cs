namespace Fanex.CustomerSetting.DemoSite.LoggingConfig.LogEngines
{
    using Fanex.CustomerSetting.DemoSite.App_Start;
    using Fanex.Logging;
    using Fanex.Logging.RabbitMQ;

    public class RabbitLogEngine : LogEngineFactory
    {
        public ILogEngine Create()
            => new RabbitMQLogging(AppSettings.RabbitMQConnectionStrings);
    }
}
