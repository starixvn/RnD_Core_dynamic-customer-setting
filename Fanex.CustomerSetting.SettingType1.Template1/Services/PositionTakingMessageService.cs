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
        private const string ResourceKeyPerfix = "PositionTaking";
        private readonly IResourceService _resourceService;
        private readonly ISettingContext _settingContext;
        private readonly ResourceManager _resourceManager = SettingType1Template1Error.ResourceManager;

        public PositionTakingMessageService()
        {
        }

        public PositionTakingMessageService(ISettingContext settingContext, IResourceService resourceService)
        {
            _resourceService = resourceService ?? new ResourceService();
            _settingContext = settingContext;
        }

        public MessageContent TranslateMessage(int msgType, string rawMessage)
        {
            var baseMessage = TranslateSimpleMessage(msgType, rawMessage);
            if (string.IsNullOrEmpty(baseMessage))
            {
                return TranslatePositionTakingViolationMessage(msgType, rawMessage);
            }

            return new MessageContent(baseMessage);
        }

        private MessageContent TranslatePositionTakingViolationMessage(int msgType, string rawMessage)
        {
            switch ((PositionTakingMessageType)msgType)
            {
                case PositionTakingMessageType.WrongPositionTaking:
                    return TranslateWrongPositionTakingMessage(rawMessage);

                case PositionTakingMessageType.DownlinePTViolation:
                    return TranslateDownlinePTViolationMessage(rawMessage);

                case PositionTakingMessageType.MinPTViolation:
                    return TranslateMinPTViolationMessage(rawMessage);

                case PositionTakingMessageType.MaxPTViolation:
                    return TranslateMaxPTViolationMessage(rawMessage);

                case PositionTakingMessageType.MinMaxPTViolation:
                    return TranslateMinMaxPTViolationMessage(rawMessage);
            }

            return new MessageContent(rawMessage);
        }

        private MessageContent TranslateWrongPositionTakingMessage(string rawMessage)
        {
            var positionTakingRoot = Deserialize<MessageRoot<WrongPTMessageRow>>(rawMessage);
            return GetMessageContent<WrongPTMessageRow>(positionTakingRoot);
        }

        private MessageContent TranslateDownlinePTViolationMessage(string rawMessage)
        {
            var positionTakingRoot = Deserialize<DownlinePTViolationMessageRoot>(rawMessage);
            positionTakingRoot.GroupUsernameBySportTypeBetType();
            return GetMessageContent<DownlinePTViolationMessageRow>(positionTakingRoot);
        }

        private MessageContent TranslateMinPTViolationMessage(string rawMessage)
        {
            var positionTakingRoot = Deserialize<MessageRoot<MinPTViolationMessageRow>>(rawMessage);
            return GetMessageContent<MinPTViolationMessageRow>(positionTakingRoot);
        }

        private MessageContent TranslateMaxPTViolationMessage(string rawMessage)
        {
            var positionTakingRoot = Deserialize<MessageRoot<MaxPTViolationMessageRow>>(rawMessage);
            return GetMessageContent<MaxPTViolationMessageRow>(positionTakingRoot);
        }

        private MessageContent TranslateMinMaxPTViolationMessage(string rawMessage)
        {
            var positionTakingRoot = Deserialize<MessageRoot<MinMaxPTViolationMessageRow>>(rawMessage);
            return GetMessageContent<MinMaxPTViolationMessageRow>(positionTakingRoot);
        }

        private MessageContent GetMessageContent<T>(MessageRoot<T> positionTakingRoot) where T : IMessageRow
        {
            var subMessages = positionTakingRoot.ToSubMessage(_resourceService, _settingContext.Language, 1);
            var mainMessage = GetGeneralMessage(_resourceManager, positionTakingRoot, ResourceKeyPerfix);
            return new MessageContent(mainMessage, subMessages);
        }
    }
}