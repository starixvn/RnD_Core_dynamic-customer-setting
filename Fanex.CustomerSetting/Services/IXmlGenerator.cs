namespace Fanex.CustomerSetting.Services
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Entities;

    /// <summary>
    /// IXmlGenerator class.
    /// </summary>
    public interface IXmlGenerator
    {
        string GenerateStatus(IEnumerable<ProductStatusEntity> entities);

        string GenerateDiscount(IEnumerable<DiscountEntity> entities);

        string GenerateBetLimit(IEnumerable<BetLimitEntity> entities);

        string GeneratePositionTaking(IEnumerable<PositionTakingEntity> entities);
    }
}