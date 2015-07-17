namespace Fanex.CustomerSetting.SettingType1.Template1.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
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

        public IEnumerable<int> GetGroupedMembers(IEnumerable<int> memberIds)
        {
            Check.Argument.IsNotNull(memberIds, "members");
            Check.Argument.IsNotNegativeOrZero(memberIds.Count(), "members.Count");

            return _memberGroupDataAccess.GetGroupedMemberIds(string.Join(",", memberIds));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Reviewed")]
        public UpdatingSettingResult UpdateGroupedMembersPT(
            ISettingContext settingContext,
            IEnumerable<int> groupedMemberIds,
            UserLevel uplineLevel,
            SettingType settingType)
        {
            Check.Argument.IsNotNull(settingContext, "settingContext");
            Check.Argument.IsNotNull(groupedMemberIds, "groupedMemberIds");
            Check.Argument.IsNotNull(settingType, "settingType");
            Check.Argument.IsNotNegativeOrZero(groupedMemberIds.Count(), "groupedMemberIds.Count");

            var updateParam = new UpdateMultipleMemberPTParam
            {
                CustIds = string.Join(",", groupedMemberIds),
                IP = settingContext.IP,
                UserId = settingContext.Actor.Id,
                SubAccId = settingContext.Actor.SubAccountId,
                UplineLevel = uplineLevel,
                PTXml = settingType.ToXml()
            };

            var updatedResult = _memberGroupDataAccess.UpdateGroupedMembersSetting(updateParam);

            return SeparateResultForEachCustomer(updatedResult, groupedMemberIds);
        }

        private UpdatingSettingResult SeparateResultForEachCustomer(IResult result, IEnumerable<int> groupedMemberIds)
        {
            var subResults = new List<SettingResult>();
            foreach (int memberId in groupedMemberIds)
            {
                var settingResult = new SettingResult(result.Code, result.Message, result.MsgType, memberId, PTConstants.SettingTypeId, Label.PositionTaking);
                subResults.Add(settingResult);
            }

            return new UpdatingSettingResult(subResults);
        }
    }
}