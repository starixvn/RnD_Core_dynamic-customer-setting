namespace Fanex.CustomerSetting.Services
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Entities;

    public interface IQueryOnMembersService
    {
        IEnumerable<MemberGroupEntity> SelectMemberGroup(IEnumerable<int> custIds);
    }
}