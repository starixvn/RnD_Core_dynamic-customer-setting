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
        private readonly ISettingContext _settingContext;
        private readonly ResourceManager _resourceManager = PositionTakingError.ResourceManager;

        public PositionTakingMessageService() :
            this(null, null)
        {
        }

        public PositionTakingMessageService(
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
                return TranslatePositionTakingViolationMessage(msgType, rawMessage, settingLevel);
            }

            return new MessageContent(baseMessage);
        }

        private MessageContent TranslatePositionTakingViolationMessage(
            int msgType,
            string rawMessage,
            byte settingLevel)
        {
            switch ((PositionTakingMessageType)msgType)
            {
                case PositionTakingMessageType.WrongPositionTaking:
                    return TranslateWrongPositionTakingMessage(rawMessage, settingLevel);

                case PositionTakingMessageType.MinPTViolation:
                    return TranslateMinPTViolationMessage(rawMessage, settingLevel);

                case PositionTakingMessageType.MaxPTViolation:
                    return TranslateMaxPTViolationMessage(rawMessage, settingLevel);

                case PositionTakingMessageType.MinMaxPTViolation:
                    return TranslateMinMaxPTViolationMessage(rawMessage, settingLevel);

                case PositionTakingMessageType.DownlinePTViolation:
                    return TranslateDownlinePTViolationMessage(rawMessage, settingLevel);
            }

            return new MessageContent(rawMessage);
        }

        private MessageContent TranslateWrongPositionTakingMessage(string rawMessage, byte settingLevel)
        {
            var positionTakingRoot = Deserialize<MessageRoot<WrongPTMessageRow>>(rawMessage);
            return GetMessageContent<WrongPTMessageRow>(positionTakingRoot, settingLevel);
        }

        private MessageContent TranslateMinPTViolationMessage(string rawMessage, byte settingLevel)
        {
            var positionTakingRoot = Deserialize<MessageRoot<MinPTViolationMessageRow>>(rawMessage);
            return GetMessageContent<MinPTViolationMessageRow>(positionTakingRoot, settingLevel);
        }

        private MessageContent TranslateMaxPTViolationMessage(string rawMessage, byte settingLevel)
        {
            var positionTakingRoot = Deserialize<MessageRoot<MaxPTViolationMessageRow>>(rawMessage);
            return GetMessageContent<MaxPTViolationMessageRow>(positionTakingRoot, settingLevel);
        }

        private MessageContent TranslateMinMaxPTViolationMessage(string rawMessage, byte settingLevel)
        {
            var positionTakingRoot = Deserialize<MessageRoot<MinMaxPTViolationMessageRow>>(rawMessage);
            return GetMessageContent<MinMaxPTViolationMessageRow>(positionTakingRoot, settingLevel);
        }

        private MessageContent TranslateDownlinePTViolationMessage(string rawMessage, byte settingLevel)
        {
            var positionTakingRoot = Deserialize<DownlinePTViolationMessageRoot>(rawMessage);
            positionTakingRoot.GroupUsernameBySportTypeBetType();
            return GetMessageContent<DownlinePTViolationMessageRow>(positionTakingRoot, settingLevel);
        }

        private MessageContent GetMessageContent<T>(MessageRoot<T> positionTakingRoot, byte settingLevel) where T : IMessageRow
        {
            var subMessages = positionTakingRoot.ToSubMessage(ResourceService, _settingContext.Language, settingLevel);
            var mainMessage = GetMainMessage(_resourceManager, positionTakingRoot.MessageId);
            return new MessageContent(mainMessage, subMessages);
        }
    }
}