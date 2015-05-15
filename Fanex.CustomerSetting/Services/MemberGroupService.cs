namespace Fanex.CustomerSetting.Services
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Entities;
    using Fanex.CustomerSetting.Exceptions;
    using Fanex.CustomerSetting.Repositories;

    public class MemberGroupService : IMemberGroupService
    {
        private readonly IMemberGroupRepository _memberGroupRepository;

        public MemberGroupService()
            : this(null)
        {
        }

        public MemberGroupService(IMemberGroupRepository memberGroupRepository)
        {
            _memberGroupRepository = memberGroupRepository ?? new MemberGroupRepository();
        }

        public IEnumerable<MemberGroupEntity> SelectMemberGroup(IEnumerable<int> custIds)
        {
            var criteria = new MemberGroupSelectCriteria
            {
                CustIds = string.Join(",", custIds)
            };

            if (!criteria.IsValid())
            {
                throw new CriteriaException(criteria);
            }

            return _memberGroupRepository.SelectMemberGroups(criteria);
        }
    }
}