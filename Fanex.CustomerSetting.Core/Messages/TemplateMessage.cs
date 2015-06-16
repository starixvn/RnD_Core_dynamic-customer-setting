namespace Fanex.CustomerSetting.Core.Messages
{
    using System.Xml.Serialization;

    [XmlRoot("Root")]
    public abstract class TemplateMessage<T> : SimpleMessage
        where T : ISettingItemMessageRow
    {
        [XmlElement("Row")]
        public T[] Rows { get; set; }
    }
}
