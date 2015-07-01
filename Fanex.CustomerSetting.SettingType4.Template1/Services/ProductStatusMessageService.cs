namespace Fanex.CustomerSetting.SettingType4.Template1.Services
{
    using System.Resources;
    using Fanex.CustomerSetting.Dtos.Messages;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.SettingType4.Template1.Constants;
    using Fanex.CustomerSetting.SettingType4.Template1.Resources;

    public class ProductStatusMessageService : BaseMessageService, IMessageService
    {
        private readonly ResourceManager _resourceManager = ProductStatusMessage.ResourceManager;

        public ProductStatusMessageService(IResourceService resourceService = null)
            : base(resourceService)
        {
        }

        public override MessageContent TranslateMessage(int msgType, string rawMessage, byte settingLevel, string language)
        {
            switch (msgType)
            {
                case SettingType4Constants.ProductStatusViolationMessageType:
                    return TranslateDefaultSettingTypeViolationMessage(_resourceManager, rawMessage, settingLevel, language);

                default:
                    return base.TranslateMessage(msgType, rawMessage, settingLevel, language);
            }
        }
    }
}