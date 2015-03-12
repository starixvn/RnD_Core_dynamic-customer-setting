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
    public class CustomerSettingSelectService : ICustomerSettingSelectService
    {
        #region Variables

        private readonly ICustomerSettingSelectRepository _respository;
        private readonly IResourceService _resourceService;

        #endregion Variables

        #region Constructors

        public CustomerSettingSelectService(string languageCode)
            : this(languageCode, null, null)
        {
        }

        public CustomerSettingSelectService(string languageCode, ICustomerSettingSelectRepository respository, IResourceService resourceService)
        {
            _respository = respository ?? new CustomerSettingSelectRepository();
            _resourceService = resourceService ?? new ResourceService(languageCode);
        }

        #endregion Constructors

        /// <summary>
        /// Company select super setting.
        /// </summary>
        /// <param name="model">The select model.</param>
        /// <returns>Customer Setting Result.</returns>
        public CompanySelectSuperResult CompanySelectSuper(SelectInfo model)
        {
            var settingContext = CreateSettingContext(model);
            var criteria = new CompanySelectSuperCriteria
            {
                CustId = model.TargetCustId,
                ProductId = model.ProductId
            };
            if (!criteria.IsValid())
            {
                throw new CriteriaException(criteria);
            }

            var customerSettingEntity = _respository.CompanySelectSuper(criteria);
            UpdateResource(customerSettingEntity.Statuses);
            UpdateResource(customerSettingEntity.Discounts);
            UpdateResource(customerSettingEntity.BetLimits);
            UpdateResource(customerSettingEntity.PositionTakings);
            return new CompanySelectSuperResult(settingContext, customerSettingEntity);
        }

        /// <summary>
        /// Super select master setting.
        /// </summary>
        /// <param name="model">The select model.</param>
        /// <returns>Customer Setting Result.</returns>
        public SuperSelectMasterResult SuperSelectMaster(SelectInfo model)
        {
            var settingContext = CreateSettingContext(model);
            var criteria = new SuperSelectMasterCriteria(model.TargetCustId, model.ProductId);
            if (!criteria.IsValid())
            {
                throw new CriteriaException(criteria);
            }

            var customerSettingEntity = _respository.SuperSelectMaster(criteria);
            UpdateResource(customerSettingEntity.Statuses);
            UpdateResource(customerSettingEntity.Discounts);
            UpdateResource(customerSettingEntity.BetLimits);
            UpdateResource(customerSettingEntity.PositionTakings);
            return new SuperSelectMasterResult(settingContext, customerSettingEntity);
        }

        /// <summary>
        /// Master select agent setting.
        /// </summary>
        /// <param name="model">The select model.</param>
        /// <returns>Customer Setting Result.</returns>
        public MasterSelectAgentResult MasterSelectAgent(SelectInfo model)
        {
            var settingContext = CreateSettingContext(model);
            var criteria = new MasterSelectAgentCriteria(model.TargetCustId, model.ProductId);
            if (!criteria.IsValid())
            {
                throw new CriteriaException(criteria);
            }

            var customerSettingEntity = _respository.MasterSelectAgent(criteria);
            UpdateResource(customerSettingEntity.Statuses);
            UpdateResource(customerSettingEntity.Discounts);
            UpdateResource(customerSettingEntity.BetLimits);
            UpdateResource(customerSettingEntity.PositionTakings);
            return new MasterSelectAgentResult(settingContext, customerSettingEntity);
        }

        /// <summary>
        /// Super select member setting.
        /// </summary>
        /// <param name="model">The select model.</param>
        /// <returns>Customer Setting Result.</returns>
        public SuperSelectMemberResult SuperSelectMember(SelectInfo model)
        {
            var settingContext = CreateSettingContext(model);
            var criteria = new SuperSelectMemberCriteria(model.TargetCustId, model.ProductId);
            if (!criteria.IsValid())
            {
                throw new CriteriaException(criteria);
            }

            var customerSettingEntity = _respository.SuperSelectMember(criteria);
            UpdateResource(customerSettingEntity.Statuses);
            UpdateResource(customerSettingEntity.Discounts);
            UpdateResource(customerSettingEntity.BetLimits);
            UpdateResource(customerSettingEntity.PositionTakings);
            return new SuperSelectMemberResult(settingContext, customerSettingEntity);
        }

        /// <summary>
        /// Master select member setting.
        /// </summary>
        /// <param name="model">The select model.</param>
        /// <returns>Customer Setting Result.</returns>
        public MasterSelectMemberResult MasterSelectMember(SelectInfo model)
        {
            var settingContext = CreateSettingContext(model);
            var criteria = new SuperSelectMemberCriteria(model.TargetCustId, model.ProductId);
            if (!criteria.IsValid())
            {
                throw new CriteriaException(criteria);
            }

            var customerSettingEntity = _respository.MasterSelectMember(criteria);
            UpdateResource(customerSettingEntity.Statuses);
            UpdateResource(customerSettingEntity.Discounts);
            UpdateResource(customerSettingEntity.BetLimits);
            UpdateResource(customerSettingEntity.PositionTakings);
            return new MasterSelectMemberResult(settingContext, customerSettingEntity);
        }

        /// <summary>
        /// Agent select member setting.
        /// </summary>
        /// <param name="model">The select model.</param>
        /// <returns>Customer Setting Result.</returns>
        public AgentSelectMemberResult AgentSelectMember(SelectInfo model)
        {
            var settingContext = CreateSettingContext(model);
            var criteria = new SuperSelectMemberCriteria(model.TargetCustId, model.ProductId);
            if (!criteria.IsValid())
            {
                throw new CriteriaException(criteria);
            }

            var customerSettingEntity = _respository.AgentSelectMember(criteria);
            UpdateResource(customerSettingEntity.Statuses);
            UpdateResource(customerSettingEntity.Discounts);
            UpdateResource(customerSettingEntity.BetLimits);
            UpdateResource(customerSettingEntity.PositionTakings);
            return new AgentSelectMemberResult(settingContext, customerSettingEntity);
        }

        private SettingContext CreateSettingContext(SelectInfo model)
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