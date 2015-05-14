namespace Fanex.CustomerSetting.Messages
{
    using System.Xml.Serialization;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;

    public class SimpleRow
    {
        [XmlAttribute]
        public short SportType { get; set; }

        [XmlAttribute]
        public short BetType { get; set; }

        public virtual string ToString(IResourceService resourceService, MessageInfo messageInfo)
        {
            switch (messageInfo.SettingLevel)
            {
                case SettingLevel.SportType:
                    return resourceService.GetSportTypeName(SportType);

                case SettingLevel.BetType:
                    return resourceService.GetBetTypeName(BetType);

                case SettingLevel.SportTypeBetType:
                    return string.Format(
                        "{0} - {1}",
                        resourceService.GetSportTypeName(SportType),
                        resourceService.GetBetTypeName(BetType));
            }

            return string.Empty;
        }
    }
}