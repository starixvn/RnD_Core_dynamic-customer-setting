namespace Fanex.CustomerSetting.WebApi.App_Start
{
    using Casino.Services.Impl;
    using ServiceLocation;

    public static class SettingServiceConfig
    {
        public const int AllBets = 21;
        
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