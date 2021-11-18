namespace Fanex.CustomerSetting.SettingType1.Services
{
    using Constants;
    using CustomerSetting.Models.Messages;
    using CustomerSetting.Services;
    using CustomerSetting.Services.Impl;
    using Models.Messages;
    using Resources;
    using System;

    internal class MessageService : MessageServiceBase, IMessageService
    {
        public MessageService(IResourceService resourceService = null)
            : base(resourceService)
        {
        }

        public override MessageContent TranslateMessage(int msgType, string rawMessage, byte settingLevel, string language)
        {
            switch (msgType)
            {
                case MessageTypes.WrongPT:
                    return TranslateMessage<WrongPTSubMessage>(rawMessage, settingLevel, language);

                case MessageTypes.MinPTViolation:
                    return TranslateMessage<MinPTViolationSubMessage>(rawMessage, settingLevel, language);

                case MessageTypes.MaxPTViolation:
                    return TranslateMessage<MaxPTViolationSubMessage>(rawMessage, settingLevel, language);

                case MessageTypes.MinMaxPTViolation:
                    return TranslateMessage<MinMaxPTViolationSubMessage>(rawMessage, settingLevel, language);

                case MessageTypes.DownlinePTViolation:
                    return TranslateDownlinePTViolationMessage(rawMessage, settingLevel, language);

                case MessageTypes.UpdateTooOften:
                    return TranslatUpdateTooOftenMessage(rawMessage, language);

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
            var subMessage = positionTakingMainMessage.TranslateSubMessages(ResourceService, language, settingLevel);
            var mainMessage = GetMessage(Message.ResourceManager, positionTakingMainMessage.MessageId);

            return new MessageContent(mainMessage, subMessage);
        }

        private MessageContent TranslateDownlinePTViolationMessage(string rawMessage, byte settingLevel, string language)
        {
            var positionTakingMainMessage = Deserialize<DownlinePTViolationMainMessage>(rawMessage);
            positionTakingMainMessage.GroupUsernameBySportTypeBetType();

            return GetMessageContent<DownlinePTViolationSubMessage>(positionTakingMainMessage, settingLevel, language);
        }

        private MessageContent TranslatUpdateTooOftenMessage(string rawMessage, string language)
        {
            var mainMessage = rawMessage;
            var updateTooOftenMessage = Deserialize<SimpleMessage>(rawMessage);

            if (updateTooOftenMessage != null)
            {
                mainMessage = string.Format(GetMessage(Message.ResourceManager, updateTooOftenMessage.MessageId), DateTime.UtcNow.AddHours(8).ToString("dd/MM/yyyy"));
            }

            return new MessageContent(mainMessage);
        }
    }
}