namespace Fanex.CustomerSetting.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using Fanex.Caching;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Dtos;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.RemotingRefApi;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.Tests.Models;
    using Fanex.NAP.Framework.Data;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class ResourceServiceTests
    {
        private IResourceService _resourceService;
        private IDynamicRepository _dymanicRepository;
        private ISettingContext _settingContext;
        private IRemotingRefService _remotingRefApi;
        private string _language;

        [SetUp]
        public void Setup()
        {
            _settingContext = new SettingContextStub();
            _dymanicRepository = Substitute.For<IDynamicRepository>();
            _remotingRefApi = Substitute.For<IRemotingRefService>();
            _language = "en-US";
            _resourceService = new ResourceService(_dymanicRepository, _remotingRefApi);
        }

        #region Get Sport Name

        [TestCase(0)]
        [TestCase(-1)]
        public void GetSportTypeName_SportTypeIdIsNegativeOrZero_ThrowArgumentOutOfRangeException(int sportTypeId)
        {
            //// Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _resourceService.GetSportTypeName(sportTypeId, _language)));
        }

        [Test]
        public void GetSportTypeName_ExecuteGetSportTypeNameOnRemotingRefApi()
        {
            //// Arrange
            int sportTypeId = 1;

            //// Act
            _resourceService.GetSportTypeName(sportTypeId, _language);

            //// Assert
            _remotingRefApi.Received().GetSportTypeName(
                Arg.Is<string>(
                    sportTypeIdString => sportTypeIdString == sportTypeId.ToString()),
                Arg.Any<string>());
        }

        [TestCase(null)]
        [TestCase("")]
        public void GetSportTypeName_SportTypeNameReturnFromRemotingRefApiIsEmptyOrNull_ReturnDefaultSportTypeName(string sportNameFromApi)
        {
            //// Arrange
            int sportTypeId = 1;
            _remotingRefApi.GetSportTypeName(sportTypeId.ToString(), Arg.Any<string>()).Returns(sportNameFromApi);

            //// Act
            var actualSportTypeName = _resourceService.GetSportTypeName(sportTypeId, _language);

            //// Assert
            Assert.AreEqual("Sport Type 1", actualSportTypeName);
        }

        #endregion Get Sport Name

        #region Get Bet Type Name

        [TestCase(0)]
        [TestCase(-1)]
        public void GetBetTypeName_BetTypeIdIsNegativeOrZero_ThrowArgumentOutOfRangeException(int betTypeId)
        {
            //// Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _resourceService.GetBetTypeName(betTypeId, _language)));
        }

        [Test]
        public void GetBetTypeName_ExecuteGetSportTypeNameOnRemotingRefApi()
        {
            //// Arrange
            int betTypeId = 1;

            //// Act
            _resourceService.GetBetTypeName(betTypeId, _language);

            //// Assert
            _remotingRefApi.Received().GetBetTypeName(
                Arg.Is<string>(
                    betTypeIdString => betTypeIdString == betTypeId.ToString()),
                Arg.Any<string>());
        }

        [TestCase(null)]
        [TestCase("")]
        public void GetBetTypeName_BetTypeNameReturnFromRemotingRefApiIsEmptyOrNull_ReturnDefaultSportTypeName(string betTypeNameFromApi)
        {
            //// Arrange
            int betTypeId = 1;
            _remotingRefApi.GetBetTypeName(betTypeId.ToString(), Arg.Any<string>()).Returns(betTypeNameFromApi);

            //// Act
            var actualBetTypeName = _resourceService.GetBetTypeName(betTypeId, _language);

            //// Assert
            Assert.AreEqual("Bet Type 1", actualBetTypeName);
        }

        #endregion Get Bet Type Name

        #region Get Product Name

        [Test]
        public void GetProductName_ProductIdIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            byte productId = 0;

            //// Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _resourceService.GetProductName(productId, _language)));
        }

        [Test]
        public void GetProductName_ExecuteFetchMethodOnDynamicRepository()
        {
            //// Arrange
            byte productId = 1;

            //// Act
            _resourceService.GetProductName(productId, _language);

            //// Assert
            _dymanicRepository.Received().Fetch<ProductNameDto>(Arg.Any<ProductNamesQueryCriteria>());
        }

        [Test]
        public void GetProductName_ProductNameListDoesNotContainProductName_ReturnDefaultProductName()
        {
            //// Arrange
            byte productId = 1;
            _dymanicRepository.Fetch<ProductNameDto>(Arg.Any<ProductNamesQueryCriteria>()).Returns(new List<ProductNameDto> 
            {
                new ProductNameDto { ProductId = 3, ProductName = "Name 1", ProductNameEN = "Name en 1" },
                new ProductNameDto { ProductId = 2, ProductName = "Sample Product Name", ProductNameEN = "Sample Product Name" }
            });
            CacheService.CreateInstance(CacheType.MemoryCache).Remove("Fanex.CustomerSetting.ProductResourceCacheKey");

            //// Act
            var actualProductName = _resourceService.GetProductName(productId, _language);

            //// Assert
            Assert.AreEqual("Product 1", actualProductName);
        }

        [Test]
        public void GetProductName_ProductNameListContainsProductName_ReturnProductName()
        {
            //// Arrange
            byte productId = 2;
            string sampleProductName = "Sample Product Name";
            _dymanicRepository.Fetch<ProductNameDto>(Arg.Any<ProductNamesQueryCriteria>()).Returns(new List<ProductNameDto> 
            {
                new ProductNameDto { ProductId = 1, ProductName = "Name 1", ProductNameEN = "Name en 1" },
                new ProductNameDto { ProductId = 2, ProductName = sampleProductName, ProductNameEN = sampleProductName }
            });
            CacheService.CreateInstance(CacheType.MemoryCache).Remove("Fanex.CustomerSetting.ProductResourceCacheKey");

            //// Act
            var actualProductName = _resourceService.GetProductName(productId, _language);

            //// Assert
            Assert.AreEqual(sampleProductName, actualProductName);
        }

        [Test]
        public void GetProductName_ProductNameListContainsProductName_ReturnProductNameWithLanguage()
        {
            //// Arrange
            byte productId = 2;
            _language = "zh-TW";
            _dymanicRepository.Fetch<ProductNameDto>(Arg.Any<ProductNamesQueryCriteria>()).Returns(new List<ProductNameDto> 
            {
                new ProductNameDto { ProductId = 1, ProductName = "Name 1", ProductNameEN = "Name en 1" },
                new ProductNameDto { ProductId = 2, ProductName = "Sample Product Name", ProductNameCH = "Sample Product Name - CH",  }
            });
            CacheService.CreateInstance(CacheType.MemoryCache).Remove("Fanex.CustomerSetting.ProductResourceCacheKey");

            //// Act
            var actualProductName = _resourceService.GetProductName(productId, _language);

            //// Assert
            Assert.AreEqual("Sample Product Name - CH", actualProductName);
        }
        #endregion Get Product Name
    }
}