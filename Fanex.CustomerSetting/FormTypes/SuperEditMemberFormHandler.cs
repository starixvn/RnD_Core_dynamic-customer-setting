namespace Fanex.CustomerSetting.FormTypes
{
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Exceptions;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Repositories;

    public class SuperEditMemberFormHandler : BaseFormHandler, IFormHandler
    {
        private readonly IQueryRepository _respository = new QueryRepository();

        public bool IsCommandOnMember
        {
            get
            {
                return true;
            }
        }

        public bool HasPermission(QueryInfo queryInfo)
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

            var customerSettingEntity = _respository.SuperQueryMember(criteria);
            UpdateResource(customerSettingEntity.Discounts);
            UpdateResource(customerSettingEntity.BetLimits);
            UpdateResource(customerSettingEntity.PositionTakings);
            return new SuperQueryMemberResult(settingContext, customerSettingEntity);
        }
    }
}