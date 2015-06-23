namespace Fanex.CustomerSetting.Core.Handlers
{
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Core.Configurations;
    using Fanex.CustomerSetting.Core.Models;

    /// <summary>
    /// Setting context manager for providing
    /// + actor information when edit customer.
    /// + sub account information when logging by sub account in agency system.
    /// + langauge for display text and message in site.
    /// Actor, Sub account, Language will be set when user login.
    /// Language can be set later when user want to change language.
    /// </summary>
    public static class SettingContextManager
    {
        static SettingContextManager()
        {
            var settingContext = new SettingContext();
            settingContext.System = CustomerSettings.Settings.SystemId;
            settingContext.Domain = CustomerSettings.Settings.Domain;

            SettingContext = settingContext;
        }

        public static ISettingContext SettingContext { get; private set; }

        public static void SetActor(User actor, User subAccount)
        {
            Check.Argument.IsNotNull(actor, "actor");
            Check.Argument.IsNotNegativeOrZero(actor.Id, "actor.Id");
            Check.Argument.IsNotNegativeOrZero((byte)actor.Level, "actor.Level");

            SettingContext.Actor = actor;
            SettingContext.SubAccount = subAccount ?? new User();
        }

        public static void SetLanguage(string language)
        {
            Check.Argument.IsNotEmpty(language, "language");

            SettingContext.Language = language;
        }
    }
}