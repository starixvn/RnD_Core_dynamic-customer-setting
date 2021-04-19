namespace Fanex.CustomerSetting.LoggingConfig.LogEngines
{
    using Fanex.Logging;

    public interface LogEngineFactory
    {
        ILogEngine Create();
    }
}
