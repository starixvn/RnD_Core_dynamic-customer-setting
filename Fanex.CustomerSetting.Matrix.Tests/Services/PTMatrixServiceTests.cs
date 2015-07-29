namespace Fanex.CustomerSetting.Matrix.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Matrix.Criteria;
    using Fanex.CustomerSetting.Matrix.Dtos;
    using Fanex.CustomerSetting.Matrix.Services;
    using Fanex.CustomerSetting.Matrix.Services.Impl;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Tests;
    using Fanex.NAP.Framework.Data;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class PTMatrixServiceTests
    {
        private IResourceService _resourceService;
        private IProductService _productService;
        private IDynamicRepository _dynamicRepository;

        private IPTMatrixService _positionTakingMatrixService;

        [SetUp]
        public void SetUp()
        {
            _resourceService = Substitute.For<IResourceService>();
            _productService = Substitute.For<IProductService>();
            _dynamicRepository = Substitute.For<IDynamicRepository>();

            _positionTakingMatrixService = new PTMatrixService(
                _resourceService,
                _productService,
                _dynamicRepository);
        }

        [Test]
        public void GetPTMatrix_ProductIdIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            byte productId = 0;

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _positionTakingMatrixService.GetPTMatrix(productId, "CustomerName")));
        }

        [TestCase("  ")]
        [TestCase("")]
        [TestCase(null)]
        public void GetPTMatrix_CustomerNameIsNullOrEmpty_ThrowArgumentOutOfRangeException(string customerName)
        {
            //// Arrange
            byte productId = 1;

            //// Act
            Assert.Throws<ArgumentException>(
                new TestDelegate(
                    () => _positionTakingMatrixService.GetPTMatrix(productId, customerName)));
        }

        private SettingType AssumeSettingTypeReturnFromDatabaseIsValid(SettingType dummySettingType = null)
        {
            dummySettingType = dummySettingType ?? InstanceFactory.CreateProductSettingType(1);

            _productService.GetProductSettingType(Arg.Any<byte>(), Arg.Any<short>()).Returns(dummySettingType);

            return dummySettingType;
        }

        [Test]
        public void GetPTMatrix_Always_ReturnSettingLevel()
        {
            //// Arrange
            byte productId = 1;
            string customerName = "CustomerName";
            var dummySettingType = AssumeSettingTypeReturnFromDatabaseIsValid();

            //// Act
            var actualPTMatrix = _positionTakingMatrixService.GetPTMatrix(productId, customerName);

            //// Assert
            Assert.AreEqual(dummySettingType.SettingLevel, actualPTMatrix.SettingLevel);
        }

        [Test]
        public void GetPTMatrix_ProductDoesNotHavePTSettingType_ThrowInvalidOperationException()
        {
            //// Arrange
            byte productId = 1;
            string customerName = "CustomerName";

            //// Act
            Assert.Throws<InvalidOperationException>(
                new TestDelegate(
                    () => _positionTakingMatrixService.GetPTMatrix(productId, customerName)));
        }

        [Test]
        public void GetPTMatrix_Always_CallGetIsAllowMasterAutoPT()
        {
            //// Arrange
            byte productId = 1;
            string customerName = "CustomerName";
            AssumeSettingTypeReturnFromDatabaseIsValid();

            //// Act
            _positionTakingMatrixService.GetPTMatrix(productId, customerName);

            //// Assert
            _dynamicRepository.Received().Get<bool>(Arg.Is<IsAllowMasterAutoPTQueryCriteria>(
                    criteria => criteria.UserName == customerName));
        }

        private void AssumeMasterIsAllowedAutoPT()
        {
            _dynamicRepository.Get<bool>(Arg.Any<IsAllowMasterAutoPTQueryCriteria>()).Returns(true);
        }

        [Test]
        public void GetPTMatrix_Always_ReturnIsAllowMasterAutoPT()
        {
            //// Arrange
            byte productId = 1;
            string customerName = "CustomerName";
            AssumeSettingTypeReturnFromDatabaseIsValid();
            AssumeMasterIsAllowedAutoPT();

            //// Act
            var actualPTMatrix = _positionTakingMatrixService.GetPTMatrix(productId, customerName);

            //// Assert
            Assert.IsTrue(actualPTMatrix.IsAllowMasterAutoPT);
        }

        [Test]
        public void GetPTMatrix_Always_CallFetchPTItemMatrixDtoList()
        {
            //// Arrange
            byte productId = 1;
            string customerName = "CustomerName";
            AssumeSettingTypeReturnFromDatabaseIsValid();

            //// Act
            _positionTakingMatrixService.GetPTMatrix(productId, customerName);

            //// Assert
            _dynamicRepository.Received().Fetch<PTItemMatrixDto>(
                Arg.Is<PTMatrixQueryCriteria>(
                    criteria => criteria.ProductId == productId
                    && criteria.UserName == customerName));
        }

        [Test]
        public void GetPTMatrix_Always_ReturnGroupedPTItemMatrices()
        {
            //// Arrange
            byte productId = 1;
            string customerName = "CustomerName";
            AssumeSettingTypeReturnFromDatabaseIsValid();

            var getPTItemMatrixDtoDummyList = GetPTItemMatrixDtoDummyList();
            _dynamicRepository.Fetch<PTItemMatrixDto>(Arg.Any<PTMatrixQueryCriteria>()).Returns(getPTItemMatrixDtoDummyList);

            //// Act
            var actualPTMatrix = _positionTakingMatrixService.GetPTMatrix(productId, customerName);

            //// Assert
            var numOfGrouped = 3;
            Assert.AreEqual(numOfGrouped, actualPTMatrix.PTItemMatrices.Count());
            Assert.AreEqual(1, actualPTMatrix.PTItemMatrices.First().SportTypeId);
            Assert.AreEqual(1, actualPTMatrix.PTItemMatrices.First().BetTypeId);
            Assert.AreEqual(true, actualPTMatrix.PTItemMatrices.First().IsLive);
            Assert.AreEqual(false, actualPTMatrix.PTItemMatrices.ElementAt(1).IsLive);
        }

        [Test]
        public void GetPTMatrix_Always_ReturnSportTypeName()
        {
            //// Arrange
            byte productId = 1;
            string customerName = "CustomerName";
            AssumeSettingTypeReturnFromDatabaseIsValid();

            var getPTItemMatrixDtoDummyList = GetPTItemMatrixDtoDummyList();
            _dynamicRepository.Fetch<PTItemMatrixDto>(Arg.Any<PTMatrixQueryCriteria>()).Returns(getPTItemMatrixDtoDummyList);

            var sampleSportName = "Sample SportName";
            _resourceService.GetSportTypeName(Arg.Any<int>(), Arg.Any<string>()).Returns(sampleSportName);

            //// Act
            var actualResult = _positionTakingMatrixService.GetPTMatrix(productId, customerName);

            //// Assert
            Assert.AreEqual(sampleSportName, actualResult.PTItemMatrices.First().SportTypeName);
        }

        [Test]
        public void GetPTMatrix_Always_ReturnBetTypeName()
        {
            //// Arrange
            byte productId = 1;
            string customerName = "CustomerName";
            AssumeSettingTypeReturnFromDatabaseIsValid();

            var getPTItemMatrixDtoDummyList = GetPTItemMatrixDtoDummyList();
            _dynamicRepository.Fetch<PTItemMatrixDto>(Arg.Any<PTMatrixQueryCriteria>()).Returns(getPTItemMatrixDtoDummyList);

            var sampleBetTypeName = "Sample BetTypeName";
            _resourceService.GetBetTypeName(Arg.Any<int>(), Arg.Any<string>()).Returns(sampleBetTypeName);

            //// Act
            var actualResult = _positionTakingMatrixService.GetPTMatrix(productId, customerName);

            //// Assert
            Assert.AreEqual(sampleBetTypeName, actualResult.PTItemMatrices.First().BetTypeName);
        }

        private IEnumerable<PTItemMatrixDto> GetPTItemMatrixDtoDummyList()
        {
            return new List<PTItemMatrixDto>
            {
                GetPTItemMatrixDtoDummy(true),
                GetPTItemMatrixDtoDummy(true, 1, 1, Level.Master, 10),
                GetPTItemMatrixDtoDummy(false),
                GetPTItemMatrixDtoDummy(true, 2, 2, Level.Master, 10)
            };
        }

        private PTItemMatrixDto GetPTItemMatrixDtoDummy(
            bool isLive = true,
            short sportType = 1,
            short betType = 1,
            Level customerLevel = null,
            byte superPT = 10,
            byte autoSuperPT = 10,
            bool isAutoSuperPT = false,
            byte masterPT = 10,
            byte autoMasterPT = 10,
            bool isAutoMasterPT = false,
            byte agentPT = 10,
            byte minPT = 2)
        {
            customerLevel = customerLevel ?? Level.Super;

            return new PTItemMatrixDto
            {
                IsLive = isLive,
                SportType = sportType,
                BetType = betType,
                LevelValue = customerLevel.Value,
                SuperPT = superPT,
                AutoSuperPT = autoSuperPT,
                IsAutoSuperPT = isAutoSuperPT,
                MasterPT = masterPT,
                AutoMasterPT = autoMasterPT,
                IsAutoMasterPT = isAutoMasterPT,
                AgentPT = agentPT,
                MinPT = minPT
            };
        }
    }
}