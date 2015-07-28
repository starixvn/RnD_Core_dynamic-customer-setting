namespace Fanex.CustomerSetting.Services.Impl
{
    using System;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.RemotingRefApi;
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

            var productName = GetProductNameFromCache(productId);

            if (productName != null)
            {
                return GetProductNameWithLanguage(productName, language);
            }

            return string.Format(ProductDefaultFormat, productId);
        }

        private ProductName GetProductNameFromCache(byte productId)
        {
            var cacheKey = string.Format(ProductNamesCacheKey, productId);

            var productName = _cachingservice.GetOrAdd<ProductName>(
                cacheKey,
                () =>
                {
                    return _resourceRepository.Get<ProductName>(new ProductNamesQueryCriteria { ProductId = productId });
                },
                new TimeSpan(1, 0, 0));

            return productName;
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
            if(sportTypeId <= 0)
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