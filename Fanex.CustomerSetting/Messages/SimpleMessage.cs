namespace Fanex.CustomerSetting.Messages
{
    using System.Xml.Serialization;

    /// <summary>
    /// BaseMessage class.
    /// </summary>
    [XmlRoot("Root")]
    public class SimpleMessage
    {
        [XmlAttribute("MsgID")]
        public int MessageId { get; set; }

        [XmlAttribute("Msg")]
        public string Message { get; set; }
    }
}