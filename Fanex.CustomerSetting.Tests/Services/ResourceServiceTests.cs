namespace Fanex.CustomerSetting.Tests.Services
{
    using System;
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
        private IRemotingRefService _remotingRefApi;
        private ICustomerSettingCache _cacheManager;
        private string _language;

        [SetUp]
        public void Setup()
        {
            _dymanicRepository = Substitute.For<IDynamicRepository>();
            _remotingRefApi = Substitute.For<IRemotingRefService>();
            _cacheManager = Substitute.For<ICustomerSettingCache>();
            _language = "en-US";
            _resourceService = new ResourceService(_dymanicRepository, _remotingRefApi, _cacheManager);
        }

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
        public void GetProductName_Always_GetProductNameFromCache()
        {
            //// Arrange
            byte productId = 1;
            var productName = new ProductName { ProductId = productId, ProductNameEN = "product name EN" };
            _cacheManager.Get<ProductName>(Arg.Any<string>()).Returns(productName);

            //// Act
            var actualResult = _resourceService.GetProductName(productId, _language);

            //// Assert
            Assert.AreEqual(productName.ProductNameEN, actualResult);
        }

        [Test]
        public void GetProductName_NoDataInCache_ReturnsDefaultProductName()
        {
            //// Arrange
            byte productId = 1;
            ProductName nullProductName = null;
            _cacheManager.Get<ProductName>(Arg.Any<string>()).Returns(nullProductName);

            //// Act
            var actualResult = _resourceService.GetProductName(productId, _language);

            //// Assert
            Assert.AreEqual(string.Format("Product {0}", productId), actualResult);
        }

        [Test]
        public void GetProductName_NoDataInCache_ReturnDataGetFromRepository()
        {
            //// Arrange
            byte productId = 1;
            ProductName productName = new ProductName { ProductNameEN = "A product name" };
            ProductName nullProductName = null;
            _cacheManager.Get<ProductName>(Arg.Any<string>()).Returns(nullProductName);
            _dymanicRepository.Get<ProductName>(Arg.Any<ICriteria>()).Returns(productName);

            //// Act
            var actualResult = _resourceService.GetProductName(productId, _language);

            //// Assert
            Assert.AreEqual(productName.ProductNameEN, actualResult);
        }

        [Test]
        public void GetProductName_Always_ReturnProductNameInRightLanguage()
        {
            //// Arrange
            byte productId = 1;
            ProductName productName = new ProductName { ProductNameEN = "Prod EN", ProductNameTH = "Prod TH" };
            _cacheManager.Get<ProductName>(Arg.Any<string>()).Returns(productName);

            //// Act
            var actualResult = _resourceService.GetProductName(productId, _language);

            //// Assert
            Assert.AreEqual(productName.ProductNameEN, actualResult);
        }

        [Test]
        public void GetProductName_UnSupportedLanguage_ReturnProductNameInEnglish()
        {
            //// Arrange
            byte productId = 1;
            ProductName productName = new ProductName { ProductNameEN = "Prod EN", ProductNameTH = "Prod TH" };
            _cacheManager.Get<ProductName>(Arg.Any<string>()).Returns(productName);

            //// Act
            var actualResult = _resourceService.GetProductName(productId, Arg.Any<string>());

            //// Assert
            Assert.AreEqual(productName.ProductNameEN, actualResult);
        }

        #endregion Get Product Name

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
        public void GetSportTypeName_Always_ExecuteGetSportTypeNameOnRemotingRefApi()
        {
            //// Arrange
            int sportTypeId = 1;

            //// Act
            _resourceService.GetSportTypeName(sportTypeId, _language);

            //// Assert
            _remotingRefApi
                .Received()
                .GetSportTypeName(
                Arg.Is<string>(sportTypeIdString => sportTypeIdString == sportTypeId.ToString()),
                Arg.Any<string>());
        }

        [Test]
        public void GetSportTypeName_Always_ReturnDataFromRemotingRefApi()
        {
            //// Arrange
            int sportTypeId = 1;
            string expectedSportName = "SportName";
            _remotingRefApi.GetSportTypeName(Arg.Any<string>(), Arg.Any<string>()).Returns(expectedSportName);

            //// Act
            var actualSportName = _resourceService.GetSportTypeName(sportTypeId, _language);

            //// Assert
            Assert.AreEqual(expectedSportName, actualSportName);
        }

        [TestCase(null)]
        [TestCase("")]
        public void GetSportTypeName_SportTypeNameIsNullOrEmpty_ReturnDefaultSportTypeName(string sportNameFromApi)
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
        public void GetBetTypeName_Always_ExecuteGetBetTypeNameOnRemotingRefApi()
        {
            //// Arrange
            int betTypeId = 1;

            //// Act
            _resourceService.GetBetTypeName(betTypeId, _language);

            //// Assert
            _remotingRefApi
                .Received()
                .GetBetTypeName(
                Arg.Is<string>(betTypeIdString => betTypeIdString == betTypeId.ToString()),
                Arg.Any<string>());
        }

        [Test]
        public void GetBetTypeName_Always_ReturnDataFromRemotingRefApi()
        {
            //// Arrange
            int betTypeId = 1;
            string expectedBetTypeName = "SportName";
            _remotingRefApi.GetBetTypeName(Arg.Any<string>(), Arg.Any<string>()).Returns(expectedBetTypeName);

            //// Act
            var actualBetTypeName = _resourceService.GetBetTypeName(betTypeId, _language);

            //// Assert
            Assert.AreEqual(expectedBetTypeName, actualBetTypeName);
        }

        [TestCase(null)]
        [TestCase("")]
        public void GetBetTypeName_BetTypeNameIsEmptyOrNull_ReturnDefaultSportTypeName(string betTypeNameFromApi)
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
    }
}