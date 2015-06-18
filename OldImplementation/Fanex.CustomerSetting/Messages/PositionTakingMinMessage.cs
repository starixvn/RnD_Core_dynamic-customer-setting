namespace Fanex.CustomerSetting.Messages
{
    using System.Xml.Serialization;

    [XmlRoot("Root")]
    public class PositionTakingMinMessage : TemplateMessage<PositionTakingMinRow>
    {
    }
}