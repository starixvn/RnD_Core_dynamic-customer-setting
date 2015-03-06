namespace Fanex.CustomerSetting.Web.Services
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Entities;
    using Fanex.CustomerSetting.Enumerations;

    /// <summary>
    /// IModelService class.
    /// </summary>
    public interface IModelService
    {
        IEnumerable<ProductStatusEntity> ParseStatus(string statusString, FormType formType);

        IEnumerable<DiscountEntity> ParseDiscount(string discountString, FormType formType);

        IEnumerable<BetLimitEntity> ParseBetLimit(string betLimitString, FormType formType);

        IEnumerable<PositionTakingEntity> ParsePositionTaking(string positionTakingString, FormType formType);
    }
}