namespace Fanex.CustomerSetting.SettingType1.Template1.Dtos.Messages
{
    using System.Linq;
    using System.Xml.Serialization;
    using Fanex.CustomerSetting.Messages;

    [XmlRoot("Root")]
    public class DownlinePTViolationMainMessage : MainMessage<DownlinePTViolationSubMessage>
    {
        public void GroupUsernameBySportTypeBetType()
        {
            SubMessages = SubMessages.GroupBy(row => new
            {
                row.SportType,
                row.BetType,
                row.IsLive
            }).Select(group => new DownlinePTViolationSubMessage
            {
                SportType = group.Key.SportType,
                BetType = group.Key.BetType,
                IsLive = group.Key.IsLive,
                UserName = string.Join(", ", group.ToList().Select(row => row.UserName))
            }).ToArray();
        }
    }
}