namespace Fanex.CustomerSetting.FormTypes
{
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Exceptions;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Repositories;

    public class MasterEditAgentFormHandler : BaseFormHandler, IFormHandler
    {
        private readonly IQueryRepository _respository = new QueryRepository();

        public bool IsCommandOnMember
        {
            get
            {
                return false;
            }
        }

        public bool HasPermission(QueryInfo queryInfo)
        {
            return !((queryInfo.RoleId != RoleType.Company && queryInfo.RoleId != RoleType.Master) || queryInfo.TargetRoleId != RoleType.Agent);
        }

        public Result Query(QueryInfo queryInfo)
        {
            var settingContext = CreateRequestContext(queryInfo);
            var criteria = new MasterQueryAgentCriteria(queryInfo.TargetCustId, queryInfo.ProductId);
            if (!criteria.IsValid())
            {
                throw new CriteriaException(criteria);
            }

            var customerSettingEntity = _respository.MasterQueryAgent(criteria);
            UpdateResource(customerSettingEntity.Statuses);
            UpdateResource(customerSettingEntity.Discounts);
            UpdateResource(customerSettingEntity.BetLimits);
            UpdateResource(customerSettingEntity.PositionTakings);
            return new MasterQueryAgentResult(settingContext, customerSettingEntity);
        }
    }
}