namespace Fanex.CustomerSetting.FormTypes
{
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Exceptions;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Repositories;
    using Fanex.CustomerSetting.Services;

    public class CompanyEditSuperFormHandler : BaseFormHandler, IFormHandler
    {
        private readonly IQueryRepository _queryRepository;

        public CompanyEditSuperFormHandler(IQueryRepository queryRepository, IResourceService resourceService)
            : base(resourceService)
        {
            _queryRepository = queryRepository;
        }

        public bool IsCommandOnMember
        {
            get
            {
                return false;
            }
        }

        public bool HasValidRole(QueryInfo queryInfo)
        {
            return !(queryInfo.RoleId != RoleType.Company || queryInfo.TargetRoleId != RoleType.Super);
        }

        public Result Query(QueryInfo queryInfo)
        {
            var settingContext = CreateRequestContext(queryInfo);
            var criteria = new CompanyQuerySuperCriteria
            {
                CustId = queryInfo.TargetCustId,
                ProductId = queryInfo.ProductId
            };
            if (!criteria.IsValid())
            {
                throw new CriteriaException(criteria);
            }

            var customerSettingEntity = _queryRepository.CompanyQuerySuper(criteria);
            UpdateResource(customerSettingEntity.Statuses);
            UpdateResource(customerSettingEntity.Discounts);
            UpdateResource(customerSettingEntity.BetLimits);
            UpdateResource(customerSettingEntity.PositionTakings);
            return new CompanyQuerySuperResult(settingContext, customerSettingEntity);
        }
    }
}