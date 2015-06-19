namespace Fanex.CustomerSetting.SettingType1.Template1.Services
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.CustomerSetting.Core.Models;

    public interface IMemberGroupService
    {
        IEnumerable<Customer> GetGroupedMembers(IList<Customer> members);

        IResult UpdateGroupedMembersSetting(IList<Customer> groupedMembers, SettingType settingType);
    }
}