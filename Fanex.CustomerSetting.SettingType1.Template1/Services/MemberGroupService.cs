namespace Fanex.CustomerSetting.SettingType1.Template1.Services
{
    using System;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.CustomerSetting.Core.Models;

    public class MemberGroupService : IMemberGroupService
    {
        public IEnumerable<Customer> GetGroupedMembers(IList<Customer> members)
        {
            throw new NotImplementedException();
        }

        public IResult UpdateGroupedMembersSetting(IList<Customer> groupedMembers, SettingType settingType)
        {
            throw new NotImplementedException();
        }
    }
}