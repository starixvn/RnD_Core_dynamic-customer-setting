namespace Fanex.CustomerSetting.Web.Contexts
{
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Configurations;
    using Fanex.CustomerSetting.Models;

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
        private const int AgentSystem = 1;

        static SettingContextManager()
        {
            SettingContext = new SettingContext();
        }

        public static ISettingContext SettingContext { get; private set; }

        // should be removed, outside code does not need to know what is User
        public static void SetActor(User actor)
        {
            Check.Argument.IsNotNull(actor, "actor");
            Check.Argument.IsNotNegativeOrZero(actor.Id, "actor.Id");
            Check.Argument.IsNotNegativeOrZero((byte)actor.Level, "actor.Level");

            SettingContext.Actor = actor;
        }

        /// <summary>
        /// add some line to make developer clear how this function work
        /// </summary>
        /// <param name="loginUserId"></param>
        /// <param name="loginUserName"></param>
        /// <param name="loginUserLevel"></param>
        /// <param name="subAccountId"></param>
        public static void Init(int loginUserId, int loginUserName, byte loginUserLevel, int subAccountId = 0)
        {

            if (subAccountId > 0 && CustomerSettings.Settings.SystemId != AgentSystem)
                throw new System.ArgumentException("Ricky pls select approriated exception messsage");

            // check validation of paramaters

            var actor = new User { 
                Id = loginUserId, 
                Name = loginUserName, 
                Level = loginUserLevel, 
                SubAccountId = subAccountId };

            // set actor to setting context
        }

        // TODO: consider removing it, pass language via action paramater
        public static void SetLanguage(string language)
        {
            Check.Argument.IsNotEmpty(language, "language");

            SettingContext.Language = language;
        }
    }
}