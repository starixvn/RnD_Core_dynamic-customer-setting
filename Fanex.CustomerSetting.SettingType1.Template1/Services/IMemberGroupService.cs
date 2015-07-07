namespace Fanex.CustomerSetting.SettingType1.Template1.Services
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;

    public interface IMemberGroupService
    {
        IEnumerable<int> GetGroupedMembers(IEnumerable<int> memberIds);

        UpdatingSettingResult UpdateGroupedMembersPT(ISettingContext settingContext, IEnumerable<int> groupedMemberIds, CustomerLevel uplineLevel, SettingType settingType);
    }
}