namespace Fanex.CustomerSetting.SettingType2.Template1.Services
{
    using System.Resources;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resource.Dtos.Messages;
    using Fanex.CustomerSetting.Resource.Services;
    using Fanex.CustomerSetting.Resource.Services.Impl;
    using Fanex.CustomerSetting.SettingType2.Template1.Resources;

    public class BetLimitMessageService : BaseMessageService, IMessageService
    {
        private readonly ISettingContext _settingContext;
        private readonly ResourceManager _resourceManager = BetLimitError.ResourceManager;

        public BetLimitMessageService() : 
            this(null, null)
        {
        }

        public BetLimitMessageService(
            ISettingContext settingContext, 
            IResourceService resourceService)
            : base(resourceService)
        {
            _settingContext = settingContext;
        }

        public MessageContent TranslateMessage(int msgType, string rawMessage, byte settingLevel)
        {
            var generalMessage = TranslateGeneralMessage(msgType, rawMessage);
            if (string.IsNullOrEmpty(generalMessage))
            {
                return TranslateDefaultViolationMessage(_resourceManager, rawMessage, settingLevel, _settingContext.Language);
            }

            return new MessageContent(generalMessage);
        }
    }
}