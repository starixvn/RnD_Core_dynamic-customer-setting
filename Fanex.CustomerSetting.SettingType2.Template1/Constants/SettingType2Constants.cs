namespace Fanex.CustomerSetting.SettingType2.Template1.Constants
{
    internal static class SettingType2Constants
    {
        public const byte SettingType = 2;
        public const decimal MaxBetLimitValue = 99999999999;

        public const int MinBetIsGreaterThanMaxBetErrorCode = 7001;
        public const int MaxBetIsGreaterThanMaxPerMatchErrorCode = 7002;
        public const int MinBetIsNegativeErrorCode = 7003;
        public const int MaxBetIsNegativeErrorCode = 7004;
        public const int MaxPerMatchIsNegativeErrorCode = 7005;
        public const int BetLimitExceedMaxValueErrorCode = 7006;
    }
}