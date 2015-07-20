namespace Fanex.CustomerSetting.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Handlers;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.Tests.Models;
    using Fanex.Logging;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class UpdateMultipleCustomerSettingTest
    {
        private IUpdateSettingService _settingService;
        private ISettingContext _settingContext;
        private IValidationService _validationService;
        private IProductService _productService;
        private ILogger _logger;
        private ISettingTypeServiceFactory _settingResolver;
        private ISettingTypeService _settingTypeService;

        [SetUp]
        public void Setup()
        {
            _settingContext = new SettingContextStub();
            _productService = Substitute.For<IProductService>();
            _validationService = Substitute.For<IValidationService>();
            _logger = Substitute.For<ILogger>();
            _settingResolver = Substitute.For<ISettingTypeServiceFactory>();

            _settingTypeService = Substitute.For<ISettingTypeService>();

            _settingService = new UpdateSettingService(
                _settingContext,
                _validationService,
                _logger,
                _settingResolver);
        }

        [Test]
        public void UpdateMultipleCustomerSetting_UpdateMultipleSettingParamIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            UpdateMultipleSettingParam updateMultipleSettingParam = null;

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => _settingService.UpdateCustomerSetting(updateMultipleSettingParam)));
        }

        [Test]
        public void UpdateMultipleCustomerSetting_NumberOfSettingTypesIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            UpdateMultipleSettingParam updateMultipleSettingParam = InstanceFactory.CreateUpdateMultipleSettingParam();
            updateMultipleSettingParam.Setting.SettingTypes = new List<SettingType>();

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _settingService.UpdateCustomerSetting(updateMultipleSettingParam)));
        }

        [Test]
        public void UpdateMultipleCustomerSetting_Always_CallValidateUpdateMultipleSetting()
        {
            //// Arrange
            UpdateMultipleSettingParam updateMultipleSettingParam = InstanceFactory.CreateUpdateMultipleSettingParam();

            //// Act
            _settingService.UpdateCustomerSetting(updateMultipleSettingParam);

            //// Assert
            _validationService.Received().ValidateUpdateMultipleSettingAction(updateMultipleSettingParam);
        }

        private void AssumeUpdateMultipleCustomersSettingActionIsValid(UpdateMultipleSettingParam updateParam)
        {
            _validationService.ValidateUpdateMultipleSettingAction(updateParam).Returns(Result.Success);
        }

        private void SetupSettingTypeServiceStub(bool isReturnSuccessfulResult = true)
        {
            var results = new List<SettingResult> { new SettingResult(isReturnSuccessfulResult ? Result.Success : Result.DataError) };

            _settingTypeService.UpdateMultipleCustomersSettingType(
               Arg.Any<ISettingContext>(),
               Arg.Any<IEnumerable<int>>(),
               Arg.Any<Level>(),
               Arg.Any<Level>(),
               Arg.Any<byte>(),
               Arg.Any<SettingType>()).Returns(results);

            _settingResolver.GetInstance(Arg.Any<SettingType>()).Returns(_settingTypeService);
        }

        [Test]
        public void UpdateMultipleCustomerSetting_Always_CallResolveSettingTypeService()
        {
            //// Arrange
            UpdateMultipleSettingParam updateMultipleSettingParam = InstanceFactory.CreateUpdateMultipleSettingParam();
            AssumeUpdateMultipleCustomersSettingActionIsValid(updateMultipleSettingParam);
            SetupSettingTypeServiceStub();

            //// Act
            _settingService.UpdateCustomerSetting(updateMultipleSettingParam);

            //// Assert
            var numOfSettingTypes = updateMultipleSettingParam.Setting.SettingTypes.Count();
            _settingResolver.Received(numOfSettingTypes).GetInstance(Arg.Any<SettingType>());
        }

        [Test]
        public void UpdateMultipleCustomerSetting_Always_CallUpdateMultipleCustomersSettingType()
        {
            //// Arrange
            var updateMultipleSettingParam = InstanceFactory.CreateUpdateMultipleSettingParam();
            AssumeUpdateMultipleCustomersSettingActionIsValid(updateMultipleSettingParam);
            SetupSettingTypeServiceStub();

            //// Act
            _settingService.UpdateCustomerSetting(updateMultipleSettingParam);

            //// Assert
            var numOfRunPostTasks = updateMultipleSettingParam.Setting.SettingTypes.Count();
            _settingTypeService.Received(numOfRunPostTasks)
                .UpdateMultipleCustomersSettingType(
                    Arg.Is(_settingContext),
                    Arg.Is(updateMultipleSettingParam.CustomerIds),
                    Arg.Is(updateMultipleSettingParam.Setting.CustomerLevel),
                    Arg.Is(updateMultipleSettingParam.UplineLevel),
                    Arg.Is(updateMultipleSettingParam.Setting.ProductId),
                    Arg.Any<SettingType>());
        }

        [Test]
        public void UpdateMultipleCustomerSetting_UpdateIsFailed_ReturnFailedResult()
        {
            //// Arrange
            UpdateMultipleSettingParam updateMultipleSettingParam = InstanceFactory.CreateUpdateMultipleSettingParam();
            AssumeUpdateMultipleCustomersSettingActionIsValid(updateMultipleSettingParam);
            SetupSettingTypeServiceStub(false);

            //// Act
            var actualResult = _settingService.UpdateCustomerSetting(updateMultipleSettingParam);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [Test]
        public void UpdateMultipleCustomerSetting_Updated_ReturnSuccessResult()
        {
            //// Arrange
            UpdateMultipleSettingParam updateMultipleSettingParam = InstanceFactory.CreateUpdateMultipleSettingParam();
            AssumeUpdateMultipleCustomersSettingActionIsValid(updateMultipleSettingParam);
            SetupSettingTypeServiceStub();

            //// Act
            var actualResult = _settingService.UpdateCustomerSetting(updateMultipleSettingParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [Test]
        public void UpdateMultipleCustomerSetting_UpdatedSettingTypes_ReturnResultWithCustomerName()
        {
            //// Arrange
            UpdateMultipleSettingParam updateMultipleSettingParam = InstanceFactory.CreateUpdateMultipleSettingParam();
            AssumeUpdateMultipleCustomersSettingActionIsValid(updateMultipleSettingParam);

            var successResultOfCustomer1 = new SettingResult(
                Result.Success.Code,
                Result.Success.Message,
                0,
                updateMultipleSettingParam.CustomerIds.First(),
                1,
                string.Empty);
            var successResultOfCustomer2 = new SettingResult(
                Result.Success.Code,
                Result.Success.Message,
                0,
                updateMultipleSettingParam.CustomerIds.ElementAt(1),
                1,
                string.Empty);
            var results = new List<SettingResult> { successResultOfCustomer1, successResultOfCustomer2 };

            _settingTypeService.UpdateMultipleCustomersSettingType(
               Arg.Any<ISettingContext>(),
               Arg.Any<IEnumerable<int>>(),
               Arg.Any<Level>(),
               Arg.Any<Level>(),
               Arg.Any<byte>(),
               Arg.Any<SettingType>()).Returns(results);

            _settingResolver.GetInstance(Arg.Any<SettingType>()).Returns(_settingTypeService);

            //// Act
            var actualResult = _settingService.UpdateCustomerSetting(updateMultipleSettingParam);

            //// Assert
            var updateSettingResult = actualResult as UpdateSettingResult;
            Assert.IsTrue(
                updateSettingResult.SubResults.Any(settingResult => settingResult.CustomerName == "Username1")
                && updateSettingResult.SubResults.Any(settingResult => settingResult.CustomerName == "Username2"));
        }
    }
}