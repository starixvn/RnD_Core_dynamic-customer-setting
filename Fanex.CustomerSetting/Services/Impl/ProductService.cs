namespace Fanex.CustomerSetting.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Models;
    using Fanex.Data.Repository;

    public class ProductService : IProductService
    {
        private const int CacheDuration = 1; //// 1 hour
        private const string SettingTypesCacheKey = "Fanex.CustomerSetting.SettingTypesCacheKey_{0}";
        private const string AllProductNamesCacheKey = "Fanex.CustomerSetting.AllProductNamesCacheKey";

        private readonly IDynamicRepository _productRepository;
        private readonly ICachingService _cachingService;

        public ProductService(IDynamicRepository productRepository = null, ICachingService cachingService = null)
        {
            _productRepository = productRepository ?? new DynamicRepository();
            _cachingService = cachingService ?? new CachingService();
        }

        public IEnumerable<SettingType> GetProductSettingTypes(byte productId)
        {
            Check.Argument.IsNotNegativeOrZero(productId, "productId");

            var settingTypes = _cachingService.GetOrAdd(
                   string.Format(SettingTypesCacheKey, productId),
                   () =>
                   {
                       return _productRepository.Fetch<SettingType>(
                           new ProductSettingTypesQueryCriteria
                       {
                           ProductId = productId
                       });
                   },
                   new TimeSpan(CacheDuration, 0, 0));

            return settingTypes;
        }

        public SettingType GetProductSettingType(byte productId, short settingTypeId)
        {
            var productSettingTypes = GetProductSettingTypes(productId);

            if (productSettingTypes == null)
            {
                return null;
            }

            return productSettingTypes.FirstOrDefault(settingType => settingType.Id == settingTypeId);
        }

        public IEnumerable<Product> GetAllProducts()
        {
            var products = _cachingService.GetOrAdd(
                  AllProductNamesCacheKey,
                  () =>
                  {
                      return _productRepository
                          .Fetch<ProductName>(new ProductNamesQueryCriteria { LanguageKey = LanguageNameConverter.GetRefLanguageName() })
                          .Select<ProductName, Product>(productName => new Product { Id = productName.ProductId, Name = productName.Name });
                  },
                  new TimeSpan(CacheDuration, 0, 0));

            return products;
        }
    }
}