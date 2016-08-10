namespace Fanex.CustomerSetting.SettingType10.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Results;
    using Resources;

    public class MemberCommissionGroupSettingType : SettingType
    {
        private const string CommissionWithUserGroupXmlFormat = "<ROOT><UserGroup Group=\"{0}\"/>{1}</ROOT>";

        public string UserGroup { get; set; }

        public IEnumerable<string> AvailableGroups { get; set; }

        public override string ToXml()
        {
            if (!string.IsNullOrEmpty(UserGroup))
            {
                var settingItemsXml = string.Join(string.Empty, SettingItems.Select(settingItem => settingItem.ToXml()));

                return string.Format(CommissionWithUserGroupXmlFormat, UserGroup, settingItemsXml);
            }

            return base.ToXml();
        }

        public override IResult Validate()
        {
            if (!string.IsNullOrEmpty(UserGroup))
            {
                if (!AvailableGroups.Any())
                {
                    return new Result(ErrorCode.UnableToChangeUserGroup, Message.UnableToChangeUserGroup);
                }

                if (!AvailableGroups.Contains(UserGroup))
                {
                    return new Result(ErrorCode.InvalidUserGroup, Message.InvalidUserGroup);
                }
            }

            return base.Validate();
        }
    }
}