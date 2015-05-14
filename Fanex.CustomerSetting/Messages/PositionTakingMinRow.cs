namespace Fanex.CustomerSetting.Messages
{
    using System.Xml.Serialization;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resources;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Utility;

    public class PositionTakingMinRow : PositionTakingRow
    {
        [XmlAttribute]
        public byte MinPT { get; set; }

        public override string ToString(IResourceService resourceService, MessageInfo messageInfo)
        {
            return string.Format(
                "{0} - {1}: {2}",
                base.ToString(resourceService, messageInfo),
                IsLive ? Lang.MinLive : Lang.Min,
                Formatter.FormatPositionTaking(MinPT));
        }
    }
}