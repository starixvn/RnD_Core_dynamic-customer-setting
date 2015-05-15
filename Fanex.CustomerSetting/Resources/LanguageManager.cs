namespace Fanex.CustomerSetting.Resources
{
    using Fanex.CustomerSetting.Core;

    public static class LanguageManager
    {
        static LanguageManager()
        {
            CurrentLanguage = Configuration.DefaultLanguage;
        }

        public static string CurrentLanguage { get; set; }
    }
}