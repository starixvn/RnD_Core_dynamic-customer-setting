namespace Fanex.CustomerSetting.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.DataAccesses;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.Tests.Models;
    using Fanex.NAP.Framework.Data;
    using NSubstitute;
    using NUnit.Framework;

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1306:FieldNamesMustBeginWithLowerCaseLetter", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1013:ClosingCurlyBracketsMustBeSpacedCorrectly", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1204:StaticElementsMustAppearBeforeInstanceElements", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1012:OpeningCurlyBracketsMustBeSpacedCorrectly", Justification = "Reviewed.")]
    [TestFixture]
    public class ValidationServiceTestsValidateUpdatingSetting
    {
        private ISettingContext _settingContext;
        private ValidationService _validationService;
        private IValidationDataAccess _validationDataAccess;
        private IDynamicRepository _dynamicRepository;
        private ILoggingService _loggingService;
        private IProductService _productService;

        [SetUp]
        public void Setup()
        {
            _settingContext = new SettingContextStub();
            _productService = Substitute.For<IProductService>();
            _loggingService = Substitute.For<ILoggingService>();
            _validationDataAccess = Substitute.For<IValidationDataAccess>();
            _dynamicRepository = Substitute.For<IDynamicRepository>();

            _validationService = Substitute.ForPartsOf<ValidationService>(
                _settingContext,
                _productService,
                _loggingService,
                _validationDataAccess,
                _dynamicRepository);
        }

        private void AssumeValidateQuerySettingIsSuccessful()
        {
            _validationService.ValidateQuerySetting(Arg.Any<QuerySettingParam>()).Returns(Result.Success);
        }

        [Test]
        public void ValidateUpdatingSetting_Always_CallValidateQuerySetting()
        {
            //// Arrange
            var updatingParam = InstanceFactory.CreateUpdatingSettingParam();
            _settingContext.Actor.Level = UserLevel.Super;

            //// Act
            _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert

            _validationService.Received().ValidateQuerySetting(
                Arg.Is<QuerySettingParam>(queryParam =>
                    queryParam.CustomerId == updatingParam.Setting.CustomerId
                    && queryParam.CustomerLevel == updatingParam.Setting.CustomerLevel
                    && queryParam.ProductId == updatingParam.Setting.ProductId
                    && queryParam.UplineLevel == updatingParam.UplineLevel));
        }

        [Test]
        public void ValidateUpdatingSetting_Always_CallGetProductSettingTypes()
        {
            //// Arrange
            var updatingParam = InstanceFactory.CreateUpdatingSettingParam();

            AssumeValidateQuerySettingIsSuccessful();

            //// Act
            _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            _productService.Received().GetProductSettingTypes(Arg.Is<byte>(
                productId => productId == updatingParam.Setting.ProductId));
        }

        private void AssumeCustomerSettingTypesAreValid(IEnumerable<SettingType> dummySettingTypes)
        {
            _productService.GetProductSettingTypes(Arg.Any<byte>()).Returns(dummySettingTypes);
        }

        [Test]
        public void ValidateUpdatingSetting_CustomerSettingTypesDontMatchWithProductSettingTypes_ReturnFailedResult()
        {
            //// Arrange
            var updatingParam = InstanceFactory.CreateUpdatingSettingParam();

            AssumeValidateQuerySettingIsSuccessful();

            updatingParam.Setting.SettingTypes = new List<SettingType>
            {
                InstanceFactory.CreateCustomerSettingType(1)
            };
            _productService.GetProductSettingTypes(updatingParam.Setting.ProductId)
                .Returns(new List<SettingType>
                {
                    InstanceFactory.CreateCustomerSettingType(2)
                });

            //// Act
            var actualResult = _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateUpdatingSetting_CustomerSettingTypesMatchWithProductSettingTypes_ReturnSuccessResult()
        {
            //// Arrange
            var updatingParam = InstanceFactory.CreateUpdatingSettingParam();

            AssumeValidateQuerySettingIsSuccessful();

            updatingParam.Setting.SettingTypes = new List<SettingType>
            {
                InstanceFactory.CreateCustomerSettingType(1)
            };
            _productService.GetProductSettingTypes(updatingParam.Setting.ProductId)
                .Returns(new List<SettingType>
                {
                    InstanceFactory.CreateCustomerSettingType(1)
                });

            //// Act
            var actualResult = _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateUpdatingSetting_Always_CallValidateMethodOnEachSettingItem()
        {
            //// Arrange
            var updatingParam = InstanceFactory.CreateUpdatingSettingParam();
            var settingItemMock = Substitute.For<ISettingItem>();
            updatingParam.Setting.SettingTypes = new List<SettingType>
            {
                new SettingType
                {
                    Id = 1,
                    SettingTemplate = 1,
                    SettingItems = new List<ISettingItem>
                    {
                        settingItemMock
                    }
                }
            };

            AssumeValidateQuerySettingIsSuccessful();
            _productService.GetProductSettingTypes(updatingParam.Setting.ProductId).Returns(updatingParam.Setting.SettingTypes);

            //// Act
            _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            settingItemMock.Received().Validate();
        }

        [Test]
        public void ValidateUpdatingSetting_CompanyPerformsUpdatingActionInAgencySystem_ReturnAccessDeniedResult()
        {
            //// Arrange
            var updatingParam = InstanceFactory.CreateUpdatingSettingParam();
            _settingContext.Actor.Level = UserLevel.Company;

            AssumeValidateQuerySettingIsSuccessful();
            AssumeCustomerSettingTypesAreValid(updatingParam.Setting.SettingTypes);

            _validationService.GetCurrentSystemId().Returns((byte)SystemType.Agency);

            //// Act
            var actualResult = _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            Assert.AreEqual(Result.AccessDenied.Code, actualResult.Code);
        }

        [TestCase((byte)SystemType.Accounting)]
        [TestCase((byte)SystemType.ExternalAccounting)]
        public void ValidateUpdatingSetting_CompanyPerformsUpdatingActionInSystemIsNotAgency_ReturnSuccessResult(byte systemId)
        {
            //// Arrange
            var updatingParam = InstanceFactory.CreateUpdatingSettingParam();
            _settingContext.Actor.Level = UserLevel.Company;

            AssumeValidateQuerySettingIsSuccessful();
            AssumeCustomerSettingTypesAreValid(updatingParam.Setting.SettingTypes);

            _validationService.GetCurrentSystemId().Returns(systemId);

            //// Act
            var actualResult = _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [Test]
        public void UpdateCustomerSetting_RaisedException_CallLogValidatingUpdatingError()
        {
            //// Arrange
            UpdatingSettingParam updatingParam = InstanceFactory.CreateUpdatingSettingParam();

            //// Simulate exception
            var expectedException = new Exception("Exception");
            _dynamicRepository.Get<bool>(Arg.Any<ActorStatusQueryCriteria>()).Returns(x => { throw expectedException; });

            //// Act
            _validationService.ValidateUpdatingSetting(updatingParam);

            //// Assert
            _loggingService.Received().LogValidatingUpdatingError(
                Arg.Is<int>(customerId => customerId == updatingParam.Setting.CustomerId),
                Arg.Is<CustomerLevel?>(uplineLevel => uplineLevel == updatingParam.UplineLevel),
                Arg.Is<byte>(productId => productId == updatingParam.Setting.ProductId),
                Arg.Any<Exception>());
        }
    }
}