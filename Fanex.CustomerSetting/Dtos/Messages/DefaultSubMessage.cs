namespace Fanex.CustomerSetting.Dtos.Messages
{
    using System;
    using System.Xml.Serialization;
    using Fanex.CustomerSetting.Constants;
    using Fanex.CustomerSetting.Services;

    public class DefaultSubMessage : ISubMessage
    {
        [XmlAttribute]
        public short SportType { get; set; }

        [XmlAttribute]
        public short BetType { get; set; }

        public virtual string ToString(IResourceService resourceService, string language, byte settingLevel)
        {
            switch (settingLevel)
            {
                case SettingLevel.Product:
                    return string.Empty;

                case SettingLevel.SportType:
                    return resourceService.GetSportTypeName(SportType, language);

                case SettingLevel.BetType:
                    return resourceService.GetBetTypeName(BetType, language);

                case SettingLevel.SportTypeBetType:
                    return string.Format(
                        "{0} - {1}",
                        resourceService.GetSportTypeName(SportType, language),
                        resourceService.GetBetTypeName(BetType, language));

                default:
                    throw new InvalidOperationException("Not implement setting level");
            }
        }
    }
}