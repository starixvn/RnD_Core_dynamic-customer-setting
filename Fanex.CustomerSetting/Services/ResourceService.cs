namespace Fanex.CustomerSetting.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Caching;
    using Fanex.CustomerSetting.Core;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Entities;
    using Fanex.CustomerSetting.Exceptions;
    using Fanex.CustomerSetting.Repositories;
    using RemotingRef;

    public class ResourceService : IResourceService
    {
        #region Variables

        private readonly IResourceRepository _resourceRepository;
        private readonly ICache _cacheService;

        #endregion Variables

        #region Constructors

        public ResourceService()
            : this(null, null, null)
        {
        }

        public ResourceService(string language)
            : this(null, null, language)
        {
        }

        public ResourceService(IResourceRepository resourceRepository, ICache cacheService, string language)
        {
            _resourceRepository = resourceRepository ?? new ResourceRepository();
            _cacheService = cacheService ?? CacheService.CreateInstance(CacheType.MemoryCache);
            RefLanguage = GetRefLanguage(language);
        }

        #endregion Constructors

        #region Properties

        public string RefLanguage { get; private set; }

        #endregion Properties

        #region Get product name

        public string GetProductName(int productId)
        {
            return GetProductName(productId, RefLanguage);
        }

        public string GetProductName(int productId, string refLanguage)
        {
            var productResources = InternalGetProductResources();
            var productResource = productResources.Where(x => x.ProductID == productId).FirstOrDefault();
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

        public string GetSportTypeName(int sportType)
        {
            return GetSportTypeName(sportType, RefLanguage);
        }

        public string GetSportTypeName(int sportType, string refLanguage)
        {
            var sportTypeName = RefData.GetSportName(sportType.ToString(), refLanguage);
            return string.IsNullOrEmpty(sportTypeName)
                ? string.Format("Sport Type {0}", sportType)
                : sportTypeName;
        }

        #endregion Get sport type name

        #region Get bet type name

        public string GetBetTypeName(int betType)
        {
            return GetBetTypeName(betType, RefLanguage);
        }

        public string GetBetTypeName(int betType, string refLanguage)
        {
            var betTypeName = RefData.GetBetTypeName(betType.ToString(), refLanguage);
            return string.IsNullOrEmpty(betTypeName)
                ? string.Format("Bet Type {0}", betType)
                : betTypeName;
        }

        #endregion Get bet type name

        #region Utils

        private static string GetRefLanguage(string language)
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

        private IEnumerable<ProductResourceEntity> InternalGetProductResources()
        {
            var productResources = _cacheService.Get<IEnumerable<ProductResourceEntity>>(Configuration.ProductResourceCacheKey);
            if (productResources == null)
            {
                var criteria = new ProductResourceQueryCriteria();
                if (!criteria.IsValid())
                {
                    throw new CriteriaException(criteria);
                }

                productResources = _resourceRepository.GetProductResources(criteria);
                _cacheService.Set<IEnumerable<ProductResourceEntity>>(Configuration.ProductResourceCacheKey, productResources, Configuration.CacheTimeout);
            }

            return productResources;
        }

        #endregion Utils
    }
}