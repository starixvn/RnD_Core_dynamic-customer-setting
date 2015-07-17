namespace Fanex.CustomerSetting.Services.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Caching;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Dtos;
    using Fanex.CustomerSetting.RemotingRefApi;
    using Fanex.CustomerSetting.Resource;
    using Fanex.CustomerSetting.Utilz;
    using Fanex.NAP.Framework.Data;

    public class ResourceService : IResourceService
    {
        private const int ProductNamesCacheTimeout = 12 * 60 * 60; // 1 day
        private const string ProductNamesCacheKey = "Fanex.CustomerSetting.ProductResourceCacheKey_{0}";
        private const string SportTypeDefaultFormat = "Sport Type {0}";
        private const string BetTypeDefaultFormat = "Bet Type {0}";
        private const string ProductDefaultFormat = "Product {0}";
        private const string DefaultRefDataLanguage = "en";

        private readonly IDynamicRepository _resourceRepository;
        private readonly IRemotingRefService _remotingRef;
        private readonly ICache _cacheManager;

        public ResourceService(
            IDynamicRepository resourceRepository = null, 
            IRemotingRefService remotingRef = null, 
            ICache cacheManager = null)
        {
            _resourceRepository = resourceRepository ?? new DynamicRepository();
            _remotingRef = remotingRef ?? new RemotingRefService();
            _cacheManager = cacheManager ?? CacheService.CreateInstance();
        }

        public string GetProductName(byte productId, string language)
        {
            Check.Argument.IsNotNegativeOrZero(productId, "productId");

            var productName = GetProductNameFromCache(productId);

            if (productName != null)
            {
                return GetProductNameWithLanguage(productName, language);
            }

            return string.Format(ProductDefaultFormat, productId);
        }

        private string GetProductNameWithLanguage(ProductName productName, string language)
        {
            var propertyName = string.Format("ProductName{0}", LanguageNameConverter.GetRefLanguageName(language).ToUpper());
            var property = productName.GetType().GetProperty(propertyName);

            if (property == null)
            {
                return productName.ProductNameEN;
            }
            else if (property.GetValue(productName, null) == null)
            {
                return string.Format(Resource.Message.ProductNameMissing, productName.ProductId, language);
            }

            return property.GetValue(productName, null).ToString();
        }

        public string GetSportTypeName(int sportTypeId, string language)
        {
            Check.Argument.IsNotNegativeOrZero(sportTypeId, "sportTypeId");

            var sportTypeName = _remotingRef.GetSportTypeName(sportTypeId.ToString(), LanguageNameConverter.GetRefLanguageName(language).ToUpper());
            return string.IsNullOrEmpty(sportTypeName)
                ? string.Format(SportTypeDefaultFormat, sportTypeId)
                : sportTypeName;
        }

        public string GetBetTypeName(int betTypeId, string language)
        {
            Check.Argument.IsNotNegativeOrZero(betTypeId, "betTypeId");

            var betTypeName = _remotingRef.GetBetTypeName(betTypeId.ToString(), LanguageNameConverter.GetRefLanguageName(language).ToUpper());
            return string.IsNullOrEmpty(betTypeName)
                ? string.Format(BetTypeDefaultFormat, betTypeId)
                : betTypeName;
        }

        private ProductName GetProductNameFromCache(byte productId)
        {
            var cacheKey = string.Format(ProductNamesCacheKey, productId);

            var productName = _cacheManager.Get<ProductName>(cacheKey);

            if (ReferenceEquals(null, productName))
            {
                productName = _resourceRepository.Get<ProductName>(new ProductNamesQueryCriteria { ProductId = productId });

                if (!ReferenceEquals(null, productName))
                {
                    _cacheManager.Set<ProductName>(cacheKey, productName, ProductNamesCacheTimeout);
                }
            }

            return productName;
        }
    }
}