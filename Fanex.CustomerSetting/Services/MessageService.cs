namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Messages;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.SettingTypes;

    public class MessageService : IMessageService
    {
        private readonly IMessageBuilderFactory _messageBuilderFactory;
        private readonly ISettingHandlerFactory _settingHandlerFactory;

        public MessageService()
            : this(null, null)
        {
        }

        public MessageService(IMessageBuilderFactory messageBuilderFactory, ISettingHandlerFactory settingHandlerFactory)
        {
            _messageBuilderFactory = messageBuilderFactory ?? new MessageBuilderFactory();
            _settingHandlerFactory = settingHandlerFactory ?? new SettingHandlerFactory();
        }

        public string GetTranslatedMessage(MessageInfo messageInfo)
        {
            if (messageInfo.ErrCode == Result.SuccessValue)
            {
                ISettingHandler settingHandler = _settingHandlerFactory.Create(messageInfo.SettingType);
                return settingHandler.SuccessMsg;
            }

            var messageBuilder = _messageBuilderFactory.Create(messageInfo.MsgType);
            return messageBuilder.Build(messageInfo);
        }
    }
}