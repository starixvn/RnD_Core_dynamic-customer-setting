namespace Fanex.CustomerSetting.SettingType1.Template1.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Handlers;
    using Fanex.CustomerSetting.Models;
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

        public override PreUpdateMultipleCustomersSettingTypeResult PreUpdateMultipleCustomersSettingType(
            IEnumerable<Customer> customers,
            CustomerLevel customerLevel,
            SettingType settingType,
            CustomerLevel? uplineLevel)
        {
            if (customerLevel == CustomerLevel.Member)
            {
                var groupedMembers = _memberGroupService.GetGroupedMembers(customers);
                var ungroupedMembers = GetUnGroupedMembers(customers, groupedMembers);
                return new PreUpdateMultipleCustomersSettingTypeResult
                    {
                        CustomersNeedForSingleUpdating = ungroupedMembers,
                        Result = UpdateGroupedMembers(customers, uplineLevel.Value, settingType)
                    };
            }
            else
            {
                return new PreUpdateMultipleCustomersSettingTypeResult
                {
                    Result = Result.Success
                };
            }
        }

        private IResult UpdateGroupedMembers(
            IEnumerable<Customer> members,
            CustomerLevel uplineLevel,
            SettingType settingType)
        {
            var result = _memberGroupService.UpdateGroupedMembersSetting(members, uplineLevel, settingType);

            return result;
        }

        private IEnumerable<Customer> GetUnGroupedMembers(
            IEnumerable<Customer> originalMembers,
            IEnumerable<Customer> groupedMembers)
        {
            return originalMembers.Except(groupedMembers, new CustomerComparer());
        }

        private class CustomerComparer : IEqualityComparer<Customer>
        {
            public bool Equals(Customer firstCustomer, Customer secondCustomer)
            {
                return firstCustomer.Id == secondCustomer.Id;
            }

            public int GetHashCode(Customer hashCode)
            {
                return hashCode.Id.GetHashCode();
            }
        }
    }
}