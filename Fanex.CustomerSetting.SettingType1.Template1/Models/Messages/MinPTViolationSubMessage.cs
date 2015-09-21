namespace Fanex.CustomerSetting.SettingType1.Models.Messages
{
    using System.Xml.Serialization;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.SettingType1.Resources;
    using Fanex.CustomerSetting.SettingType1.Utilities;

    public class MinPTViolationSubMessage : WrongPTSubMessage
    {
        [XmlAttribute]
        public byte MinPT { get; set; }

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