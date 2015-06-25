namespace Fanex.CustomerSetting.SettingType4.Template1.Services
{
    using System.Resources;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resource.Dtos.Messages;
    using Fanex.CustomerSetting.Resource.Services;
    using Fanex.CustomerSetting.Resource.Services.Impl;
    using Fanex.CustomerSetting.SettingType4.Template1.Resources;

    public class ProductStatusMessageService : BaseMessageService, IMessageService
    {
        private const string ResourceKeyPerfix = "Status";
        private readonly IResourceService _resourceService;
        private readonly ISettingContext _settingContext;
        private readonly ResourceManager _resourceManager = ProductStatusError.ResourceManager;

        public ProductStatusMessageService()
        {
        }

        public ProductStatusMessageService(ISettingContext settingContext, IResourceService resourceService)
        {
            _resourceService = resourceService ?? new ResourceService();
            _settingContext = settingContext;
        }

        public MessageContent TranslateMessage(int msgType, string rawMessage, byte settingLevel)
        {
            var baseMessage = TranslateSimpleMessage(msgType, rawMessage);
            if (string.IsNullOrEmpty(baseMessage))
            {
                return TranslateBetLimitViolationMessage(msgType, rawMessage, settingLevel);
            }

            return new MessageContent(baseMessage);
        }

        private MessageContent TranslateBetLimitViolationMessage(int msgType, string rawMessage, byte settingLevel)
        {
            var betLimitRoot = Deserialize<MessageRoot<DefaultMessageRow>>(rawMessage);
            var subMessages = betLimitRoot.ToSubMessage(_resourceService, _settingContext.Language, settingLevel);
            var mainMessage = GetMainMessage(_resourceManager, betLimitRoot, ResourceKeyPerfix);
            return new MessageContent(mainMessage, subMessages);
        }
    }
}