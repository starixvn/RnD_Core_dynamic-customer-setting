namespace Fanex.CustomerSetting.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using Fanex.Caching;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.NAP.Framework.Data;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class ProductServiceTests
    {
        private IProductService _productService;
        private IDynamicRepository _dynamicRepository;
        private ICache _cacheManager;

        [SetUp]
        public void Setup()
        {
            _dynamicRepository = Substitute.For<IDynamicRepository>();
            _cacheManager = Substitute.For<ICache>();
            _productService = new ProductService(_dynamicRepository, _cacheManager);
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

        [Test]
        public void GetProductSettingTypes_Always_ReturnSettingTypesInCache()
        {
            //// Arrange
            byte productId = 1;
            IEnumerable<SettingType> settingTypes = new List<SettingType> { new SettingType() };
            _cacheManager.Get<IEnumerable<SettingType>>("Fanex.CustomerSetting.SettingTypesCacheKey_1").Returns(settingTypes);

            //// Act
            var actualSettingTypes = _productService.GetProductSettingTypes(productId);

            //// Assert
            Assert.AreSame(settingTypes, actualSettingTypes);
        }

        [Test]
        public void GetProductSettingTypes_NoDataInCache_AlwaysExecuteFetchSettingTypes()
        {
            //// Arrange
            byte productId = 1;
            IEnumerable<SettingType> settingTypes = null;
            _cacheManager.Get<IEnumerable<SettingType>>(Arg.Any<string>()).Returns(settingTypes);

            //// Act
            _productService.GetProductSettingTypes(productId);

            //// Assert
            _dynamicRepository.Received().Fetch<SettingType>(Arg.Is<ProductSettingTypesQueryCriteria>(
                criteria => criteria.ProductId == productId));
        }

        [Test]
        public void GetProductSettingTypes_NoDataInCache_ReturnSettingTypesFromDatabase()
        {
            //// Arrange
            byte productId = 1;
            IEnumerable<SettingType> settingTypesInCache = null;
            IEnumerable<SettingType> settingTypesFromDatabase = new List<SettingType> { new SettingType() };
            _cacheManager.Get<IEnumerable<SettingType>>(Arg.Any<string>()).Returns(settingTypesInCache);
            _dynamicRepository.Fetch<SettingType>(Arg.Any<ICriteria>()).Returns(settingTypesFromDatabase);

            //// Act
            var actualSettingTypes = _productService.GetProductSettingTypes(productId);

            //// Assert
            Assert.AreSame(settingTypesFromDatabase, actualSettingTypes);
        }

        [Test]
        public void GetProductSettingTypes_GotSettingTypeFromDatabase_AlwaysSetSettingTypesToCache()
        {
            //// Arrange
            byte productId = 1;
            IEnumerable<SettingType> settingTypesInCache = null;
            IEnumerable<SettingType> settingTypesFromDatabase = new List<SettingType> { new SettingType() };
            _cacheManager.Get<IEnumerable<SettingType>>(Arg.Any<string>()).Returns(settingTypesInCache);
            _dynamicRepository.Fetch<SettingType>(Arg.Any<ICriteria>()).Returns(settingTypesFromDatabase);

            //// Act
            _productService.GetProductSettingTypes(productId);

            //// Assert
            _cacheManager.Received().Set<IEnumerable<SettingType>>("Fanex.CustomerSetting.SettingTypesCacheKey_1", settingTypesFromDatabase);
        }
    }
}