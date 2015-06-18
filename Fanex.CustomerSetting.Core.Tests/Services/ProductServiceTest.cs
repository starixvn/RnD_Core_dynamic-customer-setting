namespace Fanex.CustomerSetting.Core.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.CustomerSetting.Core.Models;
    using Fanex.CustomerSetting.Core.Services;
    using Fanex.CustomerSetting.Core.Services.Impl;
    using Fanex.CustomerSetting.Core.SPObjects.Criteria.Product;
    using Fanex.NAP.Framework.Data;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class ProductServiceTest
    {
        private IProductService _productService;
        private IDynamicRepository _dynamicRepository;

        [SetUp]
        public void Setup()
        {
            _dynamicRepository = Substitute.For<IDynamicRepository>();
            _productService = new ProductService(_dynamicRepository);
        }

        [Test]
        public void GetProductSettingTypes_ProductIdIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Arrange 
            byte productId = 0;

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _productService.GetProductSettingTypes(productId)));
        }

        [TestCase(1)]
        [TestCase(100)]
        public void GetProductSettingTypes_ProductIdGreaterThanZero_ExecuteFetchMethodOnDynamicRepository(byte productId)
        {
            //// Act
            _productService.GetProductSettingTypes(productId);

            //// Assert
            _dynamicRepository.Received().Fetch<SettingType>(Arg.Is<ProductSettingTypesQueryCriteria>(
                criteria => criteria.ProductId == productId
                ));
        }
    }
}
