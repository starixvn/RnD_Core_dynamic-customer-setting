namespace Fanex.CustomerSetting.SettingType1.Template1.DataAccesses
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.SettingType1.Template1.Dtos;

    public interface IMemberGroupDataAccess
    {
        IEnumerable<Customer> GetGroupedMembers(string memberIds);

        IResult UpdateGroupedMembersSetting(UpdateMultipleMemberPositionTakingParams updateParams);
    }
}