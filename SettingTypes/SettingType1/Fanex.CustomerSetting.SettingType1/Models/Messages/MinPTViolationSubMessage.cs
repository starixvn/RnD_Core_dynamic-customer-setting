namespace Fanex.CustomerSetting.SettingType1.Models.Messages
{
    using System.Xml.Serialization;
    using CustomerSetting.Services;
    using Resources;
    using Utilities;

    public class MinPTViolationSubMessage : WrongPTSubMessage
    {
        [XmlAttribute]
        public decimal MinPT { get; set; }

        public override string ToString(IResourceService resourceService, string language, byte settingLevel)
        {
            return string.Format(
                "{0} - {1}: {2}",
                base.ToString(resourceService, language, settingLevel),
                IsLive ? Label.MinLive : Label.Min,
                PTFormatter.FormatPT(MinPT));
        }
    }
}