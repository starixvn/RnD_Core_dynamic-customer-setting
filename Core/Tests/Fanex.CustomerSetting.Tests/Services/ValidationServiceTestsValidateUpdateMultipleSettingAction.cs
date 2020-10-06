namespace Fanex.CustomerSetting.Tests.Services
{
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using CustomerSetting.Services;
    using CustomerSetting.Services.Impl;
    using Data.Repository;
    using Fanex.CustomerSetting.DataAccesses;
    using Logging;
    using Models;
    using NSubstitute;
    using NUnit.Framework;
    using System;

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
            _validationService.ValidateUpdateSettingAction(Arg.Any<UpdateCustomerSettingCommand>()).Returns(Result.Success);
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
                Arg.Is<UpdateCustomerSettingCommand>(updateParam =>
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
        public void ValidateUpdateMultipleSetting_ActorHasNoPermission_ReturnAccessDeniedResult()
        {
            //// Arrange
            var updateMultipleSettingParam = InstanceFactory.CreateUpdateMultipleSettingParam(1, Level.Member, 1, Level.Agent);

            AssumeValidateUpdateSettingIsSuccessful();
            _validationDataAccess.ValidatePermissionMultiple(Arg.Any<int>(), Arg.Any<string>()).Returns(Result.DataError);

            //// Act
            var actualResult = _validationService.ValidateUpdateMultipleSettingAction(updateMultipleSettingParam);

            //// Assert
            Assert.AreEqual(Result.AccessDenied.Code, actualResult.Code);
        }

        [Test]
        public void ValidateUpdateMultipleSetting_CommandIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _validationService.ValidateUpdateMultipleSettingAction(null));
        }
    }
}