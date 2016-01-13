namespace Fanex.CustomerSetting.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Exceptions;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Models.Results;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.Tests.Models;
    using Fanex.Logging;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class UpdateSettingServiceTests
    {
        private IUpdateSettingService _settingService;
        private ISettingContext _settingContext;
        private IValidationService _validationService;
        private IProductService _productService;
        private IPluggedTaskService _pluggedTaskService;
        private ILogger _logger;
        private ISettingTypeServiceFactory _settingTypeServiceFactory;
        private ISettingTypeService _settingTypeService;

        [SetUp]
        public void Setup()
        {
            _settingContext = new SettingContextStub();
            _productService = Substitute.For<IProductService>();
            _validationService = Substitute.For<IValidationService>();
            _pluggedTaskService = Substitute.For<IPluggedTaskService>();
            _logger = Substitute.For<ILogger>();
            _settingTypeServiceFactory = Substitute.For<ISettingTypeServiceFactory>();

            _settingTypeService = Substitute.For<ISettingTypeService>();

            _settingService = new UpdateSettingService(
                _settingContext,
                _validationService,
                _pluggedTaskService,
                _logger,
                _settingTypeServiceFactory);
        }

        [Test]
        public void UpdateCustomerSetting_UpdateSettingParamIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            UpdateCustomerSettingCommand updateParam = null;

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => _settingService.Update(updateParam)));
        }

        [Test]
        public void UpdateCustomerSetting_Always_CallValidateUpdateSetting()
        {
            //// Arrange
            UpdateCustomerSettingCommand updateParam = InstanceFactory.CreateUpdateSettingParam();

            //// Act
            _settingService.Update(updateParam);

            //// Assert
            _validationService.Received().ValidateUpdateSettingAction(Arg.Is<UpdateCustomerSettingCommand>(
                actualSettingCommand => actualSettingCommand == updateParam));
        }

        [Test]
        public void UpdateCustomerSetting_ValidationIsFailed_ReturnFailedResult()
        {
            //// Arrange
            UpdateCustomerSettingCommand updateParam = InstanceFactory.CreateUpdateSettingParam();
            _validationService.ValidateUpdateSettingAction(Arg.Any<UpdateCustomerSettingCommand>()).Returns(Result.AccessDenied);

            //// Act
            var actualUpdateResult = _settingService.Update(updateParam);

            //// Assert
            Assert.AreEqual(Result.AccessDenied.Code, actualUpdateResult.Code);
        }

        private void AssumeUpdateCustomerSettingActionIsValid(UpdateCustomerSettingCommand updateParam)
        {
            _validationService.ValidateUpdateSettingAction(updateParam).Returns(Result.Success);
        }

        [Test]
        public void UpdateCustomerSetting_Always_CallCheckSettingTypeValid()
        {
            //// Arrange
            UpdateCustomerSettingCommand updateParam = InstanceFactory.CreateUpdateSettingParam();

            AssumeUpdateCustomerSettingActionIsValid(updateParam);

            var settingItemMock = Substitute.For<ISettingItem>();
            settingItemMock.Validate().Returns(Result.DataError);

            updateParam.Setting.SettingTypes = new List<SettingType>
            {
                new SettingType
                {
                    SettingItems = new List<ISettingItem> { settingItemMock }
                }
            };

            //// Act
            var result = _settingService.Update(updateParam);

            //// Assert
            settingItemMock.Received().Validate();
        }

        [Test]
        public void UpdateCustomerSetting_SettingTypeIsNotValid_ReturnFailedResult()
        {
            //// Arrange
            UpdateCustomerSettingCommand updateParam = InstanceFactory.CreateUpdateSettingParam();

            AssumeUpdateCustomerSettingActionIsValid(updateParam);

            var settingItemStub = Substitute.For<ISettingItem>();
            settingItemStub.Validate().Returns(Result.DataError);

            updateParam.Setting.SettingTypes = new List<SettingType>
            {
                new SettingType
                {
                    SettingItems = new List<ISettingItem> { settingItemStub }
                }
            };

            //// Act
            var result = _settingService.Update(updateParam);

            //// Assert
            var updateSettingResult = result as UpdateSettingResult;
            Assert.IsTrue(updateSettingResult.IsNotOk);
            Assert.AreEqual(Result.DataError.Message, updateSettingResult.SubResults.First().Message);
        }

        [Test]
        public void UpdateCustomerSetting_SettingTypeIsNotValid_AlwaysCallLogError()
        {
            //// Arrange
            UpdateCustomerSettingCommand updateParam = InstanceFactory.CreateUpdateSettingParam();

            AssumeUpdateCustomerSettingActionIsValid(updateParam);

            var settingItemStub = Substitute.For<ISettingItem>();
            settingItemStub.Validate().Returns(Result.DataError);

            updateParam.Setting.SettingTypes = new List<SettingType>
            {
                new SettingType
                {
                    SettingItems = new List<ISettingItem> { settingItemStub }
                }
            };

            //// Act
            var result = _settingService.Update(updateParam);

            //// Assert
            _logger.Received().Error(
                Arg.Is<string>(message => message.Contains(Result.DataError.Message)),
                Arg.Any<CustomerSettingException>());
        }

        [Test]
        public void UpdateCustomerSetting_Always_CallResolveSettingTypeService()
        {
            //// Arrange
            UpdateCustomerSettingCommand updateParam = InstanceFactory.CreateUpdateSettingParam();
            AssumeUpdateCustomerSettingActionIsValid(updateParam);

            //// Act
            _settingService.Update(updateParam);

            //// Assert
            _settingTypeServiceFactory.Received(updateParam.Setting.SettingTypes.Count()).GetInstance(Arg.Any<SettingType>());
        }

        private void SetupSettingTypeServiceStub(bool isReturnSuccessfulResult = true)
        {
            var result = new SettingResult(isReturnSuccessfulResult ? Result.Success : Result.DataError);

            _settingTypeService.UpdateCustomerSettingType(
               Arg.Any<ISettingContext>(),
               Arg.Any<int>(),
               Arg.Any<Level>(),
               Arg.Any<Level>(),
               Arg.Any<byte>(),
               Arg.Any<SettingType>()).Returns(result);

            _settingTypeServiceFactory.GetInstance(Arg.Any<SettingType>()).Returns(_settingTypeService);
        }

        [Test]
        public void UpdateCustomerSetting_Always_CallUpdateCustomerSettingType()
        {
            //// Arrange
            UpdateCustomerSettingCommand updateParam = InstanceFactory.CreateUpdateSettingParam();
            AssumeUpdateCustomerSettingActionIsValid(updateParam);
            SetupSettingTypeServiceStub();

            //// Act
            _settingService.Update(updateParam);

            //// Assert
            _settingTypeService.Received(updateParam.Setting.SettingTypes.Count()).UpdateCustomerSettingType(
                Arg.Is(_settingContext),
                Arg.Is(updateParam.Setting.CustomerId),
                Arg.Is(updateParam.Setting.CustomerLevel),
                Arg.Is(updateParam.UplineLevel),
                Arg.Is(updateParam.Setting.ProductId),
                Arg.Any<SettingType>());
        }

        [Test]
        public void UpdateCustomerSetting_UpdateSettingTypeIsFailed_ReturnFailedResult()
        {
            //// Arrange
            UpdateCustomerSettingCommand updateParam = InstanceFactory.CreateUpdateSettingParam();
            AssumeUpdateCustomerSettingActionIsValid(updateParam);
            SetupSettingTypeServiceStub(false);

            //// Act
            var actualUpdateResult = _settingService.Update(updateParam);

            //// Assert
            Assert.IsFalse(actualUpdateResult.IsSuccessful);
        }

        [Test]
        public void UpdateCustomerSetting_UpdatedSetting_ReturnSuccessResult()
        {
            //// Arrange
            UpdateCustomerSettingCommand updateParam = InstanceFactory.CreateUpdateSettingParam();
            AssumeUpdateCustomerSettingActionIsValid(updateParam);
            SetupSettingTypeServiceStub();

            //// Act
            var actualUpdateResult = _settingService.Update(updateParam);

            //// Assert
            Assert.IsTrue(actualUpdateResult.IsSuccessful);
        }

        [Test]
        public void UpdateCustomerSetting_RaisedException_AlwaysCallLogError()
        {
            //// Arrange
            UpdateCustomerSettingCommand updateParam = InstanceFactory.CreateUpdateSettingParam();
            AssumeUpdateCustomerSettingActionIsValid(updateParam);

            var expectedException = new Exception("Expected Exception");
            _settingTypeServiceFactory.GetInstance(Arg.Any<SettingType>()).Returns(x => { throw expectedException; });

            //// Act
            _settingService.Update(updateParam);

            //// Assert
            _logger.Received().Error(
                Arg.Is<string>(message => message.Contains(expectedException.Message)),
                Arg.Any<CustomerSettingException>());
        }
    }
}