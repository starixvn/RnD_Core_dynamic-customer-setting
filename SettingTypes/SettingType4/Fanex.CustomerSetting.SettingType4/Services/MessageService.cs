namespace Fanex.CustomerSetting.SettingType4.Services
{
    using Fanex.CustomerSetting.Models.Messages;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.SettingType4.Constants;
    using Fanex.CustomerSetting.SettingType4.Resources;

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
                case MessageType.ProductStatusViolation:
                    return TranslateDefaultSettingTypeViolationMessage(Message.ResourceManager, rawMessage, settingLevel, language);

                default:
                    return base.TranslateMessage(msgType, rawMessage, settingLevel, language);
            }
        }
    }
}