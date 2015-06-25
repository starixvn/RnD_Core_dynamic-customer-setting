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

        protected T Deserialize<T>(string text) where T : SimpleMessage
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var stringReader = new StringReader(text))
            {
                return (T)serializer.Deserialize(stringReader);
            }
        }

        private string TranslateMessage(string rawMessage)
        {
            var simpleMessage = Deserialize<SimpleMessage>(rawMessage);

            if (simpleMessage != null)
            {
                return GetMainMessage(Error.ResourceManager, simpleMessage);
            }

            return rawMessage;
        }

        protected string GetMainMessage(
            ResourceManager resouceManager, SimpleMessage simpleMessage, string messagePrefix = "Msg")
        {
            var key = string.Format("{0}_{1}", messagePrefix, simpleMessage.MessageId);
            var message = resouceManager.GetString(key);
            return !string.IsNullOrEmpty(message)
                ? message
                : string.Format("Undefined error message {0}", simpleMessage.MessageId);
        }
    }
}