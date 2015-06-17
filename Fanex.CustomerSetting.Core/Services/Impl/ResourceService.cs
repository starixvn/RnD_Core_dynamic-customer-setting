namespace Fanex.CustomerSetting.Core.Services.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Caching;
    using Fanex.CustomerSetting.Core.Dtos;
    using Fanex.CustomerSetting.Core.Exceptions;
    using Fanex.CustomerSetting.Core.Handlers;
    using Fanex.CustomerSetting.Core.Models;
    using Fanex.CustomerSetting.Core.SPObjects.Criteria.Product;
    using Fanex.NAP.Framework.Data;
    using RemotingRef;

    public class ResourceService : IResourceService
    {
        private const string ProductResourceCacheKey = "ProductResourceCacheKey";
        private const int CacheTimeout = 12 * 60 * 60; // 1 day
        private readonly IDynamicRepository _resourceRepository;
        private readonly ISettingContext _settingContext;

        public ResourceService()
            : this(null, null)
        {
        }

        public ResourceService(ISettingContext settingContext, IDynamicRepository resourceRepository)
        {
            _resourceRepository = resourceRepository ?? new DynamicRepository();
            _settingContext = settingContext ?? SettingContextManager.SettingContext;
            RefLanguage = GetRefLanguage(_settingContext.Language);
        }

        public string RefLanguage { get; private set; }

        #region Get product name

        public string GetProductName(int productId)
        {
            return GetProductName(productId, RefLanguage);
        }

        public string GetProductName(int productId, string refLanguage)
        {
            var productResources = InternalGetProductResources();
            var productResource = productResources.Where(x => x.ProductId == productId).FirstOrDefault();
            if (productResource != null)
            {
                var propertyName = string.Format("ProductName{0}", refLanguage.ToUpper());
                var property = productResource.GetType().GetProperty(propertyName);
                return property == null
                    ? productResource.ProductName
                    : property.GetValue(productResource, null).ToString();
            }

            return string.Format("Product {0}", productId);
        }

        #endregion Get product name

        #region Get sport type name

        public string GetSportTypeName(int sportTypeId)
        {
            return GetSportTypeName(sportTypeId, RefLanguage);
        }

        public string GetSportTypeName(int sportTypeId, string refLanguage)
        {
            var sportTypeName = RefData.GetSportName(sportTypeId.ToString(), refLanguage);
            return string.IsNullOrEmpty(sportTypeName)
                ? string.Format("Sport Type {0}", sportTypeId)
                : sportTypeName;
        }

        #endregion Get sport type name

        #region Get bet type name

        public string GetBetTypeName(int betTypeId)
        {
            return GetBetTypeName(betTypeId, RefLanguage);
        }

        public string GetBetTypeName(int betTypeId, string refLanguage)
        {
            var betTypeName = RefData.GetBetTypeName(betTypeId.ToString(), refLanguage);
            return string.IsNullOrEmpty(betTypeName)
                ? string.Format("Bet Type {0}", betTypeId)
                : betTypeName;
        }

        #endregion Get bet type name

        #region Utils

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

        private IEnumerable<ProductNameDto> InternalGetProductResources()
        {
            var cacheService = CacheService.CreateInstance(CacheType.MemoryCache);
            var productResources = cacheService.Get<IEnumerable<ProductNameDto>>(ProductResourceCacheKey);
            if (productResources == null)
            {
                var criteria = new ProductResourceQueryCriteria();
                if (!criteria.IsValid())
                {
                    throw new StoredProcedureException(criteria);
                }

                productResources = _resourceRepository.Fetch<ProductNameDto>(new ProductResourceQueryCriteria());
                cacheService.Set<IEnumerable<ProductNameDto>>(ProductResourceCacheKey, productResources, CacheTimeout);
            }

            return productResources;
        }

        #endregion Utils
    }
}