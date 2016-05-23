namespace Fanex.CustomerSetting.Tests.Services
{
    using System;
    using CustomerSetting.Models;
    using CustomerSetting.Services;
    using CustomerSetting.Services.Impl;
    using Data.Repository;
    using NSubstitute;
    using NUnit.Framework;
    using RemotingRefApi;

    [TestFixture]
    public class ResourceServiceTests
    {
        private IResourceService _resourceService;
        private IDynamicRepository _dymanicRepository;
        private IRemotingRefService _remotingRefApi;
        private ICachingService _cachingService;
        private string _language;

        [SetUp]
        public void Setup()
        {
            _dymanicRepository = Substitute.For<IDynamicRepository>();
            _remotingRefApi = Substitute.For<IRemotingRefService>();
            _cachingService = Substitute.For<ICachingService>();
            _language = "en-US";
            _resourceService = new ResourceService(_dymanicRepository, _remotingRefApi, _cachingService);
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
            var productName = new ProductName { ProductId = productId, Name = "product name EN" };
            _cachingService.GetOrAdd<ProductName>(
                Arg.Any<string>(),
                Arg.Any<Func<ProductName>>(),
                new TimeSpan(1, 0, 0)).Returns(productName);

            //// Act
            var actualResult = _resourceService.GetProductName(productId, _language);

            //// Assert
            Assert.AreEqual(productName.Name, actualResult);
        }

        [Test]
        public void GetProductName_NoDataInCache_ReturnsDefaultProductName()
        {
            //// Arrange
            byte productId = 1;
            ProductName nullProductName = null;
            _cachingService.GetOrAdd<ProductName>(
               Arg.Any<string>(),
               Arg.Any<Func<ProductName>>(),
               new TimeSpan(1, 0, 0)).Returns(nullProductName);

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
            ProductName productName = new ProductName { Name = "A product name" };

            _cachingService.GetOrAdd<ProductName>(
               Arg.Any<string>(),
               Arg.Any<Func<ProductName>>(),
               new TimeSpan(1, 0, 0)).Returns(productName);

            _dymanicRepository.Get<ProductName>(Arg.Any<ICriteria>()).Returns(productName);

            //// Act
            var actualResult = _resourceService.GetProductName(productId, _language);

            //// Assert
            Assert.AreEqual(productName.Name, actualResult);
        }

        #endregion Get Product Name

        #region Get Sport Name

        [TestCase(0)]
        [TestCase(-1)]
        public void GetSportTypeName_SportTypeIdIsNegativeOrZero_ReturnEmptyName(int sportTypeId)
        {
            //// Act
            var actualSportTypeName = _resourceService.GetSportTypeName(sportTypeId, _language);

            //// Assert
            Assert.IsEmpty(actualSportTypeName);
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
        public void GetBetTypeName_BetTypeIdIsNegativeOrZero_ReturnEmptyName(int betTypeId)
        {
            //// Act
            var actualBetTypeName = _resourceService.GetBetTypeName(betTypeId, _language);

            //// Assert
            Assert.IsEmpty(actualBetTypeName);
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

        #region Get Data Resource

        [TestCase(null)]
        [TestCase("")]
        public void GetDataResource_RefIdIsNullOrEmpty_ReturnEmpty(string refId)
        {
            // Arrange.
            string resourceId = nameof(resourceId);

            // Act.
            string actualResourceData = _resourceService.GetResourceData(refId, resourceId, _language);

            // Assert.
            Assert.IsEmpty(actualResourceData);
        }

        [TestCase(null)]
        [TestCase("")]
        public void GetDataResource_ResourceIdIsNullOrEmpty_ReturnEmpty(string resourceId)
        {
            // Arrange.
            string refId = nameof(refId);

            // Act.
            string actualResourceData = _resourceService.GetResourceData(refId, resourceId, _language);

            // Assert.
            Assert.IsEmpty(actualResourceData);
        }

        public void GetDataResource_RefIdAndResourceIdAreNotEmpty_CallRemotingRefApi()
        {
            // Arrange.
            string refId = nameof(refId);
            string resourceId = nameof(resourceId);

            // Act.
            _resourceService.GetResourceData(refId, resourceId, _language);

            // Assert.
            _remotingRefApi
                .Received()
                .GetResourceData(
                    Arg.Is<string>(passedRefId => passedRefId == refId),
                    Arg.Is<string>(passedResourceId => passedResourceId == resourceId),
                    Arg.Any<string>());
        }

        public void GetDataResource_RefIdAndResourceIdAreNotEmpty_ReturnDataFromRemotingRefApi()
        {
            // Arrange.
            string refId = nameof(refId);
            string resourceId = nameof(resourceId);
            string resourceData = nameof(resourceData);

            _remotingRefApi.GetResourceData(refId, resourceId, Arg.Any<string>()).Returns(resourceData);

            // Act.
            string actualData = _resourceService.GetResourceData(refId, resourceId, _language);

            // Assert.
            Assert.AreEqual(resourceData, actualData);
        }

        public void GetDataResource_RemotingRefApiReturnsEmpty_ReturnDefaultResourceData()
        {
            // Arrange.
            string refId = nameof(refId);
            string resourceId = nameof(resourceId);

            _remotingRefApi.GetResourceData(refId, resourceId, Arg.Any<string>()).Returns(string.Empty);

            // Act.
            string actualData = _resourceService.GetResourceData(refId, resourceId, _language);

            // Assert.
            Assert.AreEqual($"Ref Resource: {refId} - Key: {resourceId}", actualData);
        }

        #endregion Get Data Resource
    }
}