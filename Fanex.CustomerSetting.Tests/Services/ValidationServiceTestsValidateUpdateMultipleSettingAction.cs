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
    public class ValidationServiceTestsValidateUpdateMultipleSettingAction
    {
        private ISettingContext _settingContext;
        private IValidationService _validationService;
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

        private void AssumeValidatingPermissionMultipleExecutedOnDatabaseIsSuccessful()
        {
            _validationDataAccess.ValidatePermissionMultiple(Arg.Any<int>(), Arg.Any<string>()).Returns(Result.Success);
        }

        private void AssumeValidateUpdateSettingIsSuccessful()
        {
            _validationService.ValidateUpdateSettingAction(Arg.Any<UpdateSettingParam>()).Returns(Result.Success);
        }

        [Test]
        public void ValidateUpdateMultipleSetting_Always_CallValidateUpdateSetting()
        {
            //// Arrange
            var updateMultipleSettingParam = InstanceFactory.CreateUpdateMultipleSettingParam(1, Level.Member, 1, Level.Agent);

            //// Act
            _validationService.ValidateUpdateMultipleSettingAction(updateMultipleSettingParam);

            //// Assert
            _validationService.Received().ValidateUpdateSettingAction(
                Arg.Is<UpdateSettingParam>(updateParam =>
                    updateParam.Setting.CustomerId == updateMultipleSettingParam.Setting.CustomerId
                    && updateParam.UplineLevel == updateMultipleSettingParam.UplineLevel
                    && updateParam.Setting.ProductId == updateMultipleSettingParam.Setting.ProductId));
        }

        [Test]
        public void ValidateUpdateMultipleSetting_CustomerIsMember_AlwaysCallValidatePermissionMultiple()
        {
            //// Arrange
            var updateMultipleSettingParam = InstanceFactory.CreateUpdateMultipleSettingParam(1, Level.Member, 1, Level.Agent);

            AssumeValidatingPermissionMultipleExecutedOnDatabaseIsSuccessful();
            AssumeValidateUpdateSettingIsSuccessful();

            //// Act
            _validationService.ValidateUpdateMultipleSettingAction(updateMultipleSettingParam);

            //// Assert
            _validationDataAccess.Received().ValidatePermissionMultiple(
                Arg.Is<int>(actorId => actorId == _settingContext.Actor.Id),
                Arg.Is<string>(memberIds => memberIds == string.Join(",", updateMultipleSettingParam.CustomerIds)));
        }

        [Test]
        public void ValidateUpdateMultipleSetting_CustomerIsNotMember_DontCallValidatePermissionMultiple()
        {
            //// Arrange
            var updateMultipleSettingParam = InstanceFactory.CreateUpdateMultipleSettingParam();

            AssumeValidatingPermissionMultipleExecutedOnDatabaseIsSuccessful();
            AssumeValidateUpdateSettingIsSuccessful();

            //// Act
            _validationService.ValidateUpdateMultipleSettingAction(updateMultipleSettingParam);

            //// Assert
            _validationDataAccess.DidNotReceive().ValidatePermissionMultiple(Arg.Any<int>(), Arg.Any<string>());
        }
    }
}