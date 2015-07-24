namespace Fanex.CustomerSetting.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.CustomerSetting.DataAccesses;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Exceptions;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Params;
    using Fanex.CustomerSetting.Results;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.Tests.Models;
    using Fanex.Logging;
    using Fanex.NAP.Framework.Data;
    using NSubstitute;
    using NUnit.Framework;

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1306:FieldNamesMustBeginWithLowerCaseLetter", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1013:ClosingCurlyBracketsMustBeSpacedCorrectly", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1204:StaticElementsMustAppearBeforeInstanceElements", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1012:OpeningCurlyBracketsMustBeSpacedCorrectly", Justification = "Reviewed.")]
    [TestFixture]
    public class ValidationServiceTestsValidateUpdateSettingAction
    {
        private ISettingContext _settingContext;
        private ValidationService _validationService;
        private IValidationDataAccess _validationDataAccess;
        private IDynamicRepository _dynamicRepository;
        private IProductService _productService;
        private ILogger _logger;

        [SetUp]
        public void Setup()
        {
            _settingContext = new SettingContextStub();
            _productService = Substitute.For<IProductService>();
            _validationDataAccess = Substitute.For<IValidationDataAccess>();
            _dynamicRepository = Substitute.For<IDynamicRepository>();
            _logger = Substitute.For<ILogger>();

            _validationService = Substitute.ForPartsOf<ValidationService>(
                _settingContext,
                _productService,
                _validationDataAccess,
                _dynamicRepository,
                _logger);
        }

        private void AssumeValidateQuerySettingIsSuccessful()
        {
            _validationService.ValidateQuerySettingAction(Arg.Any<QueryCustomerSettingParam>()).Returns(Result.Success);
        }

        [Test]
        public void ValidateUpdateSetting_Always_CallValidateQuerySetting()
        {
            //// Arrange
            var updateParam = InstanceFactory.CreateUpdateSettingParam();
            _settingContext.Actor.Level = Level.Super;

            //// Act
            _validationService.ValidateUpdateSettingAction(updateParam);

            //// Assert

            _validationService.Received().ValidateQuerySettingAction(
                Arg.Is<QueryCustomerSettingParam>(queryParam =>
                    queryParam.CustomerId == updateParam.Setting.CustomerId
                    && queryParam.CustomerLevel == updateParam.Setting.CustomerLevel
                    && queryParam.ProductId == updateParam.Setting.ProductId
                    && queryParam.UplineLevel == updateParam.UplineLevel));
        }

        [Test]
        public void ValidateUpdateSetting_Always_CallGetProductSettingTypes()
        {
            //// Arrange
            var updateParam = InstanceFactory.CreateUpdateSettingParam();

            AssumeValidateQuerySettingIsSuccessful();

            //// Act
            _validationService.ValidateUpdateSettingAction(updateParam);

            //// Assert
            _productService.Received().GetProductSettingTypes(Arg.Is<byte>(
                productId => productId == updateParam.Setting.ProductId));
        }

        private void AssumeCustomerSettingTypesAreValid(IEnumerable<SettingType> dummySettingTypes)
        {
            _productService.GetProductSettingTypes(Arg.Any<byte>()).Returns(dummySettingTypes);
        }

        [Test]
        public void ValidateUpdateSetting_CustomerSettingTypesDontMatchWithProductSettingTypes_ReturnFailedResult()
        {
            //// Arrange
            var updateParam = InstanceFactory.CreateUpdateSettingParam();

            AssumeValidateQuerySettingIsSuccessful();

            updateParam.Setting.SettingTypes = new List<SettingType>
            {
                InstanceFactory.CreateCustomerSettingType(1)
            };
            _productService.GetProductSettingTypes(updateParam.Setting.ProductId)
                .Returns(new List<SettingType>
                {
                    InstanceFactory.CreateCustomerSettingType(2)
                });

            //// Act
            var actualResult = _validationService.ValidateUpdateSettingAction(updateParam);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateUpdateSetting_CustomerSettingTypesMatchWithProductSettingTypes_ReturnSuccessResult()
        {
            //// Arrange
            var updateParam = InstanceFactory.CreateUpdateSettingParam();

            AssumeValidateQuerySettingIsSuccessful();

            updateParam.Setting.SettingTypes = new List<SettingType>
            {
                InstanceFactory.CreateCustomerSettingType(1)
            };
            _productService.GetProductSettingTypes(updateParam.Setting.ProductId)
                .Returns(new List<SettingType>
                {
                    InstanceFactory.CreateCustomerSettingType(1)
                });

            //// Act
            var actualResult = _validationService.ValidateUpdateSettingAction(updateParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateUpdateSetting_CompanyPerformsUpdateActionInAgencySystem_ReturnAccessDeniedResult()
        {
            //// Arrange
            var updateParam = InstanceFactory.CreateUpdateSettingParam();
            _settingContext.Actor.Level = Level.Company;

            AssumeValidateQuerySettingIsSuccessful();
            AssumeCustomerSettingTypesAreValid(updateParam.Setting.SettingTypes);

            _validationService.GetCurrentSystemId().Returns((byte)SystemType.Agency);

            //// Act
            var actualResult = _validationService.ValidateUpdateSettingAction(updateParam);

            //// Assert
            Assert.AreEqual(Result.AccessDenied.Code, actualResult.Code);
        }

        [TestCase((byte)SystemType.Accounting)]
        [TestCase((byte)SystemType.ExternalAccounting)]
        public void ValidateUpdateSetting_CompanyPerformsUpdateActionInSystemIsNotAgency_ReturnSuccessResult(byte systemId)
        {
            //// Arrange
            var updateParam = InstanceFactory.CreateUpdateSettingParam();
            _settingContext.Actor.Level = Level.Company;

            AssumeValidateQuerySettingIsSuccessful();
            AssumeCustomerSettingTypesAreValid(updateParam.Setting.SettingTypes);

            _validationService.GetCurrentSystemId().Returns(systemId);

            //// Act
            var actualResult = _validationService.ValidateUpdateSettingAction(updateParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateUpdateSetting_RaisedException_AlwaysLogError()
        {
            //// Arrange
            var updateParam = InstanceFactory.CreateUpdateSettingParam();
            _settingContext.Actor.Level = Level.Company;

            AssumeValidateQuerySettingIsSuccessful();

            var expectedException = new Exception("Expected Exception");
            _productService.GetProductSettingTypes(Arg.Any<byte>()).Returns(x => { throw expectedException; });

            //// Act
            _validationService.ValidateUpdateSettingAction(updateParam);

            //// Assert
            _logger.Received().Error(
                Arg.Is<string>(message => message.Contains(expectedException.Message)),
                Arg.Any<CustomerSettingException>());
        }
    }
}