namespace Fanex.CustomerSetting.LoggingConfig.LogEngines
{
    using Fanex.CustomerSetting.Configurations;
    using Fanex.Logging;
    using Fanex.Logging.RabbitMQ;

    public class RabbitLogEngine : LogEngineFactory
    {
        public ILogEngine Create()
            => new RabbitMQLogging(AppSettings.RabbitMQConnectionStrings);
    }
}
