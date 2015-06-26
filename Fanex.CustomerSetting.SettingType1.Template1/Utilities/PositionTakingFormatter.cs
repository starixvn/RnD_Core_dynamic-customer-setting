namespace Fanex.CustomerSetting.SettingType1.Template1.Utilities
{
    using System.Globalization;

    internal static class PositionTakingFormatter
    {
        private const byte PositionTakingDivider = 100;
        private static readonly CultureInfo EnglishCultureInfo = CultureInfo.GetCultureInfo("en-US");

        public static string FormatDecimal(object value)
        {
            return string.Format(EnglishCultureInfo.NumberFormat, "{0:#,##0.00}", value);
        }

        public static string FormatPositionTaking(byte value)
        {
            var positionTakingValue = (float)value / PositionTakingDivider;
            return FormatDecimal(positionTakingValue);
        }
    }
}