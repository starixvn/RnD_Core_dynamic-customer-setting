namespace Fanex.CustomerSetting.Repositories
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Entities;
    using Fanex.Data;

    public class QueryOnMembersRepository : IQueryOnMembersRepository
    {
        public IEnumerable<MemberGroupEntity> SelectMemberGroup(GroupQueryOnMultipleMemberCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                return objectDb.Query<MemberGroupEntity>(criteria.GetSpParams());
            }
        }
    }
}