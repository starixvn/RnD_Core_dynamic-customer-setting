namespace Fanex.CustomerSetting.SettingType16.Services
{
    using Constants;
    using CustomerSetting.Models.Messages;
    using CustomerSetting.Services;
    using CustomerSetting.Services.Impl;
    using Resources;

    internal class MessageService : MessageServiceBase, IMessageService
    {
        public MessageService(IResourceService resourceService = null)
            : base(resourceService)
        {
        }

        public override MessageContent TranslateMessage(int msgType, string rawMessage, byte settingLevel, string language)
        {
#pragma warning disable S1301 // "switch" statements should have at least 3 "case" clauses
            switch (msgType)
#pragma warning restore S1301 // "switch" statements should have at least 3 "case" clauses
            {
                case MessageTypes.BetLimitViolation:
                    return TranslateDefaultSettingTypeViolationMessage(Message.ResourceManager, rawMessage, settingLevel, language);

                default:
                    return base.TranslateMessage(msgType, rawMessage, settingLevel, language);
            }
        }
    }
}