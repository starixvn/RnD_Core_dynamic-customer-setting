namespace Fanex.CustomerSetting.SettingType1.Template1.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.CustomerSetting.Core.Enumerations;
    using Fanex.CustomerSetting.Core.Handlers;
    using Fanex.CustomerSetting.Core.Models;
    using Fanex.CustomerSetting.SettingType1.Template1.Services;

    public class SettingType1PreTasks : SettingPreTasks
    {
        private readonly IMemberGroupService _memberGroupService;

        public SettingType1PreTasks() :
            this(null, null)
        {
        }

        public SettingType1PreTasks(
            ISettingContext settingContext,
            IMemberGroupService memberGroupService)
            : base(settingContext)
        {
            _memberGroupService = memberGroupService ?? new MemberGroupService();
        }

        public override PreUpdateMultipleSettingResult PreUpdateMultipleSetting(
            IEnumerable<Customer> customers,
            CustomerLevel customerLevel,
            SettingType settingType,
            CustomerLevel? uplineLevel)
        {
            if (customerLevel == CustomerLevel.Member)
            {
                var groupedCustomer = _memberGroupService.GetGroupedMembers(customers);
                var ungroupedCustomer = GetUnGroupedMember(customers, groupedCustomer);
                return new PreUpdateMultipleSettingResult
                    {
                        CustomersNeedForUpdating = ungroupedCustomer,
                        Result = UpdateGroupedMembers(customers, uplineLevel.Value, settingType)
                    };
            }
            else
            {
                return new PreUpdateMultipleSettingResult
                {
                    Result = Result.Success
                };
            }
        }

        private IResult UpdateGroupedMembers(
            IEnumerable<Customer> customers,
            CustomerLevel uplineLevel,
            SettingType settingType)
        {
            var result = _memberGroupService.UpdateGroupedMembersSetting(customers, uplineLevel, settingType);

            return result;
        }

        private IEnumerable<Customer> GetUnGroupedMember(
            IEnumerable<Customer> originalCustomers,
            IEnumerable<Customer> groupedCustomers)
        {
            return originalCustomers.Except(groupedCustomers);
        }
    }
}