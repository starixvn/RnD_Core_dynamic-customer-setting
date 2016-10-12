namespace Fanex.CustomerSetting.SettingType12.Utilities
{
    using System.Globalization;

    internal static class NumberFormatter
    {
        private const string DecimalFormat = "{0:#,##0.00}";
        private static readonly CultureInfo EnglishCultureInfo = CultureInfo.GetCultureInfo("en-US");

        public static string FormatDecimal(object value)
        {
            return string.Format(EnglishCultureInfo.NumberFormat, DecimalFormat, value);
        }
    }
}