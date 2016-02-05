namespace Fanex.CustomerSetting.WebApi.App_Start
{
    using Casino.Services;
    using Services.Impl;
    using static ServiceLocation.SettingServiceBootstraper;

    public static class SettingServiceConfig
    {
        public const int AllBet = 21;
        public const int Cricket = 20;
        public const int Cockfighting = 19;
        public const int Sportsbook2 = 16;

        // TODO: WebAPI can store setting in configuraiton file.
        // should use another key for setting service instead of productId to reuse the setting?
        public static void Run()
        {
            //RegisterService(Sportsbook2, typeof(QuerySettingService), typeof(UpdateSettingService));
            //RegisterService(Cockfighting, typeof(QuerySettingService), typeof(UpdateSettingService));
            //RegisterService(Cricket, typeof(QuerySettingService), typeof(UpdateSettingService));
            //RegisterService(AllBet, typeof(QueryCasinoSettingService), typeof(UpdateCasinoSettingService));
        }
    }
}