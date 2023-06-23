namespace Fanex.CustomerSetting.WebApi.AppStart
{
    using Casino.Services.Impl;
    using CustomerSetting.Configurations;

    public static class SettingServiceConfig
    {
        public const int RNGSlot = 18;

        public static void Run()
        {
            var configurator = new SettingServiceConfigurator();

            configurator
                .UseQueryServiceType(typeof(QueryCasinoSettingService))
                .UseUpdateServiceType(typeof(UpdateCasinoSettingService))
                .ForProducts(RNGSlot)
                .ForAllSystems()
                .Apply();

            configurator.Verify();
        }
    }
}