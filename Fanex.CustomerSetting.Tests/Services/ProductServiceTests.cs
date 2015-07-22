namespace Fanex.CustomerSetting.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
        private ICachingService _cachingService;

        [SetUp]
        public void Setup()
        {
            _dynamicRepository = Substitute.For<IDynamicRepository>();
            _cachingService = Substitute.For<ICachingService>();
            _productService = new ProductService(_dynamicRepository, _cachingService);
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
            _cachingService.GetOrAdd<IEnumerable<SettingType>>(
              Arg.Any<string>(),
              Arg.Any<Func<IEnumerable<SettingType>>>(),
              new TimeSpan(1, 0, 0))
              .Returns(settingTypes);

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
            var cachingServiceStub = new CachingService();
            cachingServiceStub.Remove("Fanex.CustomerSetting.SettingTypesCacheKey_1");

            var productService = new ProductService(_dynamicRepository, cachingServiceStub);

            //// Act
            productService.GetProductSettingTypes(productId);

            //// Assert
            _dynamicRepository.Received().Fetch<SettingType>(Arg.Is<ProductSettingTypesQueryCriteria>(
                criteria => criteria.ProductId == productId));
        }

        [Test]
        public void GetProductSettingTypes_NoDataInCache_ReturnSettingTypesFromDatabase()
        {
            //// Arrange
            byte productId = 1;
            IEnumerable<SettingType> settingTypesFromDatabase = new List<SettingType> { new SettingType() };
            var cachingServiceStub = new CachingService();
            cachingServiceStub.Remove("Fanex.CustomerSetting.SettingTypesCacheKey_1");

            var productService = new ProductService(_dynamicRepository, cachingServiceStub);

            _dynamicRepository.Fetch<SettingType>(Arg.Any<ICriteria>()).Returns(settingTypesFromDatabase);

            //// Act
            var actualSettingTypes = productService.GetProductSettingTypes(productId);

            //// Assert
            Assert.AreSame(settingTypesFromDatabase, actualSettingTypes);
        }

        /// <summary>
        /// Get single setting types
        /// </summary>
        [Test]
        public void GetProductSettingType_Always_ReturnSettingTypeInCache()
        {
            //// Arrange
            byte productId = 1;
            byte settingType = 1;
            IEnumerable<SettingType> settingTypes = new List<SettingType> { new SettingType { Id = 1, SettingLevel = 1 } };
            _cachingService.GetOrAdd<IEnumerable<SettingType>>(
                Arg.Any<string>(),
                Arg.Any<Func<IEnumerable<SettingType>>>(),
                new TimeSpan(1, 0, 0))
                .Returns(settingTypes);

            //// Act
            var actualSettingType = _productService.GetProductSettingType(productId, settingType);

            //// Assert
            Assert.AreSame(settingTypes.First(), actualSettingType);
        }

        [Test]
        public void GetProductSettingType_NoData_ReturnNull()
        {
            //// Arrange
            byte productId = 1;
            byte settingType = 1;
            IEnumerable<SettingType> settingTypesInCache = null;
            IEnumerable<SettingType> settingTypesFromDatabase = new List<SettingType>();

            _cachingService.GetOrAdd<IEnumerable<SettingType>>(Arg.Any<string>(), Arg.Any<Func<IEnumerable<SettingType>>>(), new TimeSpan(1, 0, 0))
               .Returns(settingTypesInCache);

            _dynamicRepository.Fetch<SettingType>(Arg.Any<ICriteria>()).Returns(settingTypesFromDatabase);

            //// Act
            var actualSettingType = _productService.GetProductSettingType(productId, settingType);

            //// Assert
            Assert.IsNull(actualSettingType);
        }
    }
}