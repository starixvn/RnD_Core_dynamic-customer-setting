namespace Fanex.CustomerSetting.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.CustomerSetting.DataAccesses;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
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
    public class ValidationServiceTestsValidateUpdatingMultipleSetting
    {
        private ISettingContext _settingContext;
        private IValidationService _validationService;
        private IValidationDataAccess _validationDataAccess;
        private IDynamicRepository _dynamicRepository;
        private ILoggingService _loggingService;
        private IProductService _productService;
        private ILogger _logger;

        [SetUp]
        public void Setup()
        {
            _settingContext = new SettingContextStub();
            _productService = Substitute.For<IProductService>();
            _loggingService = Substitute.For<ILoggingService>();
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

        private void AssumeValidatingPermissionMultipleExecutedOnDatabaseIsSuccessful()
        {
            _validationDataAccess.ValidatePermissionMultiple(Arg.Any<int>(), Arg.Any<string>()).Returns(Result.Success);
        }

        private void AssumeValidateUpdatingSettingIsSuccessful()
        {
            _validationService.ValidateUpdatingSetting(Arg.Any<UpdatingSettingParam>()).Returns(Result.Success);
        }

        [Test]
        public void ValidateUpdatingMultipleSetting_Always_CallValidateUpdatingSetting()
        {
            //// Arrange
            var updatingMultipleSettingParam = InstanceFactory.CreateUpdatingMultipleSettingParam(1, CustomerLevel.Member, 1, CustomerLevel.Agent);

            //// Act
            _validationService.ValidateUpdatingMultipleSetting(updatingMultipleSettingParam);

            //// Assert
            _validationService.Received().ValidateUpdatingSetting(
                Arg.Is<UpdatingSettingParam>(updateParam =>
                    updateParam.Setting.CustomerId == updatingMultipleSettingParam.Setting.CustomerId
                    && updateParam.UplineLevel == updatingMultipleSettingParam.UplineLevel
                    && updateParam.Setting.ProductId == updatingMultipleSettingParam.Setting.ProductId));
        }

        [Test]
        public void ValidateUpdatingMultipleSetting_CustomerIsMember_AlwaysCallValidatePermissionMultiple()
        {
            //// Arrange
            var updatingMultipleSettingParam = InstanceFactory.CreateUpdatingMultipleSettingParam(1, CustomerLevel.Member, 1, CustomerLevel.Agent);

            AssumeValidatingPermissionMultipleExecutedOnDatabaseIsSuccessful();
            AssumeValidateUpdatingSettingIsSuccessful();

            //// Act
            _validationService.ValidateUpdatingMultipleSetting(updatingMultipleSettingParam);

            //// Assert
            _validationDataAccess.Received().ValidatePermissionMultiple(
                Arg.Is<int>(actorId => actorId == _settingContext.Actor.Id),
                Arg.Is<string>(memberIds => memberIds == string.Join(",", updatingMultipleSettingParam.CustomerIds)));
        }

        [Test]
        public void ValidateUpdatingMultipleSetting_CustomerIsNotMember_DontCallValidatePermissionMultiple()
        {
            //// Arrange
            var updatingMultipleSettingParam = InstanceFactory.CreateUpdatingMultipleSettingParam();

            AssumeValidatingPermissionMultipleExecutedOnDatabaseIsSuccessful();
            AssumeValidateUpdatingSettingIsSuccessful();

            //// Act
            _validationService.ValidateUpdatingMultipleSetting(updatingMultipleSettingParam);

            //// Assert
            _validationDataAccess.DidNotReceive().ValidatePermissionMultiple(Arg.Any<int>(), Arg.Any<string>());
        }

        ////[Test]
        ////public void UpdateMultipleCustomerSetting_RaisedException_CallLogUpdatingMultipleCustomersSettingTypeError()
        ////{
        ////    //// Arrange
        ////    var updatingMultipleSettingParam = InstanceFactory.CreateUpdatingMultipleSettingParam(1, CustomerLevel.Member, 1, CustomerLevel.Agent);
        ////    AssumeValidateUpdatingSettingIsSuccessful();

        ////    //// Simulate throw exception
        ////    var expectedException = new Exception("Exception");
        ////    _validationDataAccess.ValidatePermissionMultiple(Arg.Any<int>(), Arg.Any<string>()).Returns(x => { throw expectedException; });

        ////    //// Act
        ////    _validationService.ValidateUpdatingMultipleSetting(updatingMultipleSettingParam);

        ////    //// Assert
        ////    _loggingService.Received().LogValidatingUpdatingMultipleError(
        ////        Arg.Is<IEnumerable<int>>(customerIds => string.Join(",", customerIds) == string.Join(",", updatingMultipleSettingParam.CustomerIds)),
        ////        Arg.Is<CustomerLevel?>(uplineLevel => uplineLevel == updatingMultipleSettingParam.UplineLevel),
        ////        Arg.Is<byte>(productId => productId == updatingMultipleSettingParam.Setting.ProductId),
        ////        Arg.Any<Exception>());
        ////}
    }
}