namespace Fanex.CustomerSetting.Services
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Entities;
    using Fanex.CustomerSetting.Exceptions;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Repositories;
    using Fanex.CustomerSetting.Utility;

    /// <summary>
    /// CustomerSettingService class.
    /// </summary>
    public class CustomerSettingQueryService : ICustomerSettingQueryService
    {
        #region Variables

        private readonly ICustomerSettingQueryRepository _respository;
        private readonly IResourceService _resourceService;

        #endregion Variables

        #region Constructors

        public CustomerSettingQueryService(string languageCode)
            : this(languageCode, null, null)
        {
        }

        public CustomerSettingQueryService(string languageCode, ICustomerSettingQueryRepository respository, IResourceService resourceService)
        {
            _respository = respository ?? new CustomerSettingQueryRepository();
            _resourceService = resourceService ?? new ResourceService(languageCode);
        }

        #endregion Constructors

        /// <summary>
        /// Company select super setting.
        /// </summary>
        /// <param name="model">The select model.</param>
        /// <returns>Customer Setting Result.</returns>
        public CompanyQuerySuperResult CompanyQuerySuper(QueryInfo model)
        {
            var settingContext = CreateSettingContext(model);
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

        /// <summary>
        /// Super select master setting.
        /// </summary>
        /// <param name="model">The select model.</param>
        /// <returns>Customer Setting Result.</returns>
        public SuperQueryMasterResult SuperQueryMaster(QueryInfo model)
        {
            var settingContext = CreateSettingContext(model);
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

        /// <summary>
        /// Master select agent setting.
        /// </summary>
        /// <param name="model">The select model.</param>
        /// <returns>Customer Setting Result.</returns>
        public MasterQueryAgentResult MasterQueryAgent(QueryInfo model)
        {
            var settingContext = CreateSettingContext(model);
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

        /// <summary>
        /// Super select member setting.
        /// </summary>
        /// <param name="model">The select model.</param>
        /// <returns>Customer Setting Result.</returns>
        public SuperQueryMemberResult SuperQueryMember(QueryInfo model)
        {
            var settingContext = CreateSettingContext(model);
            var criteria = new SuperQueryMemberCriteria(model.TargetCustId, model.ProductId);
            if (!criteria.IsValid())
            {
                throw new CriteriaException(criteria);
            }

            var customerSettingEntity = _respository.SuperQueryMember(criteria);
            UpdateResource(customerSettingEntity.Statuses);
            UpdateResource(customerSettingEntity.Discounts);
            UpdateResource(customerSettingEntity.BetLimits);
            UpdateResource(customerSettingEntity.PositionTakings);
            return new SuperQueryMemberResult(settingContext, customerSettingEntity);
        }

        /// <summary>
        /// Master select member setting.
        /// </summary>
        /// <param name="model">The select model.</param>
        /// <returns>Customer Setting Result.</returns>
        public MasterQueryMemberResult MasterQueryMember(QueryInfo model)
        {
            var settingContext = CreateSettingContext(model);
            var criteria = new SuperQueryMemberCriteria(model.TargetCustId, model.ProductId);
            if (!criteria.IsValid())
            {
                throw new CriteriaException(criteria);
            }

            var customerSettingEntity = _respository.MasterQueryMember(criteria);
            UpdateResource(customerSettingEntity.Statuses);
            UpdateResource(customerSettingEntity.Discounts);
            UpdateResource(customerSettingEntity.BetLimits);
            UpdateResource(customerSettingEntity.PositionTakings);
            return new MasterQueryMemberResult(settingContext, customerSettingEntity);
        }

        /// <summary>
        /// Agent select member setting.
        /// </summary>
        /// <param name="model">The select model.</param>
        /// <returns>Customer Setting Result.</returns>
        public AgentQueryMemberResult AgentQueryMember(QueryInfo model)
        {
            var settingContext = CreateSettingContext(model);
            var criteria = new SuperQueryMemberCriteria(model.TargetCustId, model.ProductId);
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

        private SettingContext CreateSettingContext(QueryInfo model)
        {
            return new SettingContext
            {
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