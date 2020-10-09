namespace Fanex.CustomerSetting.Matrix.Tests.Services
{
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Services;
    using Data.Repository;
    using Dtos;
    using Fanex.CustomerSetting.Matrix.Criteria;
    using Matrix.Services;
    using Matrix.Services.Impl;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
    public class CommissionMatrixServiceTests
    {
        private IResourceService _resourceService;
        private IDynamicRepository _dynamicRepository;

        private ICommissionMatrixService _commissionMatrixService;

        [SetUp]
        public void SetUp()
        {
            _resourceService = Substitute.For<IResourceService>();
            _dynamicRepository = Substitute.For<IDynamicRepository>();

            _commissionMatrixService = new CommissionMatrixService(
                _resourceService,
                _dynamicRepository);
        }

        [Test]
        public void GetCommissionMatrix_ProductIdIsZero_ThrowInvalidOperationException()
        {
            //// Arrange
            byte productId = 0;

            //// Act
            Assert.Throws<InvalidOperationException>(
                new TestDelegate(
                    () => _commissionMatrixService.GetCommissionMatrix(productId, "CustomerName")));
        }

        [TestCase("  ")]
        [TestCase("")]
        [TestCase(null)]
        public void GetCommissionMatrix_CustomerNameIsNullOrEmpty_ThrowInvalidOperationException(string customerName)
        {
            //// Arrange
            byte productId = 1;

            //// Act
            Assert.Throws<InvalidOperationException>(
                new TestDelegate(
                    () => _commissionMatrixService.GetCommissionMatrix(productId, customerName)));
        }

        [Test]
        public void GetCommissionMatrix_ProductDoesNotHaveCommissionSettingType_ThrowInvalidOperationException()
        {
            //// Arrange
            byte productId = 1;
            string customerName = "CustomerName";

            //// Act
            Assert.Throws<InvalidOperationException>(
                new TestDelegate(
                    () => _commissionMatrixService.GetCommissionMatrix(productId, customerName)));
        }

        private void AssumeDatabaseReturnValidCommission()
        {
            var commissionItemMatrixDtoDummyList = GetCommissionItemMatrixDtoDummyList();
            _dynamicRepository.Fetch<CommissionItemMatrixDto>(Arg.Any<CommissionMatrixQueryCriteria>()).Returns(commissionItemMatrixDtoDummyList);
        }

        [Test]
        public void GetCommissionMatrix_Always_CallFetchCommissionItemMatrixDtoList()
        {
            //// Arrange
            byte productId = 1;
            string customerName = "CustomerName";
            AssumeDatabaseReturnValidCommission();

            //// Act
            _commissionMatrixService.GetCommissionMatrix(productId, customerName);

            //// Assert
            _dynamicRepository.Received().Fetch<CommissionItemMatrixDto>(
                Arg.Is<CommissionMatrixQueryCriteria>(
                    criteria => criteria.ProductId == productId
                    && criteria.UserName == customerName));
        }

        [Test]
        public void GetCommissionMatrix_Always_ReturnGroupedCommissionItemMatrices()
        {
            //// Arrange
            byte productId = 1;
            string customerName = "CustomerName";
            AssumeDatabaseReturnValidCommission();

            //// Act
            var actualCommissionMatrix = _commissionMatrixService.GetCommissionMatrix(productId, customerName);

            //// Assert
            var numOfGrouped = 3;
            Assert.AreEqual(numOfGrouped, actualCommissionMatrix.CommissionItemMatrices.Count());
            Assert.AreEqual(1, actualCommissionMatrix.CommissionItemMatrices.First().SportTypeId);
            Assert.AreEqual(1, actualCommissionMatrix.CommissionItemMatrices.First().BetTypeId);
            Assert.AreEqual(0.03, actualCommissionMatrix.CommissionItemMatrices.First().CommissionItemMatrixRows.First().CommRate);
        }

        [Test]
        public void GetCommissionMatrix_Always_ReturnSportTypeName()
        {
            //// Arrange
            byte productId = 1;
            string customerName = "CustomerName";

            AssumeDatabaseReturnValidCommission();

            var sampleSportName = "Sample SportName";
            _resourceService.GetSportTypeName(Arg.Any<int>(), Arg.Any<string>()).Returns(sampleSportName);

            //// Act
            var actualResult = _commissionMatrixService.GetCommissionMatrix(productId, customerName);

            //// Assert
            Assert.AreEqual(sampleSportName, actualResult.CommissionItemMatrices.First().SportTypeName);
        }

        [Test]
        public void GetCommissionMatrix_Always_ReturnBetTypeName()
        {
            //// Arrange
            byte productId = 1;
            string customerName = "CustomerName";

            AssumeDatabaseReturnValidCommission();

            var sampleBetTypeName = "Sample BetTypeName";
            _resourceService.GetBetTypeName(Arg.Any<int>(), Arg.Any<string>()).Returns(sampleBetTypeName);

            //// Act
            var actualResult = _commissionMatrixService.GetCommissionMatrix(productId, customerName);

            //// Assert
            Assert.AreEqual(sampleBetTypeName, actualResult.CommissionItemMatrices.First().BetTypeName);
        }

        private IEnumerable<CommissionItemMatrixDto> GetCommissionItemMatrixDtoDummyList()
        {
            return new List<CommissionItemMatrixDto>
            {
                GetCommissionItemMatrixDtoDummy(),
                GetCommissionItemMatrixDtoDummy(1, 2, Level.Master, 0.07),
                GetCommissionItemMatrixDtoDummy(2, 2, Level.Master, 0.09)
            };
        }

        private CommissionItemMatrixDto GetCommissionItemMatrixDtoDummy(
            short sportType = 1,
            short betType = 1,
            Level customerLevel = null,
            double commRate = 0.03)
        {
            customerLevel = customerLevel ?? Level.Super;

            return new CommissionItemMatrixDto
            {
                SportType = sportType,
                BetType = betType,
                LevelValue = customerLevel.Value,
                CommRate = (decimal)commRate
            };
        }
    }
}