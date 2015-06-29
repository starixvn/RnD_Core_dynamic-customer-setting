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
                return TranslatePositionTakingViolationMessage(msgType, rawMessage, settingLevel, language);
            }

            return new MessageContent(generalMessage);
        }

        private MessageContent TranslatePositionTakingViolationMessage(
            int msgType,
            string rawMessage,
            byte settingLevel,
            string language)
        {
            switch ((PositionTakingMessageType)msgType)
            {
                case PositionTakingMessageType.WrongPositionTaking:
                    return TranslateWrongPositionTakingMessage(rawMessage, settingLevel, language);

                case PositionTakingMessageType.MinPTViolation:
                    return TranslateMinPTViolationMessage(rawMessage, settingLevel, language);

                case PositionTakingMessageType.MaxPTViolation:
                    return TranslateMaxPTViolationMessage(rawMessage, settingLevel, language);

                case PositionTakingMessageType.MinMaxPTViolation:
                    return TranslateMinMaxPTViolationMessage(rawMessage, settingLevel, language);

                case PositionTakingMessageType.DownlinePTViolation:
                    return TranslateDownlinePTViolationMessage(rawMessage, settingLevel, language);
            }

            return new MessageContent(rawMessage);
        }

        private MessageContent TranslateWrongPositionTakingMessage(string rawMessage, byte settingLevel, string language)
        {
            var positionTakingRoot = Deserialize<MessageRoot<WrongPTMessageRow>>(rawMessage);
            return GetMessageContent<WrongPTMessageRow>(positionTakingRoot, settingLevel, language);
        }

        private MessageContent TranslateMinPTViolationMessage(string rawMessage, byte settingLevel, string language)
        {
            var positionTakingRoot = Deserialize<MessageRoot<MinPTViolationMessageRow>>(rawMessage);
            return GetMessageContent<MinPTViolationMessageRow>(positionTakingRoot, settingLevel, language);
        }

        private MessageContent TranslateMaxPTViolationMessage(string rawMessage, byte settingLevel, string language)
        {
            var positionTakingRoot = Deserialize<MessageRoot<MaxPTViolationMessageRow>>(rawMessage);
            return GetMessageContent<MaxPTViolationMessageRow>(positionTakingRoot, settingLevel, language);
        }

        private MessageContent TranslateMinMaxPTViolationMessage(string rawMessage, byte settingLevel, string language)
        {
            var positionTakingRoot = Deserialize<MessageRoot<MinMaxPTViolationMessageRow>>(rawMessage);
            return GetMessageContent<MinMaxPTViolationMessageRow>(positionTakingRoot, settingLevel, language);
        }

        private MessageContent TranslateDownlinePTViolationMessage(string rawMessage, byte settingLevel, string language)
        {
            var positionTakingRoot = Deserialize<DownlinePTViolationMessageRoot>(rawMessage);
            positionTakingRoot.GroupUsernameBySportTypeBetType();
            return GetMessageContent<DownlinePTViolationMessageRow>(positionTakingRoot, settingLevel, language);
        }

        private MessageContent GetMessageContent<T>(MessageRoot<T> positionTakingRoot, byte settingLevel, string language) where T : IMessageRow
        {
            var subMessages = positionTakingRoot.ToSubMessage(ResourceService, language, settingLevel);
            var mainMessage = GetMainMessage(_resourceManager, positionTakingRoot.MessageId);
            return new MessageContent(mainMessage, subMessages);
        }
    }
}