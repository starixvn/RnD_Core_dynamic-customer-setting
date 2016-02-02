namespace Fanex.CustomerSetting.Services.Impl
{
    using System;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.RemotingRefApi;
    using Fanex.Data.Repository;

    public class ResourceService : IResourceService
    {
        private const int ProductNamesCacheTimeout = 1; // 1 hour
        private const string ProductNamesCacheKey = "Fanex.CustomerSetting.ProductResourceCacheKey_{0}_{1}";
        private const string SportTypeDefaultFormat = "Sport Type {0}";
        private const string BetTypeDefaultFormat = "Bet Type {0}";
        private const string ProductDefaultFormat = "Product {0}";

        private readonly IDynamicRepository _resourceRepository;
        private readonly IRemotingRefService _remotingRef;
        private readonly ICachingService _cachingservice;

        public ResourceService(
            IDynamicRepository resourceRepository = null,
            IRemotingRefService remotingRef = null,
            ICachingService cacheService = null)
        {
            _resourceRepository = resourceRepository ?? new DynamicRepository();
            _remotingRef = remotingRef ?? new RemotingRefService();
            _cachingservice = cacheService ?? new CachingService();
        }

        public string GetProductName(byte productId, string language)
        {
            Check.Argument.IsNotNegativeOrZero(productId, "productId");

            var languageKey = LanguageNameConverter.GetRefLanguageName(language).ToUpper();

            var productName = GetProductNameFromCache(productId, languageKey);

            if (productName != null)
            {
                return productName.Name;
            }

            return string.Format(ProductDefaultFormat, productId);
        }

        private ProductName GetProductNameFromCache(byte productId, string languageKey)
        {
            var cacheKey = string.Format(ProductNamesCacheKey, productId, languageKey);

            var productName = _cachingservice.GetOrAdd<ProductName>(
                cacheKey,
                () =>
                {
                    return _resourceRepository.Get<ProductName>(new ProductNamesQueryCriteria
                    {
                        ProductId = productId,
                        LanguageKey = languageKey
                    });
                },
                new TimeSpan(ProductNamesCacheTimeout, 0, 0));

            return productName;
        }

        public string GetSportTypeName(int sportTypeId, string language)
        {
            if (sportTypeId <= 0)
            {
                return string.Empty;
            }

            var sportTypeName = _remotingRef.GetSportTypeName(sportTypeId.ToString(), LanguageNameConverter.GetRefLanguageName(language).ToUpper());

            return string.IsNullOrEmpty(sportTypeName)
                ? string.Format(SportTypeDefaultFormat, sportTypeId)
                : sportTypeName;
        }

        public string GetBetTypeName(int betTypeId, string language)
        {
            if (betTypeId <= 0)
            {
                return string.Empty;
            }

            var betTypeName = _remotingRef.GetBetTypeName(betTypeId.ToString(), LanguageNameConverter.GetRefLanguageName(language).ToUpper());

            return string.IsNullOrEmpty(betTypeName)
                ? string.Format(BetTypeDefaultFormat, betTypeId)
                : betTypeName;
        }
    }
}