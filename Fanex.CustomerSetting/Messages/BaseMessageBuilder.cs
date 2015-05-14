namespace Fanex.CustomerSetting.Messages
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Resources;

    public abstract class BaseMessageBuilder
    {
        private Dictionary<SettingType, string> _prefixs = new Dictionary<SettingType, string>
        {
            { SettingType.Status, "Status" },
            { SettingType.Discount, "Discount" },
            { SettingType.BetLimit, "BetLimit" },
            { SettingType.PositionTaking, "PositionTaking" }
        };

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
            var prefix = _prefixs[settingType];
            var key = string.Format("{0}_{1}", prefix, messageId);
            var message = Error.ResourceManager.GetString(key);
            return !string.IsNullOrEmpty(message)
                ? message
                : string.Format("Undefined error message {0}", messageId);
        }
    }
}