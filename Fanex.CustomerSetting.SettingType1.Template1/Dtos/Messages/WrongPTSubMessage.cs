namespace Fanex.CustomerSetting.SettingType1.Template1.Dtos.Messages
{
    using System.Xml.Serialization;
    using Fanex.CustomerSetting.Messages;
    using Fanex.CustomerSetting.Resource;
    using Fanex.CustomerSetting.Services;

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