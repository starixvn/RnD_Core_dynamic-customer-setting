namespace Fanex.CustomerSetting.SettingType1.Models.Messages
{
    using System.Xml.Serialization;
    using Fanex.CustomerSetting.Models.Messages;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.SettingType1.Resources;

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