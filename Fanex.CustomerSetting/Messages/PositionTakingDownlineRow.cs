namespace Fanex.CustomerSetting.Messages
{
    using System.Xml.Serialization;

    public class PositionTakingDownlineRow : PositionTakingRow
    {
        [XmlAttribute]
        public string UserName { get; set; }
    }
}