namespace Fanex.CustomerSetting.SettingType1.Template1.Services
{
    using System.Resources;
    using Fanex.CustomerSetting.Dtos.Messages;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.SettingType1.Template1.Constants;
    using Fanex.CustomerSetting.SettingType1.Template1.Dtos.Messages;
    using Fanex.CustomerSetting.SettingType1.Template1.Resources;

    public class MessageService : MessageServiceBase, IMessageService
    {
        public MessageService(IResourceService resourceService = null)
            : base(resourceService)
        {
        }

        public override MessageContent TranslateMessage(int msgType, string rawMessage, byte settingLevel, string language)
        {
            switch (msgType)
            {
                case MessageType.WrongPT:
                    return TranslateMessage<WrongPTSubMessage>(rawMessage, settingLevel, language);

                case MessageType.MinPTViolation:
                    return TranslateMessage<MinPTViolationSubMessage>(rawMessage, settingLevel, language);

                case MessageType.MaxPTViolation:
                    return TranslateMessage<MaxPTViolationSubMessage>(rawMessage, settingLevel, language);

                case MessageType.MinMaxPTViolation:
                    return TranslateMessage<MinMaxPTViolationSubMessage>(rawMessage, settingLevel, language);

                case MessageType.DownlinePTViolation:
                    return TranslateDownlinePTViolationMessage(rawMessage, settingLevel, language);

                default:
                    return base.TranslateMessage(msgType, rawMessage, settingLevel, language);
            }
        }

        private MessageContent TranslateMessage<T>(string rawMessage, byte settingLevel, string language) where T : ISubMessage
        {
            var mainMessage = Deserialize<MainMessage<T>>(rawMessage);
            return GetMessageContent<T>(mainMessage, settingLevel, language);
        }

        private MessageContent GetMessageContent<T>(MainMessage<T> positionTakingMainMessage, byte settingLevel, string language) where T : ISubMessage
        {
            var translatedSubMessages = positionTakingMainMessage.TranslateSubMessages(ResourceService, language, settingLevel);
            var translatedMainMessage = GetMessage(PTMessage.ResourceManager, positionTakingMainMessage.MessageId);
            return new MessageContent(translatedMainMessage, translatedSubMessages);
        }

        private MessageContent TranslateDownlinePTViolationMessage(string rawMessage, byte settingLevel, string language)
        {
            var positionTakingMainMessage = Deserialize<DownlinePTViolationMainMessage>(rawMessage);
            positionTakingMainMessage.GroupUsernameBySportTypeBetType();
            return GetMessageContent<DownlinePTViolationSubMessage>(positionTakingMainMessage, settingLevel, language);
        }
    }
}