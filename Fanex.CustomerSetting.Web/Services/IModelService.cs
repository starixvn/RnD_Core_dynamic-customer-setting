namespace Fanex.CustomerSetting.Web.Services
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Entities;
    using Fanex.CustomerSetting.Enumerations;

    public interface IModelService
    {
        IEnumerable<ProductStatusEntity> ParseStatus(string status, FormType formType);

        IEnumerable<DiscountEntity> ParseDiscount(string discount, FormType formType);

        IEnumerable<BetLimitEntity> ParseBetLimit(string betLimit, FormType formType);

        IEnumerable<PositionTakingEntity> ParsePositionTaking(string positionTaking, FormType formType);
    }
}