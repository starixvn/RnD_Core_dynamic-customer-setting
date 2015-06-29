namespace Fanex.CustomerSetting.SettingType3.Template1.Services
{
    using System.Resources;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resource.Dtos.Messages;
    using Fanex.CustomerSetting.Resource.Services;
    using Fanex.CustomerSetting.Resource.Services.Impl;
    using Fanex.CustomerSetting.SettingType3.Template1.Resources;

    public class CommissionMessageService : BaseMessageService, IMessageService
    {
        private readonly ResourceManager _resourceManager = CommissionError.ResourceManager;

        public CommissionMessageService(IResourceService resourceService = null)
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