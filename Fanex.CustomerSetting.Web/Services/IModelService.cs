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
        List<ProductStatusEntity> ParseStatus(string status, FormType formType);

        List<DiscountEntity> ParseDiscount(string discount, FormType formType);

        List<BetLimitEntity> ParseBetLimit(string betLimit, FormType formType);

        List<PositionTakingEntity> ParsePositionTaking(string positionTaking, FormType formType);
    }
}