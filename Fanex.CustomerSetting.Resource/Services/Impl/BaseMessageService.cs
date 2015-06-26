namespace Fanex.CustomerSetting.Resource.Services.Impl
{
    using System.IO;
    using System.Resources;
    using System.Xml.Serialization;
    using Fanex.CustomerSetting.Resource.Dtos.Messages;
    using Fanex.CustomerSetting.Resource.Enumerations;
    using Fanex.CustomerSetting.Resource.Resources;

    public class BaseMessageService
    {
        private const string NotFoundMessageFormat = "Undefined error message {0}";
        private const string MessageFormat = "Msg_{0}";
        protected readonly IResourceService ResourceService;

        public BaseMessageService(IResourceService resourceService)
        {
            ResourceService = resourceService ?? new ResourceService();
        }

        protected string TranslateSimpleMessage(int msgType, string rawMessage)
        {
            switch ((MessageType)msgType)
            {
                case MessageType.Simple:
                    return TranslateMessage(rawMessage);

                case MessageType.Other:
                    return rawMessage;

                default:
                    return string.Empty;
            }
        }

        private string TranslateMessage(string rawMessage)
        {
            var simpleMessage = Deserialize<SimpleMessage>(rawMessage);

            if (simpleMessage != null)
            {
                return GetMainMessage(Error.ResourceManager, simpleMessage.MessageId);
            }

            return rawMessage;
        }

        protected T Deserialize<T>(string text) where T : SimpleMessage
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var stringReader = new StringReader(text))
            {
                return (T)serializer.Deserialize(stringReader);
            }
        }

        protected string GetMainMessage(
            ResourceManager resouceManager, 
            int messsageId)
        {
            var key = string.Format(MessageFormat, messsageId);
            var message = resouceManager.GetString(key);
            return !string.IsNullOrEmpty(message)
                ? message
                : string.Format(NotFoundMessageFormat, messsageId);
        }

        protected MessageContent TranslateDefaultViolationMessage(
            ResourceManager resouceManager,
            string rawMessage,
            byte settingLevel,
            string language)
        {
            var messageRoot = Deserialize<MessageRoot<DefaultMessageRow>>(rawMessage);
            var subMessages = messageRoot.ToSubMessage(ResourceService, language, settingLevel);
            var mainMessage = GetMainMessage(resouceManager, messageRoot.MessageId);
            return new MessageContent(mainMessage, subMessages);
        }
    }
}