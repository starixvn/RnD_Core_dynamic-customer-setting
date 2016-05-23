namespace Fanex.CustomerSetting.SettingType1.Models.Messages
{
    using System.Xml.Serialization;
    using CustomerSetting.Services;
    using Resource;

    public class DownlinePTViolationSubMessage : WrongPTSubMessage
    {
        [XmlAttribute]
        public string UserName { get; set; }

        public override string ToString(IResourceService resourceService, string language, byte settingLevel)
        {
            return string.Format(
                "{0} - {1}: {2}",
                base.ToString(resourceService, language, settingLevel),
                Label.UserNames,
                UserName);
        }
    }
}