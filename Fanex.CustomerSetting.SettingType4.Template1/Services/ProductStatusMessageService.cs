namespace Fanex.CustomerSetting.SettingType4.Template1.Services
{
    using System.Resources;
    using Fanex.CustomerSetting.Dtos.Messages;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.SettingType4.Template1.Resources;

    public class ProductStatusMessageService : BaseMessageService, IMessageService
    {
        private readonly ResourceManager _resourceManager = ProductStatusError.ResourceManager;

        public ProductStatusMessageService(IResourceService resourceService = null)
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