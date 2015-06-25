namespace Fanex.CustomerSetting.SettingType1.Template1.Dtos.Messages
{
    using System.Xml.Serialization;
    using Fanex.CustomerSetting.Resource.Resources;
    using Fanex.CustomerSetting.Resource.Services;
    using Fanex.CustomerSetting.SettingType1.Template1.Utilities;

    public class MinPTViolationMessageRow : WrongPTMessageRow
    {
        [XmlAttribute]
        public byte MinPT { get; set; }

        public override string ToString(IResourceService resourceService, string language, byte settingLevel)
        {
            return string.Format(
                "{0} - {1}: {2}",
                base.ToString(resourceService, language, settingLevel),
                IsLive ? Lang.MinLive : Lang.Min,
                PositionTakingFormatter.FormatPositionTaking(MinPT));
        }
    }
}