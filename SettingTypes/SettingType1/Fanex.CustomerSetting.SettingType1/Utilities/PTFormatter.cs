namespace Fanex.CustomerSetting.SettingType1.Utilities
{
    internal static class PTFormatter
    {
        private const byte PTDivider = 100;

        public static string FormatPT(decimal value)
        {
            var positionTakingValue = value / PTDivider;
            return positionTakingValue.ToString("G29");
        }
    }
}