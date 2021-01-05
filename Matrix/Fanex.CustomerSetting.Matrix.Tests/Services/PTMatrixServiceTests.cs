namespace Fanex.CustomerSetting.Matrix.Tests.Services
{
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Services;
    using Data.Repository;
    using Dtos;
    using Fanex.CustomerSetting.Matrix.Criteria;
    using Fanex.CustomerSetting.Matrix.Models;
    using Matrix.Services;
    using Matrix.Services.Impl;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
    public class PTMatrixServiceTests
    {
        private IResourceService _resourceService;
        private IDynamicRepository _dynamicRepository;

        private IPTMatrixService _positionTakingMatrixService;
        private SettingMatrixQuery _settingMatrixQuery;

        [SetUp]
        public void SetUp()
        {
            _resourceService = Substitute.For<IResourceService>();
            _dynamicRepository = Substitute.For<IDynamicRepository>();

            _settingMatrixQuery = new SettingMatrixQuery
            {
                BetTypeId = 1,
                CustomerName = "CustomerName",
                SportTypeId = 1,
                ProductId = 1
            };

            _positionTakingMatrixService = new PTMatrixService(
                _resourceService,
                _dynamicRepository);
        }

        [Test]
        public void GetPTMatrix_SportTypeIdIsZero_ThrowInvalidOperationException()
        {
            //// Arrange
            _settingMatrixQuery.SportTypeId = 0;

            //// Act
            Assert.Throws<InvalidOperationException>(
                new TestDelegate(
                    () => _positionTakingMatrixService.GetPTMatrix(_settingMatrixQuery)));
        }

        [TestCase("  ")]
        [TestCase("")]
        [TestCase(null)]
        public void GetPTMatrix_CustomerNameIsNullOrEmpty_ThrowInvalidOperationException(string customerName)
        {
            //// Arrange
            _settingMatrixQuery.CustomerName = customerName;

            //// Act
            Assert.Throws<InvalidOperationException>(
                new TestDelegate(
                    () => _positionTakingMatrixService.GetPTMatrix(_settingMatrixQuery)));
        }

        [Test]
        public void GetPTMatrix_Always_CallGetIsAllowMasterAutoPT()
        {
            //// Arrange
            byte productId = 1;
            string masterName = "Master Name";
            _settingMatrixQuery.ProductId = productId;
            AssumeDatabaseReturnValidPT();

            //// Act
            _positionTakingMatrixService.GetPTMatrix(_settingMatrixQuery, masterName);

            //// Assert
            _dynamicRepository.Received().Get<bool>(Arg.Is<IsDisableMasterAutoPTQueryCriteria>(
                    criteria => criteria.UserName == masterName));
        }

        private void AssumeMasterIsAllowedAutoPT()
        {
            _dynamicRepository.Get<bool>(Arg.Any<IsDisableMasterAutoPTQueryCriteria>()).Returns(false);
        }

        [Test]
        public void GetPTMatrix_Always_ReturnIsAllowMasterAutoPT()
        {
            //// Arrange
            AssumeDatabaseReturnValidPT();
            AssumeMasterIsAllowedAutoPT();

            //// Act
            var actualPTMatrix = _positionTakingMatrixService.GetPTMatrix(_settingMatrixQuery, "master name");

            //// Assert
            Assert.IsTrue(actualPTMatrix.IsAllowMasterAutoPT);
        }

        private void AssumeDatabaseReturnValidPT()
        {
            var getPTItemMatrixDtoDummyList = GetPTItemMatrixDtoDummyList();
            _dynamicRepository.Fetch<PTItemMatrixDto>(Arg.Any<PTMatrixQueryCriteria>()).Returns(getPTItemMatrixDtoDummyList);
        }

        [Test]
        public void GetPTMatrix_Always_CallFetchPTItemMatrixDtoList()
        {
            //// Arrange
            short sportTypeId = 1;
            short betTypeId = 1;
            _settingMatrixQuery.SportTypeId = sportTypeId;
            _settingMatrixQuery.BetTypeId = betTypeId;
            string customerName = "CustomerName";
            _settingMatrixQuery.CustomerName = customerName;
            AssumeDatabaseReturnValidPT();

            //// Act
            _positionTakingMatrixService.GetPTMatrix(_settingMatrixQuery);

            //// Assert
            _dynamicRepository.Received().Fetch<PTItemMatrixDto>(
                Arg.Is<PTMatrixQueryCriteria>(
                    criteria => criteria.SportType == sportTypeId
                    && criteria.UserName == customerName
                    && criteria.BetType == betTypeId));
        }

        [Test]
        public void GetPTMatrix_Always_ReturnGroupedPTItemMatrices()
        {
            //// Arrange
            AssumeDatabaseReturnValidPT();

            //// Act
            var actualPTMatrix = _positionTakingMatrixService.GetPTMatrix(_settingMatrixQuery);

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
            AssumeDatabaseReturnValidPT();

            var sampleSportName = "Sample SportName";
            _resourceService.GetSportTypeName(Arg.Any<int>(), Arg.Any<string>()).Returns(sampleSportName);

            //// Act
            var actualResult = _positionTakingMatrixService.GetPTMatrix(_settingMatrixQuery);

            //// Assert
            Assert.AreEqual(sampleSportName, actualResult.PTItemMatrices.First().SportTypeName);
        }

        [Test]
        public void GetPTMatrix_Always_ReturnBetTypeName()
        {
            //// Arrange
            AssumeDatabaseReturnValidPT();

            var sampleBetTypeName = "Sample BetTypeName";
            _resourceService.GetBetTypeName(Arg.Any<int>(), Arg.Any<string>()).Returns(sampleBetTypeName);

            //// Act
            var actualResult = _positionTakingMatrixService.GetPTMatrix(_settingMatrixQuery);

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