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
        private readonly ISettingContext _settingContext;
        private readonly ResourceManager _resourceManager = CommissionError.ResourceManager;

        public CommissionMessageService() : 
            this(null, null)
        {
        }

        public CommissionMessageService(
            ISettingContext settingContext, 
            IResourceService resourceService)
            : base(resourceService)
        {
            _settingContext = settingContext;
        }

        public MessageContent TranslateMessage(int msgType, string rawMessage, byte settingLevel)
        {
            var baseMessage = TranslateSimpleMessage(msgType, rawMessage);
            if (string.IsNullOrEmpty(baseMessage))
            {
                return TranslateDefaultViolationMessage(_resourceManager, rawMessage, settingLevel, _settingContext.Language);
            }

            return new MessageContent(baseMessage);
        }
    }
}