namespace Fanex.CustomerSetting.Resource.Services.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Caching;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Resource.Criteria;
    using Fanex.CustomerSetting.Resource.Dtos;
    using Fanex.NAP.Framework.Data;

    public class ResourceService : IResourceService
    {
        
        private const int ProductNamesCacheTimeout = 12 * 60 * 60; // 1 day
        private const string ProductNamesCacheKey = "Fanex.CustomerSetting.ProductResourceCacheKey";
        private const string SportTypeDefaultFormat = "Sport Type {0}";
        private const string BetTypeDefaultFormat = "Bet Type {0}";
        private const string ProductDefaultFormat = "Product {0}";

        private readonly IDynamicRepository _resourceRepository;
        private readonly IRemotingRef _remotingRef;

        public ResourceService()
            : this(null, null)
        {
        }

        public ResourceService(IDynamicRepository resourceRepository, IRemotingRef remotingRef)
        {
            _resourceRepository = resourceRepository ?? new DynamicRepository();
            _remotingRef = remotingRef ?? new RemotingRefService();
        }

        public string GetProductName(byte productId, string language)
        {
            Check.Argument.IsNotNegativeOrZero(productId, "productId");

            var productResources = GetProductResources();
            var productResource = productResources.Where(x => x.ProductId == productId).FirstOrDefault();
            if (productResource != null)
            {
                return GetProductNameWithLanguage(productResource, language);
            }

            return string.Format(ProductDefaultFormat, productId);
        }

        private string GetProductNameWithLanguage(ProductNameDto productName, string language)
        {
            var propertyName = string.Format("ProductName{0}", GetRefLanguage(language).ToUpper());
            var property = productName.GetType().GetProperty(propertyName);
            return property == null
                ? productName.ProductName
                : property.GetValue(productName, null).ToString();
        }

        public string GetSportTypeName(int sportTypeId, string language)
        {
            Check.Argument.IsNotNegativeOrZero(sportTypeId, "sportTypeId");

            var sportTypeName = _remotingRef.GetSportTypeName(sportTypeId.ToString(), GetRefLanguage(language));
            return string.IsNullOrEmpty(sportTypeName)
                ? string.Format(SportTypeDefaultFormat, sportTypeId)
                : sportTypeName;
        }

        public string GetBetTypeName(int betTypeId, string language)
        {
            Check.Argument.IsNotNegativeOrZero(betTypeId, "betTypeId");

            var betTypeName = _remotingRef.GetBetTypeName(betTypeId.ToString(), GetRefLanguage(language));
            return string.IsNullOrEmpty(betTypeName)
                ? string.Format(BetTypeDefaultFormat, betTypeId)
                : betTypeName;
        }

        private string GetRefLanguage(string language)
        {
            var defaultRefLanguage = "en";
            if (string.IsNullOrEmpty(language))
            {
                return defaultRefLanguage;
            }

            switch (language.ToLower())
            {
                case "en-us": return "en"; // English
                case "zh-tw": return "ch"; // Traditional
                case "zh-cn": return "cs"; // Simplified
                case "ja-jp": return "jp"; // Japanese
                case "th-th": return "th"; // Thai
                default: return defaultRefLanguage;
            }
        }

        private IEnumerable<ProductNameDto> GetProductResources()
        {
            using (var cacheService = CacheService.CreateInstance(CacheType.MemoryCache))
            {
                var productNames = cacheService.Get<IEnumerable<ProductNameDto>>(ProductNamesCacheKey);
                if (productNames == null)
                {
                    productNames = _resourceRepository.Fetch<ProductNameDto>(new ProductNamesQueryCriteria());
                    cacheService.Set<IEnumerable<ProductNameDto>>(ProductNamesCacheKey, productNames, ProductNamesCacheTimeout);
                }

                return productNames;
            }
        }
    }
}