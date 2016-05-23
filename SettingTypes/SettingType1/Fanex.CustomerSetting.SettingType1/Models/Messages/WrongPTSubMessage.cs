namespace Fanex.CustomerSetting.SettingType1.Models.Messages
{
    using System.Xml.Serialization;
    using CustomerSetting.Models.Messages;
    using CustomerSetting.Services;
    using Resources;

    public class WrongPTSubMessage : DefaultSubMessage, ISubMessage
    {
        [XmlAttribute]
        public bool IsLive { get; set; }

        public override string ToString(IResourceService resourceService, string language, byte settingLevel)
        {
            var message = base.ToString(resourceService, language, settingLevel);
            if (IsLive)
            {
                message += string.Format(" ({0})", Label.Live);
            }

            return message;
        }
    }
}