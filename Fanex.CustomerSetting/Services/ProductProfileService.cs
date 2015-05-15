namespace Fanex.CustomerSetting.Services
{
    using Fanex.Caching;
    using Fanex.CustomerSetting.Core;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Entities;
    using Fanex.CustomerSetting.Exceptions;
    using Fanex.CustomerSetting.Repositories;

    public class ProductProfileService : IProductProfileService
    {
        private readonly IProductProfileRepository _productProfileRepository;

        public ProductProfileService()
            : this(null)
        {
        }

        public ProductProfileService(IProductProfileRepository productProfileRepository)
        {
            _productProfileRepository = productProfileRepository ?? new ProductProfileRepository();
        }

        public ProductProfileEntity GetProductProfile(byte productId)
        {
            return InternalGetProductProfile(productId);
        }

        private ProductProfileEntity InternalGetProductProfile(byte productId)
        {
            var cacheService = CacheService.CreateInstance(CacheType.MemoryCache);
            var cacheKey = Configuration.GenerateProductProfileCacheKey(productId);
            var productProfile = cacheService.Get<ProductProfileEntity>(cacheKey);
            if (productProfile == null)
            {
                var criteria = new ProductProfileQueryCriteria
                {
                    ProductId = productId
                };

                if (!criteria.IsValid())
                {
                    throw new CriteriaException(criteria);
                }

                productProfile = _productProfileRepository.GetProductProfile(criteria);
                cacheService.Set<ProductProfileEntity>(cacheKey, productProfile, Configuration.CacheTimeout);
            }

            return productProfile;
        }
    }
}