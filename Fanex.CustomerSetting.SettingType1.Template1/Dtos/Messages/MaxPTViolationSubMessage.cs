namespace Fanex.CustomerSetting.SettingType1.Template1.Dtos.Messages
{
    using System.Xml.Serialization;
    using Fanex.CustomerSetting.Resource;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.SettingType1.Template1.Utilities;

    public class MaxPTViolationSubMessage : WrongPTSubMessage
    {
        [XmlAttribute]
        public byte MaxPT { get; set; }

        public override string ToString(IResourceService resourceService, string language, byte settingLevel)
        {
            return string.Format(
                "{0} - {1}: {2}",
                base.ToString(resourceService, language, settingLevel),
                IsLive ? Label.MaxLive : Label.Max,
                PositionTakingFormatter.FormatPositionTaking(MaxPT));
        }
    }
}