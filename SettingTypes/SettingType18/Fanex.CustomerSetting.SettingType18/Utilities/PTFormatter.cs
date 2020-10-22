namespace Fanex.CustomerSetting.SettingType18.Utilities
{
    using System.Globalization;

    internal static class PTFormatter
    {
        private const byte PTDivider = 100;
        private static readonly CultureInfo EnglishCultureInfo = CultureInfo.GetCultureInfo("en-US");

        public static string FormatDecimal(object value)
        {
            return string.Format(EnglishCultureInfo.NumberFormat, "{0:#,##0.00}", value);
        }

        public static string FormatPT(decimal value)
        {
            var positionTakingValue = value / PTDivider;
            return FormatDecimal(positionTakingValue);
        }
    }
}