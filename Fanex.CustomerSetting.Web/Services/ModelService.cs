namespace Fanex.CustomerSetting.Web.Services
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Converters;
    using Fanex.CustomerSetting.Entities;
    using Fanex.CustomerSetting.Enumerations;
    using Newtonsoft.Json;

    public class ModelService : IModelService
    {
        private const int MaxJsonStringLength = 10000;

        public IEnumerable<ProductStatusEntity> ParseStatus(string status, FormType formType)
        {
            if (string.IsNullOrEmpty(status) || status.Length > MaxJsonStringLength)
            {
                return new List<ProductStatusEntity>();
            }

            return JsonConvert.DeserializeObject<List<ProductStatusEntity>>(status, new StatusJsonConverter(formType));
        }

        public IEnumerable<DiscountEntity> ParseDiscount(string discount, FormType formType)
        {
            if (string.IsNullOrEmpty(discount) || discount.Length > MaxJsonStringLength)
            {
                return new List<DiscountEntity>();
            }

            return JsonConvert.DeserializeObject<List<DiscountEntity>>(discount, new DiscountJsonConverter(formType));
        }

        public IEnumerable<BetLimitEntity> ParseBetLimit(string betLimit, FormType formType)
        {
            if (string.IsNullOrEmpty(betLimit) || betLimit.Length > MaxJsonStringLength)
            {
                return new List<BetLimitEntity>();
            }

            return JsonConvert.DeserializeObject<List<BetLimitEntity>>(betLimit, new BetLimitJsonConverter(formType));
        }

        public IEnumerable<PositionTakingEntity> ParsePositionTaking(string positionTaking, FormType formType)
        {
            if (string.IsNullOrEmpty(positionTaking) || positionTaking.Length > MaxJsonStringLength)
            {
                return new List<PositionTakingEntity>();
            }

            return JsonConvert.DeserializeObject<List<PositionTakingEntity>>(positionTaking, new PositionTakingJsonConverter(formType));
        }
    }
}