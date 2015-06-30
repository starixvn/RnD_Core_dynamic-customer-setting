namespace Fanex.CustomerSetting.SettingType2.Template1.Services
{
    using System.Resources;
    using Fanex.CustomerSetting.Dtos.Messages;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.SettingType2.Template1.Resources;

    public class BetLimitMessageService : BaseMessageService, IMessageService
    {
        private readonly ResourceManager _resourceManager = BetLimitError.ResourceManager;

        public BetLimitMessageService(IResourceService resourceService = null)
            : base(resourceService)
        {
        }

        public MessageContent TranslateMessage(int msgType, string rawMessage, byte settingLevel, string language)
        {
            var generalMessage = TranslateGeneralMessage(msgType, rawMessage);
            if (string.IsNullOrEmpty(generalMessage))
            {
                return TranslateDefaultViolationMessage(_resourceManager, rawMessage, settingLevel, language);
            }

            return new MessageContent(generalMessage);
        }
    }
}