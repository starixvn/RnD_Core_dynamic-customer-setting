namespace Fanex.CustomerSetting.WebApi.App_Start
{
    using Casino.Services.Impl;
    using CustomerSetting.Configurations;

    public static class SettingServiceConfig
    {
        public const int AllBets = 21;
        public const int Voidbridge = 21;

        public static void Run()
        {
            var configurator = new SettingServiceConfigurator();
            configurator
                .UseQueryServiceType(typeof(QueryCasinoSettingService))
                .UseUpdateServiceType(typeof(UpdateCasinoSettingService))
                .ForProducts(AllBets, Voidbridge)
                .ForAllSystems()
                .Apply();

            configurator.Verify();
        }
    }
}