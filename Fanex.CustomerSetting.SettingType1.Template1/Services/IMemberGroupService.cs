namespace Fanex.CustomerSetting.SettingType1.Template1.Services
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.CustomerSetting.Core.Enumerations;
    using Fanex.CustomerSetting.Core.Models;

    public interface IMemberGroupService
    {
        IEnumerable<Customer> GetGroupedMembers(IEnumerable<Customer> members);

        IResult UpdateGroupedMembersSetting(IEnumerable<Customer> groupedMembers, CustomerLevel uplineLevel, SettingType settingType);
    }
}