namespace Fanex.CustomerSetting.WebApi.AppStart
{
    using Casino.Services.Impl;
    using CustomerSetting.Configurations;
    using NumberGames2.Services.Impl;
    using Services.Impl;

    public static class SettingServiceConfig
    {
        public const int AllBets = 21;
        public const int MacauGames = 22;
        public const int NumberGames2 = 23;
        public const int FishingWorld = 24;

        public static void Run()
        {
            var configurator = new SettingServiceConfigurator();

            configurator
                .UseQueryServiceType(typeof(QueryCasinoSettingService))
                .UseUpdateServiceType(typeof(UpdateCasinoSettingService))
                .ForProducts(AllBets, MacauGames, FishingWorld)
                .ForAllSystems()
                .Apply();

            configurator
                .UseQueryServiceType(typeof(QueryNumberGames2SettingService))
                .UseUpdateServiceType(typeof(UpdateNumberGames2SettingService))
                .ForProducts(NumberGames2)
                .ForAllSystems()
                .Apply();

            configurator.Verify();
        }
    }
}