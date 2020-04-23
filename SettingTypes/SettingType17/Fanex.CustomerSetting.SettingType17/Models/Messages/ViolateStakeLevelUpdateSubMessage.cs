namespace Fanex.CustomerSetting.SettingType17.Models.Messages
{
    using Fanex.CustomerSetting.Models.Messages;
    using System.Xml.Serialization;

    public class ViolateStakeLevelUpdateSubMessage : DefaultSubMessage
    {
        [XmlAttribute]
        public string MaxChoice { get; set; }
    }
}
