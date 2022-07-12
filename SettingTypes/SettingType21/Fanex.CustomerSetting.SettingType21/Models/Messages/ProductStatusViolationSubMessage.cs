namespace Fanex.CustomerSetting.SettingType21.Models.Messages
{
    using System.Xml.Serialization;
    using Fanex.CustomerSetting.Models.Messages;
    
    public class ProductStatusViolationSubMessage : DefaultSubMessage
    {
        [XmlAttribute]
        public short ParentSportType { get; set; }

        [XmlAttribute]
        public short ParentBetType { get; set; }
        
    }
}