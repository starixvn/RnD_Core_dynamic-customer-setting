namespace Fanex.CustomerSetting.Services
{
    using Fanex.Caching;
    using Fanex.CustomerSetting.Core;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Entities;
    using Fanex.CustomerSetting.Exceptions;
    using Fanex.CustomerSetting.Repositories;

    /// <summary>
    /// ProductProfileService class.
    /// </summary>
    public class ProductProfileService : IProductProfileService
    {
        #region Variables

        private ICache _cacheService;
        private IProductProfileRepository _repository;

        #endregion Variables

        #region Constructors

        public ProductProfileService()
            : this(null, null)
        {
        }

        public ProductProfileService(IProductProfileRepository repository, ICache cacheService)
        {
            _repository = repository ?? new ProductProfileRepository();
            _cacheService = cacheService ?? CacheService.CreateInstance(CacheType.MemoryCache);
        }

        #endregion Constructors

        #region Methods

        public ProductProfileEntity GetProductProfile(byte productId)
        {
            return InternalGetProductProfile(productId);
        }

        /// <summary>
        /// Internals the get product profile.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>Returns product profile.</returns>
        private ProductProfileEntity InternalGetProductProfile(byte productId)
        {
            var cacheKey = Configuration.GenerateProductProfileCacheKey(productId);
            var productProfile = _cacheService.Get<ProductProfileEntity>(cacheKey);
            if (productProfile == null)
            {
                var criteria = new ProductProfileSelectCriteria
                {
                    ProductId = productId
                };

                if (!criteria.IsValid())
                {
                    throw new CriteriaException(criteria);
                }

                productProfile = _repository.GetProductProfile(criteria);
                _cacheService.Set<ProductProfileEntity>(cacheKey, productProfile, Configuration.CacheTimeout);
            }

            return productProfile;
        }

        #endregion Methods
    }
}