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
    public class CommissionMatrixServiceTests
    {
        private IResourceService _resourceService;
        private IProductService _productService;
        private IDynamicRepository _dynamicRepository;

        private ICommissionMatrixService _commissionMatrixService;

        [SetUp]
        public void SetUp()
        {
            _resourceService = Substitute.For<IResourceService>();
            _productService = Substitute.For<IProductService>();
            _dynamicRepository = Substitute.For<IDynamicRepository>();

            _commissionMatrixService = new CommissionMatrixService(
                _resourceService,
                _productService,
                _dynamicRepository);
        }

        [Test]
        public void GetCommissionMatrix_ProductIdIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            byte productId = 0;

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _commissionMatrixService.GetCommissionMatrix(productId, "CustomerName")));
        }

        [TestCase("  ")]
        [TestCase("")]
        [TestCase(null)]
        public void GetCommissionMatrix_CustomerNameIsNullOrEmpty_ThrowArgumentOutOfRangeException(string customerName)
        {
            //// Arrange
            byte productId = 1;

            //// Act
            Assert.Throws<ArgumentException>(
                new TestDelegate(
                    () => _commissionMatrixService.GetCommissionMatrix(productId, customerName)));
        }

        private SettingType AssumeSettingTypeReturnFromDatabaseIsValid(SettingType dummySettingType = null)
        {
            dummySettingType = dummySettingType ?? InstanceFactory.CreateProductSettingType(3);
            _productService.GetProductSettingType(Arg.Any<byte>(), Arg.Any<byte>()).Returns(dummySettingType);

            return dummySettingType;
        }

        [Test]
        public void GetCommissionMatrix_Always_ReturnSettingLevel()
        {
            //// Arrange
            byte productId = 1;
            string customerName = "CustomerName";
            var dummySettingType = AssumeSettingTypeReturnFromDatabaseIsValid();

            //// Act
            var actualPTMatrix = _commissionMatrixService.GetCommissionMatrix(productId, customerName);

            //// Assert
            Assert.AreEqual(dummySettingType.SettingLevel, actualPTMatrix.SettingLevel);
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

        [Test]
        public void GetCommissionMatrix_Always_CallFetchCommissionItemMatrixDtoList()
        {
            //// Arrange
            byte productId = 1;
            string customerName = "CustomerName";
            AssumeSettingTypeReturnFromDatabaseIsValid();

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
            AssumeSettingTypeReturnFromDatabaseIsValid();

            var commissionItemMatrixDtoDummyList = GetCommissionItemMatrixDtoDummyList();
            _dynamicRepository.Fetch<CommissionItemMatrixDto>(Arg.Any<CommissionMatrixQueryCriteria>()).Returns(commissionItemMatrixDtoDummyList);

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
            AssumeSettingTypeReturnFromDatabaseIsValid();

            var commissionItemMatrixDtoDummyList = GetCommissionItemMatrixDtoDummyList();
            _dynamicRepository.Fetch<CommissionItemMatrixDto>(Arg.Any<CommissionMatrixQueryCriteria>()).Returns(commissionItemMatrixDtoDummyList);

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
            AssumeSettingTypeReturnFromDatabaseIsValid();

            var commissionItemMatrixDtoDummyList = GetCommissionItemMatrixDtoDummyList();
            _dynamicRepository.Fetch<CommissionItemMatrixDto>(Arg.Any<CommissionMatrixQueryCriteria>()).Returns(commissionItemMatrixDtoDummyList);

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
                CustomerLevel = customerLevel,
                CommRate = (decimal)commRate
            };
        }
    }
}