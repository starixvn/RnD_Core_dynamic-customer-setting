namespace Fanex.CustomerSetting.FormTypes
{
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Exceptions;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Repositories;
    using Fanex.CustomerSetting.Services;

    public class SuperEditMasterFormHandler : BaseFormHandler, IFormHandler
    {
        private readonly IQueryRepository _queryRepository;

        public SuperEditMasterFormHandler(IQueryRepository queryRepository, IResourceService resourceService)
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
            return !((queryInfo.RoleId != RoleType.Company && queryInfo.RoleId != RoleType.Super) || queryInfo.TargetRoleId != RoleType.Master);
        }

        public Result Query(QueryInfo queryInfo)
        {
            var settingContext = CreateRequestContext(queryInfo);
            var criteria = new SuperQueryMasterCriteria(queryInfo.TargetCustId, queryInfo.ProductId);
            if (!criteria.IsValid())
            {
                throw new CriteriaException(criteria);
            }

            var customerSettingEntity = _queryRepository.SuperQueryMaster(criteria);
            UpdateResource(customerSettingEntity.Statuses);
            UpdateResource(customerSettingEntity.Discounts);
            UpdateResource(customerSettingEntity.BetLimits);
            UpdateResource(customerSettingEntity.PositionTakings);
            return new SuperQueryMasterResult(settingContext, customerSettingEntity);
        }
    }
}