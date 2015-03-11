namespace Fanex.CustomerSetting.Messages
{
    using System.Xml.Serialization;

    /// <summary>
    /// PositionTakingRow class.
    /// </summary>
    public abstract class PositionTakingRow : SimpleRow
    {
        [XmlAttribute]
        public bool IsLive { get; set; }
    }
}