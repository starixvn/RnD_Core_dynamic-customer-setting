namespace Fanex.CustomerSetting.Messages
{
    using System.Xml.Serialization;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resources;
    using Fanex.CustomerSetting.Services;

    public class PositionTakingRow : SimpleRow
    {
        [XmlAttribute]
        public bool IsLive { get; set; }

        public override string ToString(IResourceService resourceService, MessageInfo messageInfo)
        {
            var message = base.ToString(resourceService, messageInfo);
            if (IsLive && (messageInfo.SettingLevel == SettingLevel.BetType || messageInfo.SettingLevel == SettingLevel.SportTypeBetType))
            {
                message += string.Format(" ({0})", Lang.Live);
            }

            return message;
        }
    }
}