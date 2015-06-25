namespace Fanex.CustomerSetting.SettingType1.Template1.Dtos.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Fanex.CustomerSetting.Resource.Services;

    public class DownlinePTViolationMessageRow : WrongPTMessageRow
    {
        [XmlAttribute]
        public string UserName { get; set; }

        public override string ToString(IResourceService resourceService, string language, byte settingLevel)
        {
            return string.Format(
                "{0} - {1}: {2}",
                base.ToString(resourceService, language, settingLevel),
                UserName);
        }
    }
}
