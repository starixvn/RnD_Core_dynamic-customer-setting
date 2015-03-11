namespace Fanex.CustomerSetting.Messages
{
    using System.Xml.Serialization;

    /// <summary>
    /// NormalMessage class.
    /// </summary>
    [XmlRoot("Root")]
    public class NormalMessage : TemplateMessage<SimpleRow>
    {
    }
}