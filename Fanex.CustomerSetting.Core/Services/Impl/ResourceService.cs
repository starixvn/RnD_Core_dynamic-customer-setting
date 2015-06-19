namespace Fanex.CustomerSetting.Core.Services.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Caching;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Core.Dtos;
    using Fanex.CustomerSetting.Core.Exceptions;
    using Fanex.CustomerSetting.Core.Handlers;
    using Fanex.CustomerSetting.Core.Models;
    using Fanex.CustomerSetting.Core.SPObjects.Criteria.Product;
    using Fanex.NAP.Framework.Data;
    using RemotingRef;

    public class ResourceService : IResourceService
    {
        private const string ProductNamesCacheKey = "Fanex.CustomerSetting.ProductResourceCacheKey";
        private const int ProductNamesCacheTimeout = 12 * 60 * 60; // 1 day
        private const string SportTypeDefaultFormat = "Sport Type {0}";
        private const string BetTypeDefaultFormat = "Bet Type {0}";
        private const string ProductDefaultFormat = "Product {0}";
        private readonly IDynamicRepository _resourceRepository;
        private readonly ISettingContext _settingContext;
        private string _refLanguage;

        public ResourceService()
            : this(null, null)
        {
        }

        public ResourceService(ISettingContext settingContext, IDynamicRepository resourceRepository)
        {
            _resourceRepository = resourceRepository ?? new DynamicRepository();
            _settingContext = settingContext ?? SettingContextManager.SettingContext;
            _refLanguage = GetRefLanguage(_settingContext.Language);
        }

        public string GetProductName(byte productId)
        {
            Check.Argument.IsNotNegativeOrZero(productId, "productId");

            var productResources = GetProductResources();
            var productResource = productResources.Where(x => x.ProductId == productId).FirstOrDefault();
            if (productResource != null)
            {
                return GetProductNameWithLanguage(productResource);
            }

            return string.Format(ProductDefaultFormat, productId);
        }

        private string GetProductNameWithLanguage(ProductNameDto productResource)
        {
            var propertyName = string.Format("ProductName{0}", _refLanguage.ToUpper());
            var property = productResource.GetType().GetProperty(propertyName);
            return property == null
                ? productResource.ProductName
                : property.GetValue(productResource, null).ToString();
        }

        public string GetSportTypeName(int sportTypeId)
        {
            Check.Argument.IsNotNegativeOrZero(sportTypeId, "sportTypeId");

            var sportTypeName = RemotingRefApiHandler.RemotingRefApi.GetSportTypeName(sportTypeId.ToString(), _refLanguage);
            return string.IsNullOrEmpty(sportTypeName)
                ? string.Format(SportTypeDefaultFormat, sportTypeId)
                : sportTypeName;
        }

        public string GetBetTypeName(int betTypeId)
        {
            Check.Argument.IsNotNegativeOrZero(betTypeId, "betTypeId");

            var betTypeName = RemotingRefApiHandler.RemotingRefApi.GetBetTypeName(betTypeId.ToString(), _refLanguage);
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
                    var criteria = new ProductNamesQueryCriteria();
                    if (!criteria.IsValid())
                    {
                        throw new StoredProcedureException(criteria);
                    }

                    productNames = _resourceRepository.Fetch<ProductNameDto>(new ProductNamesQueryCriteria());
                    cacheService.Set<IEnumerable<ProductNameDto>>(ProductNamesCacheKey, productNames, ProductNamesCacheTimeout);
                }

                return productNames;
            }
        }
    }
}