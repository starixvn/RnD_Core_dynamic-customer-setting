namespace Fanex.CustomerSetting.SettingType1.Template1.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.CustomerSetting.Core.Enumerations;
    using Fanex.CustomerSetting.Core.Handlers;
    using Fanex.CustomerSetting.Core.Models;
    using Fanex.CustomerSetting.SettingType1.Template1.DataAccesses;
    using Fanex.CustomerSetting.SettingType1.Template1.Dtos;

    public class MemberGroupService : IMemberGroupService
    {
        private readonly ISettingContext _settingContext;
        private readonly IMemberGroupDataAccess _memberGroupDataAccess;

        public MemberGroupService() :
            this(null, null)
        {
        }

        public MemberGroupService(
            ISettingContext settingContext,
            IMemberGroupDataAccess memberGroupDataAccess)
        {
            _settingContext = settingContext ?? SettingContextManager.SettingContext;
            _memberGroupDataAccess = memberGroupDataAccess ?? new MemberGroupDataAccess();
        }

        public IEnumerable<Customer> GetGroupedMembers(IEnumerable<Customer> members)
        {
            Check.Argument.IsNotNull(members, "members");
            Check.Argument.IsNotNegativeOrZero(members.Count(), "members.Count");

            string memberIds = GetMemberIds(members);

            return _memberGroupDataAccess.GetGroupedMembers(memberIds);
        }

        public IResult UpdateGroupedMembersSetting(IEnumerable<Customer> groupedMembers, CustomerLevel uplineLevel, SettingType settingType)
        {
            Check.Argument.IsNotNull(groupedMembers, "groupedMembers");
            Check.Argument.IsNotNegativeOrZero(groupedMembers.Count(), "groupedMembers.Count");

            var updateParams = new UpdateMultipleMemberPositionTakingParams
            {
                CustIds = GetMemberIds(groupedMembers),
                IP = _settingContext.IP,
                UserId = _settingContext.Actor.Id,
                SubAccId = _settingContext.SubAccount.Id,
                UplineLevel = uplineLevel
            };

            return _memberGroupDataAccess.UpdateGroupedMembersSetting(updateParams);
        }

        private string GetMemberIds(IEnumerable<Customer> members)
        {
            return string.Join(",", members.Select(member => member.Id));
        }
    }
}