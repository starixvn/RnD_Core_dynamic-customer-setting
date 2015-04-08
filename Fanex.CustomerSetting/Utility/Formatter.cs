namespace Fanex.CustomerSetting.Utility
{
    using System.Globalization;
    using Fanex.CustomerSetting.Core;

    /// <summary>
    /// Formatter class.
    /// </summary>
    public static class Formatter
    {
        private static readonly CultureInfo EnglishCultureInfo = CultureInfo.GetCultureInfo("en-US");

        public static string FormatDecimal(object value)
        {
            return string.Format(EnglishCultureInfo.NumberFormat, "{0:#,##0.00}", value);
        }

        public static string FormatPositionTaking(byte value)
        {
            var positionTakingValue = (float)value / Configuration.PositionTakingDivider;
            return FormatDecimal(positionTakingValue);
        }
    }
}