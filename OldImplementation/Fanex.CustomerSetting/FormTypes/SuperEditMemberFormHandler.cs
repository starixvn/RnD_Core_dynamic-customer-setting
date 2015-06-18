namespace Fanex.CustomerSetting.FormTypes
{
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Exceptions;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Repositories;
    using Fanex.CustomerSetting.Services;

    public class SuperEditMemberFormHandler : BaseFormHandler, IFormHandler
    {
        private readonly IQueryRepository _queryRepository;

        public SuperEditMemberFormHandler(IQueryRepository queryRepository, IResourceService resourceService)
            : base(resourceService)
        {
            _queryRepository = queryRepository;
        }

        public bool IsCommandOnMember
        {
            get
            {
                return true;
            }
        }

        public bool HasValidRole(QueryInfo queryInfo)
        {
            return !((queryInfo.RoleId != RoleType.Company && queryInfo.RoleId != RoleType.Super) || queryInfo.TargetRoleId != RoleType.Member);
        }

        public Result Query(QueryInfo queryInfo)
        {
            var settingContext = CreateRequestContext(queryInfo);
            var criteria = new QueryMemberCriteria(queryInfo.TargetCustId, (int)RoleType.Super, queryInfo.ProductId);
            if (!criteria.IsValid())
            {
                throw new CriteriaException(criteria);
            }

            var customerSettingEntity = _queryRepository.SuperQueryMember(criteria);
            UpdateResource(customerSettingEntity.Discounts);
            UpdateResource(customerSettingEntity.BetLimits);
            UpdateResource(customerSettingEntity.PositionTakings);
            return new SuperQueryMemberResult(settingContext, customerSettingEntity);
        }
    }
}