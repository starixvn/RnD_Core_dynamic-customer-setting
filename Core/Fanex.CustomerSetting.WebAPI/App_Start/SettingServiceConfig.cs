namespace Fanex.CustomerSetting.WebApi.AppStart
{
    using Casino.Services.Impl;
    using CustomerSetting.Configurations;

    public static class SettingServiceConfig
    {
        public const int AllBets = 21;
        public const int MacauGames = 22;

        public static void Run()
        {
            var configurator = new SettingServiceConfigurator();
            configurator
                .UseQueryServiceType(typeof(QueryCasinoSettingService))
                .UseUpdateServiceType(typeof(UpdateCasinoSettingService))
                .ForProducts(AllBets, MacauGames)
                .ForAllSystems()
                .Apply();

            configurator.Verify();
        }
    }
}