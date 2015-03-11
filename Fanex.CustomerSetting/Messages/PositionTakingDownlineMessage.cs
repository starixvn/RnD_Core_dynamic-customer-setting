namespace Fanex.CustomerSetting.Messages
{
    using System.Xml.Serialization;

    /// <summary>
    /// PositionTakingDownLineMessage class.
    /// </summary>
    [XmlRoot("Root")]
    public class PositionTakingDownlineMessage : TemplateMessage<PositionTakingDownlineRow>
    {
    }
}