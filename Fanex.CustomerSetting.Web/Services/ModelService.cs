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
        public List<ProductStatusEntity> ParseStatus(string status, FormType formType)
        {
            if (string.IsNullOrEmpty(status))
            {
                return new List<ProductStatusEntity>();
            }

            return JsonConvert.DeserializeObject<List<ProductStatusEntity>>(status, new StatusJsonConverter(formType));
        }

        public List<DiscountEntity> ParseDiscount(string discount, FormType formType)
        {
            if (string.IsNullOrEmpty(discount))
            {
                return new List<DiscountEntity>();
            }

            return JsonConvert.DeserializeObject<List<DiscountEntity>>(discount, new DiscountJsonConverter(formType));
        }

        public List<BetLimitEntity> ParseBetLimit(string betLimit, FormType formType)
        {
            if (string.IsNullOrEmpty(betLimit))
            {
                return new List<BetLimitEntity>();
            }

            return JsonConvert.DeserializeObject<List<BetLimitEntity>>(betLimit, new BetLimitJsonConverter(formType));
        }

        public List<PositionTakingEntity> ParsePositionTaking(string positionTaking, FormType formType)
        {
            if (string.IsNullOrEmpty(positionTaking))
            {
                return new List<PositionTakingEntity>();
            }

            return JsonConvert.DeserializeObject<List<PositionTakingEntity>>(positionTaking, new PositionTakingJsonConverter(formType));
        }
    }
}