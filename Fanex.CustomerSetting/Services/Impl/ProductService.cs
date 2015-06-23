namespace Fanex.CustomerSetting.Services.Impl
{
    using System.Collections.Generic;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Models;
    using Fanex.NAP.Framework.Data;

    public class ProductService : IProductService
    {
        private readonly IDynamicRepository _productRepository;

        public ProductService()
            : this(null)
        {
        }

        public ProductService(IDynamicRepository productRepository)
        {
            _productRepository = productRepository ?? new DynamicRepository();
        }

        public IEnumerable<SettingType> GetProductSettingTypes(byte productId)
        {
            Check.Argument.IsNotNegativeOrZero(productId, "productId");

            IEnumerable<SettingType> settingTypes = _productRepository.Fetch<SettingType>(new
                ProductSettingTypesQueryCriteria
            {
                ProductId = productId
            });

            return settingTypes;
        }
    }
}