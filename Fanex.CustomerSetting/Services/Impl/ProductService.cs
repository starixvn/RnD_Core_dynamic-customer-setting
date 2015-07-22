namespace Fanex.CustomerSetting.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Models;
    using Fanex.NAP.Framework.Data;

    public class ProductService : IProductService
    {
        private const int SettingTypeCacheDuration = 1; //// 1 hour
        private const string SettingTypesCacheKey = "Fanex.CustomerSetting.SettingTypesCacheKey_{0}";

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
                   new TimeSpan(SettingTypeCacheDuration, 0, 0));

            return settingTypes;
        }

        public SettingType GetProductSettingType(byte productId, byte settingTypeId)
        {
            var productSettingTypes = GetProductSettingTypes(productId);

            if (productSettingTypes == null)
            {
                return null;
            }

            return productSettingTypes.FirstOrDefault(settingType => settingType.Id == settingTypeId);
        }
    }
}