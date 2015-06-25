namespace Fanex.CustomerSetting.SettingType1.Template1.Dtos.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Fanex.CustomerSetting.Resource.Resources;
    using Fanex.CustomerSetting.Resource.Services;
    using Fanex.CustomerSetting.SettingType1.Template1.Utilities;

    public class MaxPTViolationMessageRow : WrongPTMessageRow
    {
        [XmlAttribute]
        public byte MaxPT { get; set; }

        public override string ToString(IResourceService resourceService, string language, byte settingLevel)
        {
            return string.Format(
                "{0} - {1}: {2}",
                base.ToString(resourceService, language, settingLevel),
                IsLive ? Lang.MaxLive : Lang.Max,
                PositionTakingFormatter.FormatPositionTaking(MaxPT));
        }
    }
}
