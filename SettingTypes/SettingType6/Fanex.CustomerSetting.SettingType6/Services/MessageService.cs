namespace Fanex.CustomerSetting.SettingType6.Services
{
    using Fanex.CustomerSetting.Models.Messages;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.SettingType6.Constants;
    using Fanex.CustomerSetting.SettingType6.Resources;

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
                case MessageType.WinLimitViolatedWithUpline:
                case MessageType.LossLimitViolatedWithUpline:
                case MessageType.DailyAutoResetViolatedWithUpline:
                    return TranslateDefaultSettingTypeViolationMessage(Message.ResourceManager, rawMessage, settingLevel, language);

                default:
                    return base.TranslateMessage(msgType, rawMessage, settingLevel, language);
            }
        }
    }
}