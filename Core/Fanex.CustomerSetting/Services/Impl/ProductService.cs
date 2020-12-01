namespace Fanex.CustomerSetting.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Helper;
    using Criteria;
    using Data.Repository;
    using Models;

    public class ProductService : IProductService
    {
        private const int CacheDuration = 1; //// 1 hour
        private const string SettingTypesCacheKey = "Fanex.CustomerSetting.SettingTypesCacheKey_{0}_{1}";
        private const string AllProductNamesCacheKey = "Fanex.CustomerSetting.AllProductNamesCacheKey";

        private readonly IDynamicRepository _productRepository;
        private readonly ICachingService _cachingService;

        public ProductService(IDynamicRepository productRepository = null, ICachingService cachingService = null)
        {
            _productRepository = productRepository ?? new DynamicRepository();
            _cachingService = cachingService ?? new CachingService();
        }

        public IEnumerable<SettingType> GetProductSettingTypes(byte productId, byte systemId, byte settingStatus)
        {
            Check.Argument.IsNotNegativeOrZero(productId, "productId");
            Check.Argument.IsNotNegative(systemId, "systemId");

            var settingTypes = _cachingService.GetOrAdd(
                   string.Format(SettingTypesCacheKey, productId, systemId),
                   () =>
                   {
                       return _productRepository.Fetch<SettingType>(
                           new ProductSettingTypesQueryCriteria
                           {
                               ProductId = productId,
                               SystemId = systemId,
                               SettingStatus = settingStatus
                           });
                   },
                   new TimeSpan(CacheDuration, 0, 0));

            return settingTypes;
        }

        public SettingType GetProductSettingType(byte productId, short settingTypeId, byte systemId, byte settingStatus)
        {
            var productSettingTypes = GetProductSettingTypes(productId, systemId, settingStatus);

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