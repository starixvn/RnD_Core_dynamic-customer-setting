namespace Fanex.CustomerSetting.Messages
{
    using System.Xml.Serialization;

    [XmlRoot("Root")]
    public class PositionTakingMessage : TemplateMessage<PositionTakingRow>
    {
    }
}