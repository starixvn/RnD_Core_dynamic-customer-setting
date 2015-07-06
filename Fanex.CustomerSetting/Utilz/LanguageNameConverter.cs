namespace Fanex.CustomerSetting.Utilz
{
    public static class LanguageNameConverter
    {
        private const string DefaultLanguageName = "en";

        public static string GetRefLanguageName(string languageName)
        {
            if (string.IsNullOrWhiteSpace(languageName))
            {
                return DefaultLanguageName;
            }

            switch (languageName.ToLowerInvariant())
            {
                case "zh-tw": return "ch"; // Traditional
                case "zh-cn": return "cs"; // Simplified
                case "ja-jp": return "jp"; // Japanese
                case "th-th": return "th"; // Thai
                case "en-us":
                default: return DefaultLanguageName;
            }
        }
    }
}
