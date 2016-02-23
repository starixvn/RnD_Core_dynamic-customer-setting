namespace Fanex.CustomerSetting.WebApi.App_Start
{
    using Casino.Services.Impl;
    using ServiceLocation;
    using static ServiceLocation.SettingServiceConfigurator;

    public static class SettingServiceConfig
    {
        public const int AllBets = 21;

        // TODO: WebAPI can store setting in configuraiton file.
        // should use another key for setting service instead of productId to reuse the setting?
        public static void Run()
        {
            var configurator = new SettingServiceConfigurator();
            configurator
                .UseQueryServiceType(typeof(QueryCasinoSettingService))
                .UseUpdateServiceType(typeof(UpdateCasinoSettingService))
                .ForProducts(AllBets)
                .ForAllSystems()
                .Apply();

            configurator.Verify();
        }
    }
}