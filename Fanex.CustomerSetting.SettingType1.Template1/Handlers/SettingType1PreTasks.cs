namespace Fanex.CustomerSetting.SettingType1.Template1.Handlers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Handlers;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.SettingType1.Template1.Services;

    public class SettingType1PreTasks : SettingPreTasks
    {
        private readonly IMemberGroupService _memberGroupService;

        public SettingType1PreTasks()
            : this(null)
        {
        }

        public SettingType1PreTasks(IMemberGroupService memberGroupService)
        {
            _memberGroupService = memberGroupService ?? new MemberGroupService();
        }

        public override PreUpdateMultipleCustomersSettingTypeResult PreUpdateMultipleCustomersSettingType(
            ISettingContext settingContext,
            IEnumerable<int> customerIds,
            CustomerLevel customerLevel,
            SettingType settingType,
            CustomerLevel? uplineLevel)
        {
            if (customerLevel == CustomerLevel.Member)
            {
                return UpdatePTForGroupedMembers(settingContext, customerIds, settingType, uplineLevel);
            }
            else
            {
                return new PreUpdateMultipleCustomersSettingTypeResult
                {
                    Result = new UpdatingSettingResult(Result.Success)
                };
            }
        }

        private PreUpdateMultipleCustomersSettingTypeResult UpdatePTForGroupedMembers(
            ISettingContext settingContext,
            IEnumerable<int> customerIds,
            SettingType settingType,
            CustomerLevel? uplineLevel)
        {
            var groupedMemberIds = _memberGroupService.GetGroupedMembers(customerIds);
            var ungroupedMemberIds = GetUnGroupedMemberIds(customerIds, groupedMemberIds);
            return new PreUpdateMultipleCustomersSettingTypeResult
            {
                CustomerIdsForSingleUpdating = ungroupedMemberIds,
                IsReturnResultToUser = true,
                Result = UpdateGroupedMembers(settingContext, groupedMemberIds, uplineLevel.Value, settingType)
            };
        }

        private UpdatingSettingResult UpdateGroupedMembers(
            ISettingContext settingContext,
            IEnumerable<int> memberIds,
            CustomerLevel uplineLevel,
            SettingType settingType)
        {
            if (!memberIds.Any())
            {
                return new UpdatingSettingResult(Result.Success);
            }

            return _memberGroupService.UpdateGroupedMembersPT(settingContext, memberIds, uplineLevel, settingType);
        }

        private IEnumerable<int> GetUnGroupedMemberIds(
            IEnumerable<int> originalMemberIds,
            IEnumerable<int> groupedMemberIds)
        {
            return originalMemberIds.Except(groupedMemberIds);
        }
    }
}