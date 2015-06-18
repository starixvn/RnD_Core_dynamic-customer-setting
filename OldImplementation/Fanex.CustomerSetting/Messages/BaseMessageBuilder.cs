namespace Fanex.CustomerSetting.Messages
{
    using System.IO;
    using System.Xml.Serialization;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Resources;
    using Fanex.CustomerSetting.SettingTypes;

    public abstract class BaseMessageBuilder
    {
        private readonly ISettingHandlerFactory _settingHandlerFactory;

        protected BaseMessageBuilder()
            : this(null)
        {
        }

        protected BaseMessageBuilder(ISettingHandlerFactory settingHandlerFactory)
        {
            _settingHandlerFactory = settingHandlerFactory ?? new SettingHandlerFactory();
        }

        protected T Deserialize<T>(string text) where T : SimpleMessage
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var stringReader = new StringReader(text))
            {
                return (T)serializer.Deserialize(stringReader);
            }
        }

        protected string TranslateResource(int messageId, SettingType settingType)
        {
            var settingTypeHandler = _settingHandlerFactory.Create(settingType);
            var prefix = settingTypeHandler.ErrorMsgPrefix;
            var key = string.Format("{0}_{1}", prefix, messageId);
            var message = Error.ResourceManager.GetString(key);
            return !string.IsNullOrEmpty(message)
                ? message
                : string.Format("Undefined error message {0}", messageId);
        }
    }
}