namespace Fanex.CustomerSetting.Messages
{
    using System.Xml.Serialization;

    [XmlRoot("Root")]
    public abstract class TemplateMessage<T> : SimpleMessage
        where T : SimpleRow
    {
        [XmlElement("Row")]
        public T[] Rows { get; set; }
    }
}