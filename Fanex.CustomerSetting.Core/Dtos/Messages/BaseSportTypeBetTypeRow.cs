namespace Fanex.CustomerSetting.Core.Dtos.Messages
{
    using System;
    using System.Xml.Serialization;
    using Fanex.CustomerSetting.Core.Services;

    public class BaseSportTypeBetTypeRow
    {
        [XmlAttribute]
        public short SportType { get; set; }

        [XmlAttribute]
        public short BetType { get; set; }

        public virtual string ToString(IResourceService resourceService, byte settingLevel)
        {
            if (resourceService == null)
            {
                throw new ArgumentNullException("resourceService");
            }

            switch (settingLevel)
            {
                case Constants.ProductLevel:
                    return string.Empty;

                case Constants.SportTypeLevel:
                    return resourceService.GetSportTypeName(SportType);

                case Constants.BetTypeLevel:
                    return resourceService.GetBetTypeName(BetType);

                case Constants.SportTypeBetTypeLevel:
                    return string.Format(
                        "{0} - {1}",
                        resourceService.GetSportTypeName(SportType),
                        resourceService.GetBetTypeName(BetType));

                default:
                    throw new NotImplementedException("Translate Message not implemented yet");
            }
        }
    }
}