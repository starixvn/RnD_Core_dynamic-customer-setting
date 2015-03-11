namespace Fanex.CustomerSetting.Messages
{
    using System.Xml.Serialization;
    using Fanex.CustomerSetting.Resources;
    using Fanex.CustomerSetting.Services;

    /// <summary>
    /// PositionTakingMinRow class.
    /// </summary>
    public class PositionTakingMinRow : PositionTakingRow
    {
        [XmlAttribute]
        public string MinPT { get; set; }

        public override string ToString(IResourceService resourceService)
        {
            return string.Format(
                "{0} - {1}: {2}",
                base.ToString(resourceService),
                IsLive ? Lang.MinLive : Lang.Min,
                MinPT);
        }
    }
}