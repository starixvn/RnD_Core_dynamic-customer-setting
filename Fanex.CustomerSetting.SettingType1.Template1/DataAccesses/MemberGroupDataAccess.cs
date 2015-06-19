namespace Fanex.CustomerSetting.SettingType1.Template1.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.CustomerSetting.Core.Models;
    using Fanex.CustomerSetting.SettingType1.Template1.Dtos;

    public class MemberGroupDataAccess : IMemberGroupDataAccess
    {
        public IEnumerable<Customer> GetGroupedMembers(string memberIds)
        {
            throw new NotImplementedException();
        }

        public IResult UpdateGroupedMembersSetting(UpdateMultipleMemberPositionTakingParams updateParams)
        {
            throw new NotImplementedException();
        }
    }
}