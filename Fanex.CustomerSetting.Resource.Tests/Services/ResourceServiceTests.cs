namespace Fanex.CustomerSetting.Resource.Services
{
    using System;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resource.Criteria;
    using Fanex.CustomerSetting.Resource.Dtos;
    using Fanex.CustomerSetting.Resource.Services.Impl;
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
        private IRemotingRef _remotingRefApi;
        private string _language;

        [SetUp]
        public void Setup()
        {
            _settingContext = new SettingContextStub();
            _dymanicRepository = Substitute.For<IDynamicRepository>();
            _remotingRefApi = Substitute.For<IRemotingRef>();
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
        public void GetProductName_ProductNameListHasNoItem_ReturnDefaultProductName()
        {
            //// Arrange
            byte productId = 1;
            _dymanicRepository.Fetch<ProductNameDto>(Arg.Any<ProductNamesQueryCriteria>()).Returns(new List<ProductNameDto>());

            //// Act
            var actualProductName = _resourceService.GetProductName(productId, _language);

            //// Assert
            Assert.AreEqual("Product 1", actualProductName);
        }

        #endregion Get Product Name
    }
}