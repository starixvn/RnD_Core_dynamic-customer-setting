namespace Fanex.CustomerSetting.FormTypes
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Entities;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Utility;

    public abstract class BaseFormHandler
    {
        private readonly IResourceService _resourceService;

        protected BaseFormHandler(IResourceService resourceService)
        {
            _resourceService = resourceService;
        }

        protected RequestContext CreateRequestContext(QueryInfo queryInfo)
        {
            return new RequestContext
            {
                TargetCustId = queryInfo.TargetCustId,
                FormType = queryInfo.FormType,
                ProductId = queryInfo.ProductId,
                ProductName = _resourceService.GetProductName(queryInfo.ProductId),
                Language = queryInfo.Language
            };
        }

        protected void UpdateResource(IEnumerable<ProductStatusEntity> statuses)
        {
            statuses.ForEach(status =>
            {
                status.SportTypeName = _resourceService.GetSportTypeName(status.SportType);
                status.BetTypeName = _resourceService.GetBetTypeName(status.BetType);
            });
        }

        protected void UpdateResource(IEnumerable<DiscountEntity> discounts)
        {
            discounts.ForEach(discount =>
            {
                discount.SportTypeName = _resourceService.GetSportTypeName(discount.SportType);
                discount.BetTypeName = _resourceService.GetBetTypeName(discount.BetType);
            });
        }

        protected void UpdateResource(IEnumerable<BetLimitEntity> betLimits)
        {
            betLimits.ForEach(betLimit =>
            {
                betLimit.SportTypeName = _resourceService.GetSportTypeName(betLimit.SportType);
                betLimit.BetTypeName = _resourceService.GetBetTypeName(betLimit.BetType);
            });
        }

        protected void UpdateResource(IEnumerable<PositionTakingEntity> positionTakings)
        {
            positionTakings.ForEach(positionTaking =>
            {
                positionTaking.SportTypeName = _resourceService.GetSportTypeName(positionTaking.SportType);
                positionTaking.BetTypeName = _resourceService.GetBetTypeName(positionTaking.BetType);
            });
        }
    }
}