namespace Fanex.CustomerSetting.SettingType1.Template1.Services
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;

    public interface IMemberGroupService
    {
        IEnumerable<CustomerInfo> GetGroupedMembers(IEnumerable<CustomerInfo> members);

        UpdatingSettingResult UpdateGroupedMembersPT(ISettingContext settingContext, IEnumerable<CustomerInfo> groupedMembers, CustomerLevel uplineLevel, SettingType settingType);
    }
}