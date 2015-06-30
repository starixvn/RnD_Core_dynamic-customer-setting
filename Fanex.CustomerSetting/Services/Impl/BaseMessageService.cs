namespace Fanex.CustomerSetting.Services.Impl
{
    using System.IO;
    using System.Resources;
    using System.Xml.Serialization;
    using Fanex.CustomerSetting.Constants;
    using Fanex.CustomerSetting.Dtos.Messages;
    using Fanex.CustomerSetting.Resource.Resources;

    public class BaseMessageService
    {
        private const string NotFoundMessageFormat = "Undefined error message {0}";
        private const string MessageFormat = "Msg_{0}";

        public BaseMessageService(IResourceService resourceService)
        {
            ResourceService = resourceService ?? new ResourceService();
        }

        protected IResourceService ResourceService { get; set; }

        protected string TranslateGeneralMessage(int msgType, string rawMessage)
        {
            switch (msgType)
            {
                case MessageType.Exception:
                    return TranslateSimpleMessage(rawMessage);

                case MessageType.Other:
                    return rawMessage;

                default:
                    //// Message type from setting type, each setting type will handle by themself.
                    return string.Empty;
            }
        }

        private string TranslateSimpleMessage(string rawMessage)
        {
            var simpleMessage = Deserialize<SimpleMessage>(rawMessage);

            if (simpleMessage != null)
            {
                return GetMessage(Message.ResourceManager, simpleMessage.MessageId);
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

        protected string GetMessage(ResourceManager resouceManager, int messsageId)
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
            var mainMessage = Deserialize<MainMessage<DefaultSubMessage>>(rawMessage);
            var subMessages = mainMessage.GetSubMessages(ResourceService, language, settingLevel);
            var message = GetMessage(resouceManager, mainMessage.MessageId);
            return new MessageContent(message, subMessages);
        }
    }
}