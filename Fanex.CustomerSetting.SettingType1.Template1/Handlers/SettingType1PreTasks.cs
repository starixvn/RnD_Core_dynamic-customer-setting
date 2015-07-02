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
            IEnumerable<Customer> customers,
            CustomerLevel customerLevel,
            SettingType settingType,
            CustomerLevel? uplineLevel)
        {
            if (customerLevel == CustomerLevel.Member)
            {
                return UpdatePTForGroupedMembers(settingContext, customers, settingType, uplineLevel);
            }
            else
            {
                return new PreUpdateMultipleCustomersSettingTypeResult
                {
                    Result = UpdatingSettingResult.SuccessUpdatingSettingResult
                };
            }
        }

        private PreUpdateMultipleCustomersSettingTypeResult UpdatePTForGroupedMembers(
            ISettingContext settingContext,
            IEnumerable<Customer> customers,
            SettingType settingType,
            CustomerLevel? uplineLevel)
        {
            var groupedMembers = _memberGroupService.GetGroupedMembers(customers);
            var ungroupedMembers = GetUnGroupedMembers(customers, groupedMembers);
            return new PreUpdateMultipleCustomersSettingTypeResult
            {
                CustomersNeedForSingleUpdating = ungroupedMembers,
                IsReturnResultToUser = true,
                Result = UpdateGroupedMembers(settingContext, customers, uplineLevel.Value, settingType)
            };
        }

        private UpdatingSettingResult UpdateGroupedMembers(
            ISettingContext settingContext,
            IEnumerable<Customer> members,
            CustomerLevel uplineLevel,
            SettingType settingType)
        {
            var result = _memberGroupService.UpdateGroupedMembersPT(settingContext, members, uplineLevel, settingType);

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
            [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Reviewed !"), 
            SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Reviewed !")]
            public bool Equals(Customer firstCustomer, Customer secondCustomer)
            {
                return firstCustomer.Id == secondCustomer.Id;
            }

            [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Reviewed !")]
            public int GetHashCode(Customer hashCode)
            {
                return hashCode.Id.GetHashCode();
            }
        }
    }
}