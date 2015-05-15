namespace Fanex.CustomerSetting.FormTypes
{
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Exceptions;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Repositories;

    public class AgentEditMemberFormHandler : BaseFormHandler, IFormHandler
    {
        private readonly IQueryRepository _respository = new QueryRepository();

        public bool IsCommandOnMember
        {
            get
            {
                return true;
            }
        }

        public bool HasPermission(QueryInfo model)
        {
            return !((model.RoleId != RoleType.Company && model.RoleId != RoleType.Agent) || model.TargetRoleId != RoleType.Member);
        }

        public Result Query(QueryInfo queryInfo)
        {
            var settingContext = CreateRequestContext(queryInfo);
            var criteria = new QueryMemberCriteria(queryInfo.TargetCustId, (int)RoleType.Agent, queryInfo.ProductId);
            if (!criteria.IsValid())
            {
                throw new CriteriaException(criteria);
            }

            var customerSettingEntity = _respository.AgentQueryMember(criteria);
            UpdateResource(customerSettingEntity.Statuses);
            UpdateResource(customerSettingEntity.Discounts);
            UpdateResource(customerSettingEntity.BetLimits);
            UpdateResource(customerSettingEntity.PositionTakings);
            return new AgentQueryMemberResult(settingContext, customerSettingEntity);
        }
    }
}