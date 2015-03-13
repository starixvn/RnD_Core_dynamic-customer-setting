namespace Fanex.CustomerSetting.Messages
{
    using System.Xml.Serialization;

    /// <summary>
    /// PositionTakingMessage class.
    /// </summary>
    [XmlRoot("Root")]
    public class PositionTakingMessage : TemplateMessage<PositionTakingRow>
    {
    }
}