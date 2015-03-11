namespace Fanex.CustomerSetting.Messages
{
    using System.Xml.Serialization;
    using Fanex.CustomerSetting.Resources;
    using Fanex.CustomerSetting.Services;

    /// <summary>
    /// PositionTakingMinRow class.
    /// </summary>
    public class PositionTakingMinMaxRow : PositionTakingRow
    {
        [XmlAttribute]
        public string MinPT { get; set; }

        [XmlAttribute]
        public string MaxPT { get; set; }

        public override string ToString(IResourceService resourceService)
        {
            return string.Format(
                "{0} - {1}: {2} - {3}: {4}",
                base.ToString(resourceService),
                IsLive ? Lang.MinLive : Lang.Min,
                MinPT,
                IsLive ? Lang.MaxLive : Lang.Max,
                MaxPT);
        }
    }
}