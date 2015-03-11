namespace Fanex.CustomerSetting.Messages
{
    using System.Xml.Serialization;

    /// <summary>
    /// PositionTakingMinMaxMessage class.
    /// </summary>
    [XmlRoot("Root")]
    public class PositionTakingMinMaxMessage : TemplateMessage<PositionTakingMinMaxRow>
    {
    }
}