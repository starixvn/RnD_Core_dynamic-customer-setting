namespace Fanex.CustomerSetting.Services
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Entities;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Exceptions;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Repositories;
    using Fanex.CustomerSetting.Utility;

    public class QueryService : IQueryService
    {
        #region Variables

        private readonly IQueryRepository _respository;
        private readonly IResourceService _resourceService;

        #endregion Variables

        #region Constructors

        public QueryService(string languageCode)
            : this(languageCode, null, null)
        {
        }

        public QueryService(string languageCode, IQueryRepository respository, IResourceService resourceService)
        {
            _respository = respository ?? new QueryRepository();
            _resourceService = resourceService ?? new ResourceService(languageCode);
        }

        #endregion Constructors

        public CompanyQuerySuperResult CompanyQuerySuper(QueryInfo model)
        {
            var settingContext = CreateRequestContext(model);
            var criteria = new CompanyQuerySuperCriteria
            {
                CustId = model.TargetCustId,
                ProductId = model.ProductId
            };
            if (!criteria.IsValid())
            {
                throw new CriteriaException(criteria);
            }

            var customerSettingEntity = _respository.CompanyQuerySuper(criteria);
            UpdateResource(customerSettingEntity.Statuses);
            UpdateResource(customerSettingEntity.Discounts);
            UpdateResource(customerSettingEntity.BetLimits);
            UpdateResource(customerSettingEntity.PositionTakings);
            return new CompanyQuerySuperResult(settingContext, customerSettingEntity);
        }

        public SuperQueryMasterResult SuperQueryMaster(QueryInfo model)
        {
            var settingContext = CreateRequestContext(model);
            var criteria = new SuperQueryMasterCriteria(model.TargetCustId, model.ProductId);
            if (!criteria.IsValid())
            {
                throw new CriteriaException(criteria);
            }

            var customerSettingEntity = _respository.SuperQueryMaster(criteria);
            UpdateResource(customerSettingEntity.Statuses);
            UpdateResource(customerSettingEntity.Discounts);
            UpdateResource(customerSettingEntity.BetLimits);
            UpdateResource(customerSettingEntity.PositionTakings);
            return new SuperQueryMasterResult(settingContext, customerSettingEntity);
        }

        public MasterQueryAgentResult MasterQueryAgent(QueryInfo model)
        {
            var settingContext = CreateRequestContext(model);
            var criteria = new MasterQueryAgentCriteria(model.TargetCustId, model.ProductId);
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

        public SuperQueryMemberResult SuperQueryMember(QueryInfo model)
        {
            var settingContext = CreateRequestContext(model);
            var criteria = new QueryMemberCriteria(model.TargetCustId, (int)RoleType.Super, model.ProductId);
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

        public MasterQueryMemberResult MasterQueryMember(QueryInfo model)
        {
            var settingContext = CreateRequestContext(model);
            var criteria = new QueryMemberCriteria(model.TargetCustId, (int)RoleType.Master, model.ProductId);
            if (!criteria.IsValid())
            {
                throw new CriteriaException(criteria);
            }

            var customerSettingEntity = _respository.MasterQueryMember(criteria);
            UpdateResource(customerSettingEntity.Discounts);
            UpdateResource(customerSettingEntity.BetLimits);
            UpdateResource(customerSettingEntity.PositionTakings);
            return new MasterQueryMemberResult(settingContext, customerSettingEntity);
        }

        public AgentQueryMemberResult AgentQueryMember(QueryInfo model)
        {
            var settingContext = CreateRequestContext(model);
            var criteria = new QueryMemberCriteria(model.TargetCustId, (int)RoleType.Agent, model.ProductId);
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

        private RequestContext CreateRequestContext(QueryInfo model)
        {
            return new RequestContext
            {
                TargetCustId = model.TargetCustId,
                FormType = model.FormType,
                ProductId = model.ProductId,
                ProductName = _resourceService.GetProductName(model.ProductId),
                Language = model.Language
            };
        }

        #region Update Resource

        private void UpdateResource(IEnumerable<ProductStatusEntity> statuses)
        {
            statuses.ForEach(status =>
            {
                status.SportTypeName = _resourceService.GetSportTypeName(status.SportType);
                status.BetTypeName = _resourceService.GetBetTypeName(status.BetType);
            });
        }

        private void UpdateResource(IEnumerable<DiscountEntity> discounts)
        {
            discounts.ForEach(discount =>
            {
                discount.SportTypeName = _resourceService.GetSportTypeName(discount.SportType);
                discount.BetTypeName = _resourceService.GetBetTypeName(discount.BetType);
            });
        }

        private void UpdateResource(IEnumerable<BetLimitEntity> betLimits)
        {
            betLimits.ForEach(betLimit =>
            {
                betLimit.SportTypeName = _resourceService.GetSportTypeName(betLimit.SportType);
                betLimit.BetTypeName = _resourceService.GetBetTypeName(betLimit.BetType);
            });
        }

        private void UpdateResource(IEnumerable<PositionTakingEntity> positionTakings)
        {
            positionTakings.ForEach(positionTaking =>
            {
                positionTaking.SportTypeName = _resourceService.GetSportTypeName(positionTaking.SportType);
                positionTaking.BetTypeName = _resourceService.GetBetTypeName(positionTaking.BetType);
            });
        }

        #endregion Update Resource
    }
}