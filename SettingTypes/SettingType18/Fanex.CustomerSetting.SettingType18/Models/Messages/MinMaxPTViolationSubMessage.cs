namespace Fanex.CustomerSetting.SettingType18.Models.Messages
{
    using System.Xml.Serialization;
    using CustomerSetting.Services;
    using Resources;
    using Utilities;

    public class MinMaxPTViolationSubMessage : WrongPTSubMessage
    {
        [XmlAttribute]
        public decimal MinPT { get; set; }

        [XmlAttribute]
        public decimal MaxPT { get; set; }

        public override string ToString(IResourceService resourceService, string language, byte settingLevel)
        {
            return string.Format(
                "{0} - {1}: {2} - {3}: {4}",
                base.ToString(resourceService, language, settingLevel),
                IsLive ? Label.MinLive : Label.Min,
                PTFormatter.FormatPT(MinPT),
                IsLive ? Label.MaxLive : Label.Max,
                PTFormatter.FormatPT(MaxPT));
        }
    }
}