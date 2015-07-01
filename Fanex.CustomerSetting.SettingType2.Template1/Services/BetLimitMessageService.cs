namespace Fanex.CustomerSetting.SettingType2.Template1.Services
{
    using System.Resources;
    using Fanex.CustomerSetting.Dtos.Messages;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.SettingType2.Template1.Constants;
    using Fanex.CustomerSetting.SettingType2.Template1.Resources;

    public class BetLimitMessageService : BaseMessageService, IMessageService
    {
        private readonly ResourceManager _resourceManager = BetLimitMessage.ResourceManager;

        public BetLimitMessageService(IResourceService resourceService = null)
            : base(resourceService)
        {
        }

        public MessageContent TranslateMessage(int msgType, string rawMessage, byte settingLevel, string language)
        {
            switch (msgType)
            {
                case SettingType2Constants.BetLimitViolationMessageType:
                    return TranslateDefaultSettingTypeViolationMessage(_resourceManager, rawMessage, settingLevel, language);

                default:
                    return base.TranslateMessage(msgType, rawMessage, settingLevel, language);
            }
        }
    }
}