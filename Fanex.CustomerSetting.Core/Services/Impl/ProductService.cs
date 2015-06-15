namespace Fanex.CustomerSetting.Core.Services.Impl
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Core.Criteria.Product;
    using Fanex.CustomerSetting.Core.Dtos;
    using Fanex.CustomerSetting.Core.Models;
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

        public Product GetProduct(byte productId)
        {
            IEnumerable<SettingType> settingTypes = GetProductSettingTypes(productId);

            //// Implement caching here
            string productName = GetProductName(productId);

            return new Product
                {
                    Id = productId,
                    Name = productName,
                    SettingTypes = settingTypes
                };
        }

        private string GetProductName(byte productId)
        {
            ProductNameDto productName = _productRepository.Get<ProductNameDto>(new ProductResourceQueryCriteria
            {
                ProductId = productId
            });

            return productName.ProductName;
        }

        public IEnumerable<SettingType> GetProductSettingTypes(byte productId)
        {
            IEnumerable<SettingType> settingTypes = _productRepository.Fetch<SettingType>(new
                ProductSettingTypesQueryCriteria
            {
                ProductId = productId
            });

            return settingTypes;
        }
    }
}