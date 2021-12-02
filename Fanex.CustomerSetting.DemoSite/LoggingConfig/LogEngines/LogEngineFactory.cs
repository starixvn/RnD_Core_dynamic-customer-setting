namespace Fanex.CustomerSetting.DemoSite.LoggingConfig.LogEngines
{
    using Fanex.Logging;

    public interface LogEngineFactory
    {
        ILogEngine Create();
    }
}
