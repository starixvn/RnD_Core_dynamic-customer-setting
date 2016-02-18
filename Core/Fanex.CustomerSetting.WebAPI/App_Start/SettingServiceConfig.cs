namespace Fanex.CustomerSetting.WebApi.App_Start
{
    using Casino.Services.Impl;
    using ServiceLocation;
    using static ServiceLocation.SettingServiceBootstraper;

    public static class SettingServiceConfig
    {
        public const int AllBet = 21;

        // TODO: WebAPI can store setting in configuraiton file.
        // should use another key for setting service instead of productId to reuse the setting?
        public static void Run()
        {
            var casinoServiceConfig = new SettingServiceConfiguration
            {
                ProductId = AllBet,
                QueryServiceType = typeof(QueryCasinoSettingService),
                UpdateServiceType = typeof(UpdateCasinoSettingService)
            };

            RegisterService(casinoServiceConfig);
        }
    }
}