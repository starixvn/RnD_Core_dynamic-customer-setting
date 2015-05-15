namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Messages;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.SettingTypes;

    public class MessageService : IMessageService
    {
        private readonly IResourceService _resourceService;

        public MessageService(string languageCode)
            : this(null, languageCode)
        {
        }

        public MessageService(IResourceService resourceService, string languageCode)
        {
            _resourceService = resourceService ?? new ResourceService(languageCode);
        }

        public string GetTranslatedMessage(MessageInfo messageInfo)
        {
            if (messageInfo.ErrCode == Result.SuccessValue)
            {
                ISettingHandler settingHandler = SettingHandlerFactory.Create(messageInfo.SettingType);
                return settingHandler.SuccessMsg;
            }

            var messageBuilder = MessageBuilderFactory.Create(messageInfo.MsgType, _resourceService);
            return messageBuilder.Build(messageInfo);
        }
    }
}