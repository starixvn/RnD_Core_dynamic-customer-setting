namespace Fanex.CustomerSetting.SettingType1.Template1.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Handlers;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resource;
    using Fanex.CustomerSetting.SettingType1.Template1.Constants;
    using Fanex.CustomerSetting.SettingType1.Template1.DataAccesses;
    using Fanex.CustomerSetting.SettingType1.Template1.Dtos;

    public class MemberGroupService : IMemberGroupService
    {
        private readonly IMemberGroupDataAccess _memberGroupDataAccess;

        public MemberGroupService(IMemberGroupDataAccess memberGroupDataAccess = null)
        {
            _memberGroupDataAccess = memberGroupDataAccess ?? new MemberGroupDataAccess();
        }

        public IEnumerable<Customer> GetGroupedMembers(IEnumerable<Customer> members)
        {
            Check.Argument.IsNotNull(members, "members");
            Check.Argument.IsNotNegativeOrZero(members.Count(), "members.Count");

            string memberIds = GetMemberIds(members);

            return _memberGroupDataAccess.GetGroupedMembers(memberIds);
        }

        public UpdatingSettingResult UpdateGroupedMembersSetting(
            ISettingContext settingContext, 
            IEnumerable<Customer> groupedMembers, 
            CustomerLevel uplineLevel, 
            SettingType settingType)
        {
            Check.Argument.IsNotNull(settingContext, "settingContext");
            Check.Argument.IsNotNull(groupedMembers, "groupedMembers");
            Check.Argument.IsNotNull(settingType, "settingType");
            Check.Argument.IsNotNegativeOrZero(groupedMembers.Count(), "groupedMembers.Count");

            var updateParam = new UpdateMultipleMemberPTParam
            {
                CustIds = GetMemberIds(groupedMembers),
                IP = settingContext.IP,
                UserId = settingContext.Actor.Id,
                SubAccId = settingContext.Actor.SubAccountId,
                UplineLevel = uplineLevel,
                PositionTakingXml = settingType.ToXml()
            };

            var updatedResult = _memberGroupDataAccess.UpdateGroupedMembersSetting(updateParam);

            return SeparateResultForEachCustomer(updatedResult, groupedMembers);
        }

        private string GetMemberIds(IEnumerable<Customer> members)
        {
            return string.Join(",", members.Select(member => member.Id));
        }

        private UpdatingSettingResult SeparateResultForEachCustomer(IResult result, IEnumerable<Customer> groupedMembers)
        {
            var subResults = new List<SettingResult>();
            foreach (Customer member in groupedMembers)
            {
                var settingResult = new SettingResult(result.Code, result.Message, result.MsgType, member.Id, member.UserName, SettingType1Constants.SettingType, Label.PositionTaking);
                subResults.Add(settingResult);
            }

            return new UpdatingSettingResult(subResults);
        }
    }
}