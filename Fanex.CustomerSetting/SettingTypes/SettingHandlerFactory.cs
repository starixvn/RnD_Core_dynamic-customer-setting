namespace Fanex.CustomerSetting.SettingTypes
{
    using System;
    using Fanex.CustomerSetting.Enumerations;

    public sealed class SettingHandlerFactory
    {
        public static ISettingHandler Create(SettingType settingType)
        {
            switch (settingType)
            {
                case SettingType.Status:
                    return new StatusHandler();

                case SettingType.Discount:
                    return new DiscountHandler();

                case SettingType.BetLimit:
                    return new BetLimitHandler();

                case SettingType.PositionTaking:
                    return new PositionTakingHandler();

                default:
                    throw new NotImplementedException("Not implemented yet");
            }
        }
    }
}