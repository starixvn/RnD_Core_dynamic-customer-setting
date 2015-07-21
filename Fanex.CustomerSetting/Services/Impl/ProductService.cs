namespace Fanex.CustomerSetting.Services.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Models;
    using Fanex.NAP.Framework.Data;

    public class ProductService : IProductService
    {
        private const int SettingTypeCacheTimeout = 60 * 60; //// 1 hour
        private const string SettingTypesCacheKey = "Fanex.CustomerSetting.SettingTypesCacheKey_{0}";

        private readonly IDynamicRepository _productRepository;
        private readonly ICustomerSettingCache _cacheManager;

        public ProductService(IDynamicRepository productRepository = null, ICustomerSettingCache cacheManager = null)
        {
            _productRepository = productRepository ?? new DynamicRepository();
            _cacheManager = cacheManager ?? new CustomerSettingCache();
        }

        public IEnumerable<SettingType> GetProductSettingTypes(byte productId)
        {
            Check.Argument.IsNotNegativeOrZero(productId, "productId");

            var settingTypes = GetSettingTypesFromCache(productId);

            if (settingTypes == null)
            {
                settingTypes = GetSettingTypesFromDatabase(productId);
                SetSettingTypesToCache(productId, settingTypes);
            }

            return settingTypes;
        }

        private IEnumerable<SettingType> GetSettingTypesFromCache(byte productId)
        {
            var cacheKey = string.Format(SettingTypesCacheKey, productId);

            return _cacheManager.Get<IEnumerable<SettingType>>(cacheKey);
        }

        private IEnumerable<SettingType> GetSettingTypesFromDatabase(byte productId)
        {
            return _productRepository.Fetch<SettingType>(new
                ProductSettingTypesQueryCriteria
            {
                ProductId = productId
            });
        }

        private void SetSettingTypesToCache(byte productId, IEnumerable<SettingType> settingTypes)
        {
            if (settingTypes != null && settingTypes.Any())
            {
                var cacheKey = string.Format(SettingTypesCacheKey, productId);

                _cacheManager.Set<IEnumerable<SettingType>>(cacheKey, settingTypes);
            }
        }

        public SettingType GetProductSettingType(byte productId, byte settingTypeId)
        {
            return GetProductSettingTypes(productId).FirstOrDefault(settingType => settingType.Id == settingTypeId);
        }
    }
}