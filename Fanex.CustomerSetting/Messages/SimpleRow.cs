namespace Fanex.CustomerSetting.Messages
{
    using System.Xml.Serialization;
    using Fanex.CustomerSetting.Services;

    /// <summary>
    /// BaseMessageRow class.
    /// </summary>
    public class SimpleRow
    {
        [XmlAttribute]
        public int SportType { get; set; }

        [XmlAttribute]
        public int BetType { get; set; }

        public virtual string ToString(IResourceService resourceService)
        {
            return string.Format(
                "{0} - {1}",
                resourceService.GetSportTypeName(SportType),
                resourceService.GetBetTypeName(BetType));
        }
    }
}