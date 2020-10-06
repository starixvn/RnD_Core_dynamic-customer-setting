namespace Fanex.CustomerSetting.Tests.Services
{
    using CustomerSetting.Services;
    using CustomerSetting.Services.Impl;
    using NSubstitute;
    using NUnit.Framework;
    using RemotingRef;

    [TestFixture]
    public class ResourceServiceTests
    {
        private IResourceService _resourceService;
        private IRemotingRefService _remotingRefApi;
        private string _language;

        [SetUp]
        public void Setup()
        {
            _remotingRefApi = Substitute.For<IRemotingRefService>();
            _language = "en-US";
            _resourceService = new ResourceService(_remotingRefApi);
        }

        #region Get Product Name

        [Test]
        public void GetProductName_ProductIdIsZero_ReturnEmptyName()
        {
            //// Act
            byte productId = 0;
            var actualProductName = _resourceService.GetProductName(productId, _language);

            //// Assert
            Assert.IsEmpty(actualProductName);
        }

        [Test]
        public void GetProductName_Always_ExecuteGetResourceDataOfRemotingRefApi()
        {
            //// Arrange
            byte productId = 1;

            //// Act
            _resourceService.GetProductName(productId, _language);

            //// Assert
            _remotingRefApi
                .Received()
                .GetResourceDataByRefIdAndResourceId(
                    Arg.Is<string>(refId => refId == "Product"),
                    Arg.Is<string>(resourceId => resourceId == productId.ToString()),
                    Arg.Any<string>(),
                    Arg.Any<string>());
        }

        [Test]
        public void GetProductName_Always_ReturnDataFromRemotingRefApi()
        {
            //// Arrange
            byte productId = 1;
            string expectedProductName = "Product Name";

            _remotingRefApi
                .GetResourceDataByRefIdAndResourceId(
                    Arg.Is<string>(refId => refId == "Product"),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>())
                .Returns(expectedProductName);

            //// Act
            var actualProductName = _resourceService.GetProductName(productId, _language);

            //// Assert
            Assert.AreEqual(expectedProductName, actualProductName);
        }

        [TestCase(null)]
        [TestCase("")]
        public void GetProductName_ProductNameIsNullOrEmpty_ReturnDefaultProductName(string productNameFromApi)
        {
            //// Arrange
            byte productId = 1;

            _remotingRefApi
                .GetResourceDataByRefIdAndResourceId(
                    Arg.Is<string>(refId => refId == "Product"),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>())
                .Returns(productNameFromApi);

            //// Act
            var actualProductName = _resourceService.GetProductName(productId, _language);

            //// Assert
            Assert.AreEqual("Product 1", actualProductName);
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
                .GetSportName(
                    Arg.Is<string>(sportTypeIdString => sportTypeIdString == sportTypeId.ToString()),
                    Arg.Any<string>());
        }

        [Test]
        public void GetSportTypeName_Always_ReturnDataFromRemotingRefApi()
        {
            //// Arrange
            int sportTypeId = 1;
            string expectedSportName = "SportName";
            _remotingRefApi.GetSportName(Arg.Any<string>(), Arg.Any<string>()).Returns(expectedSportName);

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
            _remotingRefApi.GetSportName(sportTypeId.ToString(), Arg.Any<string>()).Returns(sportNameFromApi);

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

        [Test]
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
                .GetResourceDataByRefIdAndResourceId(
                    Arg.Is<string>(passedRefId => passedRefId == refId),
                    Arg.Is<string>(passedResourceId => passedResourceId == resourceId),
                    Arg.Any<string>(),
                    Arg.Any<string>());
        }

        [Test]
        public void GetDataResource_RefIdAndResourceIdAreNotEmpty_ReturnDataFromRemotingRefApi()
        {
            // Arrange.
            string refId = nameof(refId);
            string resourceId = nameof(resourceId);
            string resourceData = nameof(resourceData);

            _remotingRefApi
                .GetResourceDataByRefIdAndResourceId(
                    refId,
                    resourceId,
                    Arg.Any<string>(),
                    Arg.Any<string>())
                .Returns(resourceData);

            // Act.
            string actualData = _resourceService.GetResourceData(refId, resourceId, _language);

            // Assert.
            Assert.AreEqual(resourceData, actualData);
        }

        [Test]
        public void GetDataResource_RemotingRefApiReturnsEmpty_ReturnDefaultResourceData()
        {
            // Arrange.
            string refId = nameof(refId);
            string resourceId = nameof(resourceId);

            _remotingRefApi
                .GetResourceDataByRefIdAndResourceId(refId, resourceId, Arg.Any<string>(), Arg.Any<string>())
                .Returns(string.Empty);

            // Act.
            string actualData = _resourceService.GetResourceData(refId, resourceId, _language);

            // Assert.
            Assert.AreEqual($"Ref Resource: {refId} - Key: {resourceId}", actualData);
        }

        #endregion Get Data Resource
    }
}