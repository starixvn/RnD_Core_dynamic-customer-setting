namespace Fanex.CustomerSetting.Utilz
{
    public static class LanguageNameConverter
    {
        private const string DefaultLanguageName = "en";

        public static string GetRefLanguageName(string languageName = DefaultLanguageName)
        {
            if (string.IsNullOrWhiteSpace(languageName))
            {
                return DefaultLanguageName;
            }

            switch (languageName.ToUpperInvariant())
            {
                case "ZH-TW": return "ch"; // Traditional
                case "ZH-CN": return "cs"; // Simplified
                case "JA-JP": return "jp"; // Japanese
                case "TH-TH": return "th"; // Thai
                case "EN-US":
                default: return DefaultLanguageName;
            }
        }
    }
}
