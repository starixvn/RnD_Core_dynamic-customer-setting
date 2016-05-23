namespace Fanex.CustomerSetting.Tests.Services
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using CustomerSetting.Services;
    using CustomerSetting.Services.Impl;
    using Data.Repository;
    using DataAccesses;
    using Logging;
    using Models;
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
            _validationService.ValidateQuerySettingAction(Arg.Any<CustomerSettingQuery>()).Returns(Result.Success);
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
                Arg.Is<CustomerSettingQuery>(query =>
                    query.CustomerId == updateParam.Setting.CustomerId
                    && query.CustomerLevel == updateParam.Setting.CustomerLevel
                    && query.ProductId == updateParam.Setting.ProductId
                    && query.UplineLevel == updateParam.UplineLevel));
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
            _productService.Received().GetProductSettingTypes(
                Arg.Is<byte>(productId => productId == updateParam.Setting.ProductId),
                Arg.Any<byte>());
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
            _productService.GetProductSettingTypes(updateParam.Setting.ProductId, (byte)SystemType.Accounting)
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
            _productService.GetProductSettingTypes(updateParam.Setting.ProductId, (byte)SystemType.Accounting)
                .Returns(new List<SettingType>
                {
                    InstanceFactory.CreateCustomerSettingType(1)
                });

            //// Act
            var actualResult = _validationService.ValidateUpdateSettingAction(updateParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        ////[Test]
        ////public void ValidateUpdateSetting_CompanyPerformsUpdateActionInAgencySystem_ReturnAccessDeniedResult()
        ////{
        ////    //// Arrange
        ////    var updateParam = InstanceFactory.CreateUpdateSettingParam();
        ////    _settingContext.Actor.Level = Level.Company;
        ////    _settingContext.System = (byte)SystemType.Agency;

        ////    AssumeValidateQuerySettingIsSuccessful();
        ////    AssumeCustomerSettingTypesAreValid(updateParam.Setting.SettingTypes);

        ////    //// Act
        ////    var actualResult = _validationService.ValidateUpdateSettingAction(updateParam);

        ////    //// Assert
        ////    Assert.AreEqual(Result.AccessDenied.Code, actualResult.Code);
        ////}

        ////[TestCase((byte)SystemType.Accounting)]
        ////[TestCase((byte)SystemType.ExternalAccounting)]
        ////public void ValidateUpdateSetting_CompanyPerformsUpdateActionInSystemIsNotAgency_ReturnSuccessResult(byte system)
        ////{
        ////    //// Arrange
        ////    var updateParam = InstanceFactory.CreateUpdateSettingParam();
        ////    _settingContext.Actor.Level = Level.Company;
        ////    _settingContext.System = system;

        ////    AssumeValidateQuerySettingIsSuccessful();
        ////    AssumeCustomerSettingTypesAreValid(updateParam.Setting.SettingTypes);

        ////    //// Act
        ////    var actualResult = _validationService.ValidateUpdateSettingAction(updateParam);

        ////    //// Assert
        ////    Assert.IsTrue(actualResult.IsSuccessful);
        ////}

        ////[Test]
        ////public void ValidateUpdateSetting_RaisedException_AlwaysLogError()
        ////{
        ////    //// Arrange
        ////    var updateParam = InstanceFactory.CreateUpdateSettingParam();
        ////    _settingContext.Actor.Level = Level.Company;

        ////    AssumeValidateQuerySettingIsSuccessful();

        ////    var expectedException = new Exception("Expected Exception");
        ////    _productService.GetProductSettingTypes(Arg.Any<byte>(), Arg.Any<byte>()).Returns(x => { throw expectedException; });

        ////    //// Act
        ////    _validationService.ValidateUpdateSettingAction(updateParam);

        ////    //// Assert
        ////    _logger.Received().Error(
        ////        Arg.Is<string>(message => message.Contains(expectedException.Message)),
        ////        Arg.Any<CustomerSettingException>());
        ////}
    }
}