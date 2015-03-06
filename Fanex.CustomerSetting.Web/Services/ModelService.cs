namespace Fanex.CustomerSetting.Web.Services
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Converters;
    using Fanex.CustomerSetting.Entities;
    using Fanex.CustomerSetting.Enumerations;
    using Newtonsoft.Json;

    /// <summary>
    /// ModelService class.
    /// </summary>
    public class ModelService : IModelService
    {
        public IEnumerable<ProductStatusEntity> ParseStatus(string statusString, FormType formType)
        {
            if (string.IsNullOrEmpty(statusString))
            {
                return new List<ProductStatusEntity>();
            }

            return JsonConvert.DeserializeObject<List<ProductStatusEntity>>(statusString, new StatusJsonConverter(formType));
        }

        public IEnumerable<DiscountEntity> ParseDiscount(string discountString, FormType formType)
        {
            if (string.IsNullOrEmpty(discountString))
            {
                return new List<DiscountEntity>();
            }

            return JsonConvert.DeserializeObject<List<DiscountEntity>>(discountString, new DiscountJsonConverter(formType));
        }

        public IEnumerable<BetLimitEntity> ParseBetLimit(string betLimitString, FormType formType)
        {
            if (string.IsNullOrEmpty(betLimitString))
            {
                return new List<BetLimitEntity>();
            }

            return JsonConvert.DeserializeObject<List<BetLimitEntity>>(betLimitString, new BetLimitJsonConverter(formType));
        }

        public IEnumerable<PositionTakingEntity> ParsePositionTaking(string positionTakingString, FormType formType)
        {
            if (string.IsNullOrEmpty(positionTakingString))
            {
                return new List<PositionTakingEntity>();
            }

            return JsonConvert.DeserializeObject<List<PositionTakingEntity>>(positionTakingString, new PositionTakingJsonConverter(formType));
        }
    }
}