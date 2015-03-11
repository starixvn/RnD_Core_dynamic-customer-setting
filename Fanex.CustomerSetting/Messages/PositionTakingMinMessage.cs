namespace Fanex.CustomerSetting.Messages
{
    using System.Xml.Serialization;

    /// <summary>
    /// PositionTakingMinMessage class.
    /// </summary>
    [XmlRoot("Root")]
    public class PositionTakingMinMessage : TemplateMessage<PositionTakingMinRow>
    {
    }
}