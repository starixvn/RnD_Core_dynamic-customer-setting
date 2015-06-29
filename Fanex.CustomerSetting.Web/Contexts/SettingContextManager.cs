namespace Fanex.CustomerSetting.Web.Contexts
{
    using System;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Configurations;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resource.Resources;

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

        /// <summary>
        /// Add some line to make developer clear how this function work.
        /// </summary>
        /// <param name="loginUserId"></param>
        /// <param name="loginUserName"></param>
        /// <param name="loginUserLevel"></param>
        /// <param name="subAccountId"></param>
        public static void Init(int loginUserId, string loginUserName, UserLevel loginUserLevel, int subAccountId = 0)
        {
            Check.Argument.IsNotNegativeOrZero(loginUserId, "loginUserId");
            Check.Argument.IsNotEmpty(loginUserName, "loginUserName");
            Check.Argument.IsNotNegativeOrZero((byte)loginUserLevel, "loginUserLevel");

            ValidateSubAccountWithAgencySystem(subAccountId);

            SettingContext.Actor = new User
            {
                Id = loginUserId,
                Name = loginUserName,
                Level = loginUserLevel,
                SubAccountId = subAccountId
            };
        }

        private static void ValidateSubAccountWithAgencySystem(int subAccountId)
        {
            if (subAccountId > 0 && CustomerSettings.Settings.SystemId != AgentSystem)
            {
                throw new InvalidOperationException(Lang.SubAccountIdShouldBeZeroWhenSystemIsNotAgency);
            }
        }
    }
}