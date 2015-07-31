namespace Fanex.CustomerSetting.Web.Contexts
{
    using System;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Resource;

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
            SettingContext = new SettingContext();
        }

        public static ISettingContext SettingContext { get; private set; }

        /// <summary>
        /// Call this function after user login to system,
        /// to store current user information for viewing/updating setting.
        /// </summary>
        /// <param name="loginUserId">Identifier of user login</param>
        /// <param name="loginUserName">User Name of user login</param>
        /// <param name="loginUserLevel">Level of user login to system, can be Admin, Super, Master or Agent.</param>
        /// <param name="subAccountId">For agency system, Identifier of sub-account log in</param>
        public static void Init(
            int loginUserId,
            string loginUserName,
            byte loginUserLevel,
            byte system,
            int subAccountId = 0)
        {
            ValidateSubAccountWithSystem(subAccountId, system);

            SettingContext.Actor = new User
            {
                Id = loginUserId,
                Name = loginUserName,
                Level = Enumeration.FromValue<Level>(loginUserLevel),
                SubAccountId = subAccountId
            };

            SettingContext.System = system;
        }

        private static void ValidateSubAccountWithSystem(
            int subAccountId,
            byte system)
        {
            if (subAccountId > 0
                && system != (byte)SystemType.Agency)
            {
                throw new InvalidOperationException(Message.SubAccountIdShouldBeZeroWhenSystemIsNotAgency);
            }
        }

        public static void SetLanguage(string language)
        {
            Check.Argument.IsNotEmpty(language, "language");

            SettingContext.Language = language;
        }
    }
}