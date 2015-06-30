namespace Fanex.CustomerSetting.SettingType1.Template1.Services
{
    using System.Resources;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resource.Dtos.Messages;
    using Fanex.CustomerSetting.Resource.Services;
    using Fanex.CustomerSetting.Resource.Services.Impl;
    using Fanex.CustomerSetting.SettingType1.Template1.Dtos.Messages;
    using Fanex.CustomerSetting.SettingType1.Template1.Enumerations;
    using Fanex.CustomerSetting.SettingType1.Template1.Resources;

    public class PositionTakingMessageService : BaseMessageService, IMessageService
    {
        private readonly ResourceManager _resourceManager = PositionTakingError.ResourceManager;

        public PositionTakingMessageService(IResourceService resourceService = null)
            : base(resourceService)
        {
        }

        public MessageContent TranslateMessage(int msgType, string rawMessage, byte settingLevel, string language)
        {
            var generalMessage = TranslateGeneralMessage(msgType, rawMessage);
            if (string.IsNullOrEmpty(generalMessage))
            {
                return TranslateViolationPTMessage(msgType, rawMessage, settingLevel, language);
            }

            return new MessageContent(generalMessage);
        }

        private MessageContent TranslateViolationPTMessage(
            int msgType,
            string rawMessage,
            byte settingLevel,
            string language)
        {
            switch ((PositionTakingMessageType)msgType)
            {
                case PositionTakingMessageType.WrongPositionTaking:

                    return TranslateMessage<WrongPTMessageRow>(rawMessage, settingLevel, language);

                    //return TranslateWrongPositionTakingMessage(rawMessage, settingLevel, language);

                case PositionTakingMessageType.MinPTViolation:

                    return TranslateMessage<MinPTViolationMessageRow>(rawMessage, settingLevel, language);
                    // return TranslateMinPTViolationMessage(rawMessage, settingLevel, language);

                case PositionTakingMessageType.MaxPTViolation:
                     return TranslateMessage<MaxPTViolationMessageRow>(rawMessage, settingLevel, language);
                    //return TranslateMaxPTViolationMessage(rawMessage, settingLevel, language);

                case PositionTakingMessageType.MinMaxPTViolation:
                    return TranslateMessage<MinMaxPTViolationMessageRow>(rawMessage, settingLevel, language);
                    //return TranslateMinMaxPTViolationMessage(rawMessage, settingLevel, language);

                case PositionTakingMessageType.DownlinePTViolation:
                    return TranslateDownlinePTViolationMessage(rawMessage, settingLevel, language);
            }

            return new MessageContent(rawMessage);
        }

        private MessageContent TranslateMessage<T>(string rawMessage, byte settingLevel, string language) where T : IMessageRow
        {
            var message = Deserialize<MessageRoot<T>>(rawMessage);
            return GetMessageContent<T>(message, settingLevel, language);
        }

        private MessageContent GetMessageContent<T>(MessageRoot<T> positionTakingRoot, byte settingLevel, string language) where T : IMessageRow
        {
            var subMessages = positionTakingRoot.ToSubMessage(ResourceService, language, settingLevel);
            var mainMessage = GetMainMessage(_resourceManager, positionTakingRoot.MessageId);
            return new MessageContent(mainMessage, subMessages);
        }

        private MessageContent TranslateDownlinePTViolationMessage(string rawMessage, byte settingLevel, string language)
        {
            var positionTakingRoot = Deserialize<DownlinePTViolationMessageRoot>(rawMessage);
            positionTakingRoot.GroupUsernameBySportTypeBetType();
            return GetMessageContent<DownlinePTViolationMessageRow>(positionTakingRoot, settingLevel, language);
        }

      
            where T : IMessageRow
    }
}