namespace Fanex.CustomerSetting.WebApi.AppStart
{
    using CustomerSetting.Configurations;

    public static class SettingServiceConfig
    {
        public const int RNGSlot = 18;

        public static void Run()
        {
            var configurator = new SettingServiceConfigurator();
            configurator.Verify();
        }
    }
}