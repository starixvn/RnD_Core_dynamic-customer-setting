namespace Fanex.CustomerSetting.SettingType1.Template1.Dtos.Messages
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Resource.Dtos.Messages;

    public class DownlinePTViolationMessageRoot : MessageRoot<DownlinePTViolationMessageRow>
    {
        public void GroupUsernameBySportTypeBetType()
        {
            var usernameGrouped = new List<DownlinePTViolationMessageRow>();

            var groups = MessageRows.GroupBy(row => new
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
            });
        }
    }
}