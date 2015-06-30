namespace Fanex.CustomerSetting.SettingType1.Template1.Dtos.Messages
{
    using System.Linq;
    using System.Xml.Serialization;
    using Fanex.CustomerSetting.Dtos.Messages;

    [XmlRoot("Root")]
    public class DownlinePTViolationMessageRoot : MessageRoot<DownlinePTViolationMessageRow>
    {
        public void GroupUsernameBySportTypeBetType()
        {
            MessageRows = MessageRows.GroupBy(row => new
            {
                row.SportType,
                row.BetType,
                row.IsLive
            }).Select(group => new DownlinePTViolationMessageRow
            {
                SportType = group.Key.SportType,
                BetType = group.Key.BetType,
                IsLive = group.Key.IsLive,
                UserName = string.Join(", ", group.ToList().Select(row => row.UserName))
            }).ToArray();
        }
    }
}