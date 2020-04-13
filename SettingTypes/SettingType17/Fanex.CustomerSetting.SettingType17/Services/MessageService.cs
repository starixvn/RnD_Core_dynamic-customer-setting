namespace Fanex.CustomerSetting.SettingType17.Services
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
            switch (msgType)
            {
                case MessageTypes.StakeLevelViolation:
                    return TranslateDefaultSettingTypeViolationMessage(Message.ResourceManager, rawMessage, settingLevel, language);

                default:
                    return base.TranslateMessage(msgType, rawMessage, settingLevel, language);
            }
        }
    }
}