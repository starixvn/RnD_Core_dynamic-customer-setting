namespace Fanex.CustomerSetting.SettingType1.Template1.Services
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;

    public interface IMemberGroupService
    {
        IEnumerable<Customer> GetGroupedMembers(IEnumerable<Customer> members);

        IResult UpdateGroupedMembersSetting(ISettingContext settingContext, IEnumerable<Customer> groupedMembers, CustomerLevel uplineLevel, SettingType settingType);
    }
}