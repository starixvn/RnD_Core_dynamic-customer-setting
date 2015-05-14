namespace Fanex.CustomerSetting.Services
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Entities;
    using Fanex.CustomerSetting.Exceptions;
    using Fanex.CustomerSetting.Repositories;

    public class QueryOnMembersService : IQueryOnMembersService
    {
        private IQueryOnMembersRepository _repository;

        public QueryOnMembersService()
            : this(null)
        {
        }

        public QueryOnMembersService(IQueryOnMembersRepository repository)
        {
            _repository = repository ?? new QueryOnMembersRepository();
        }

        public IEnumerable<MemberGroupEntity> SelectMemberGroup(IEnumerable<int> custIds)
        {
            var criteria = new GroupQueryOnMultipleMemberCriteria
            {
                CustIds = string.Join(",", custIds)
            };

            if (!criteria.IsValid())
            {
                throw new CriteriaException(criteria);
            }

            return _repository.SelectMemberGroup(criteria);
        }
    }
}