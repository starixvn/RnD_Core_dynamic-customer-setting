namespace Fanex.CustomerSetting.SettingType1.Template1.Dtos.Messages
{
    using System.Xml.Serialization;
    using Fanex.CustomerSetting.Resource;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.SettingType1.Template1.Utilities;

    public class MinMaxPTViolationSubMessage : WrongPTSubMessage
    {
        [XmlAttribute]
        public byte MinPT { get; set; }

        [XmlAttribute]
        public byte MaxPT { get; set; }

        public override string ToString(IResourceService resourceService, string language, byte settingLevel)
        {
            return string.Format(
                "{0} - {1}: {2} - {3}: {4}",
                base.ToString(resourceService, language, settingLevel),
                IsLive ? Label.MinLive : Label.Min,
                PositionTakingFormatter.FormatPositionTaking(MinPT),
                IsLive ? Label.MaxLive : Label.Max,
                PositionTakingFormatter.FormatPositionTaking(MaxPT));
        }
    }
}