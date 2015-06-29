namespace Fanex.CustomerSetting.Resource.Dtos.Messages
{
    using System;
    using System.Xml.Serialization;
    using Fanex.CustomerSetting.Resource.Services;

    public class DefaultMessageRow : IMessageRow
    {
        [XmlAttribute]
        public short SportType { get; set; }

        [XmlAttribute]
        public short BetType { get; set; }

        public virtual string ToString(IResourceService resourceService, string language, byte settingLevel)
        {
            switch (settingLevel)
            {
                case Constants.ProductLevel:
                    return string.Empty;

                case Constants.SportTypeLevel:
                    return resourceService.GetSportTypeName(SportType, language);

                case Constants.BetTypeLevel:
                    return resourceService.GetBetTypeName(BetType, language);

                case Constants.SportTypeBetTypeLevel:
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