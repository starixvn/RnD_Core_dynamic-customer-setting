namespace Fanex.CustomerSetting.Resource.Dtos.Messages
{
    using System.Xml.Serialization;

    public class SimpleMessage
    {
        [XmlAttribute("MsgID")]
        public int MessageId { get; set; }

        [XmlAttribute("Msg")]
        public string Message { get; set; }
    }
}