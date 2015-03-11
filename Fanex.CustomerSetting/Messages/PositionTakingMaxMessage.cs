namespace Fanex.CustomerSetting.Messages
{
    using System.Xml.Serialization;

    /// <summary>
    /// PositionTakingMaxMessage class.
    /// </summary>
    [XmlRoot("Root")]
    public class PositionTakingMaxMessage : TemplateMessage<PositionTakingMaxRow>
    {
    }
}