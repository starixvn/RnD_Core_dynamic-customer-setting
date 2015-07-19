namespace Fanex.CustomerSetting.SettingType3.Template1.Services
{
    using Fanex.CustomerSetting.Dtos.Messages;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.SettingType3.Template1.Constants;
    using Fanex.CustomerSetting.SettingType3.Template1.Resources;

    public class MessageService : BaseMessageService, IMessageService
    {
        public MessageService(IResourceService resourceService = null)
            : base(resourceService)
        {
        }

        public override MessageContent TranslateMessage(int msgType, string rawMessage, byte settingLevel, string language)
        {
            switch (msgType)
            {
                case MessageType.CommissionViolation:
                    return TranslateDefaultSettingTypeViolationMessage(CommissionMessage.ResourceManager, rawMessage, settingLevel, language);

                default:
                    return base.TranslateMessage(msgType, rawMessage, settingLevel, language);
            }
        }
    }
}