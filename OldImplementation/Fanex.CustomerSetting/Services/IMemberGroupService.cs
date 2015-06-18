namespace Fanex.CustomerSetting.Services
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Entities;

    public interface IMemberGroupService
    {
        IEnumerable<MemberGroupEntity> SelectMemberGroup(IEnumerable<int> custIds);
    }
}