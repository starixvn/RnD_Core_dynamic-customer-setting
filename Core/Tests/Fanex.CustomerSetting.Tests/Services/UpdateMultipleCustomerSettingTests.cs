namespace Fanex.CustomerSetting.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using CustomerSetting.Services;
    using CustomerSetting.Services.Impl;
    using Exceptions;
    using Logging;
    using Models;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class UpdateMultipleCustomerSettingTests
    {
        private IUpdateSettingService _settingService;
        private ISettingContext _settingContext;
        private IValidationService _validationService;
        private ITaskService _taskService;
        private ILogger _logger;
        private ISettingTypeServiceFactory _settingTypeServiceFactory;
        private ISettingTypeService _settingTypeService;

        [SetUp]
        public void Setup()
        {
            _settingContext = new SettingContextStub();
            _validationService = Substitute.For<IValidationService>();
            _taskService = Substitute.For<ITaskService>();
            _logger = Substitute.For<ILogger>();
            _settingTypeServiceFactory = Substitute.For<ISettingTypeServiceFactory>();

            _settingTypeService = Substitute.For<ISettingTypeService>();

            _settingService = new UpdateSettingService(
                _settingContext,
                _validationService,
                _taskService,
                _logger,
                _settingTypeServiceFactory);
        }

        [Test]
        public void UpdateMultipleCustomerSetting_UpdateMultipleSettingParamIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            MultipleUpdateCustomerSettingCommand updateMultipleSettingParam = null;

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => _settingService.Update(updateMultipleSettingParam)));
        }

        [Test]
        public void UpdateMultipleCustomerSetting_NumberOfSettingTypesIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            MultipleUpdateCustomerSettingCommand updateMultipleSettingParam = InstanceFactory.CreateUpdateMultipleSettingParam();
            updateMultipleSettingParam.Setting.SettingTypes = new List<SettingType>();

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _settingService.Update(updateMultipleSettingParam)));
        }

        [Test]
        public void UpdateMultipleCustomerSetting_Always_CallValidateUpdateMultipleSetting()
        {
            //// Arrange
            MultipleUpdateCustomerSettingCommand updateMultipleSettingParam = InstanceFactory.CreateUpdateMultipleSettingParam();

            //// Act
            _settingService.Update(updateMultipleSettingParam);

            //// Assert
            _validationService.Received().ValidateUpdateMultipleSettingAction(updateMultipleSettingParam);
        }

        private void AssumeUpdateMultipleCustomersSettingActionIsValid(MultipleUpdateCustomerSettingCommand updateParam)
        {
            _validationService.ValidateUpdateMultipleSettingAction(updateParam).Returns(Result.Success);
        }

        [Test]
        public void UpdateMultipleCustomerSetting_Always_CallCheckSettingTypeValid()
        {
            //// Arrange
            MultipleUpdateCustomerSettingCommand updateMultipleSettingParam = InstanceFactory.CreateUpdateMultipleSettingParam();

            AssumeUpdateMultipleCustomersSettingActionIsValid(updateMultipleSettingParam);

            var settingItemMock = Substitute.For<ISettingItem>();
            settingItemMock.Validate().Returns(Result.DataError);

            updateMultipleSettingParam.Setting.SettingTypes = new List<SettingType>
            {
                new SettingType
                {
                    SettingItems = new List<ISettingItem> { settingItemMock }
                }
            };

            //// Act
            _settingService.Update(updateMultipleSettingParam);

            //// Assert
            settingItemMock.Received().Validate();
        }

        [Test]
        public void UpdateMultipleCustomerSetting_SettingTypeIsNotValid_ReturnFailedResult()
        {
            //// Arrange
            MultipleUpdateCustomerSettingCommand updateMultipleSettingParam = InstanceFactory.CreateUpdateMultipleSettingParam();

            AssumeUpdateMultipleCustomersSettingActionIsValid(updateMultipleSettingParam);

            var settingItemStub = Substitute.For<ISettingItem>();
            settingItemStub.Validate().Returns(Result.DataError);

            updateMultipleSettingParam.Setting.SettingTypes = new List<SettingType>
            {
                new SettingType
                {
                    SettingItems = new List<ISettingItem> { settingItemStub }
                }
            };

            //// Act
            var result = _settingService.Update(updateMultipleSettingParam);

            //// Assert
            var updateSettingResult = result as UpdateSettingResult;
            Assert.IsTrue(updateSettingResult.IsNotOk);
            Assert.AreEqual(Result.DataError.Message, updateSettingResult.SubResults.First().Message);
        }

        [Test]
        public void UpdateMultipleCustomerSetting_SettingTypeIsNotValid_AlwaysCallLogError()
        {
            //// Arrange
            MultipleUpdateCustomerSettingCommand updateMultipleSettingParam = InstanceFactory.CreateUpdateMultipleSettingParam();

            AssumeUpdateMultipleCustomersSettingActionIsValid(updateMultipleSettingParam);

            var settingItemStub = Substitute.For<ISettingItem>();
            settingItemStub.Validate().Returns(Result.DataError);

            updateMultipleSettingParam.Setting.SettingTypes = new List<SettingType>
            {
                new SettingType
                {
                    SettingItems = new List<ISettingItem> { settingItemStub }
                }
            };

            //// Act
            _settingService.Update(updateMultipleSettingParam);

            //// Assert
            _logger.Received().Error(
                Arg.Is<string>(message => message.Contains(Result.DataError.Message)),
                Arg.Any<CustomerSettingException>());
        }

        private void SetupSettingTypeServiceStub(bool isReturnSuccessfulResult = true)
        {
            var results = new List<SettingResult> { new SettingResult(isReturnSuccessfulResult ? Result.Success : Result.DataError) };

            _settingTypeService.UpdateMultipleCustomerSettingType(
               Arg.Any<ISettingContext>(),
               Arg.Any<IEnumerable<int>>(),
               Arg.Any<Level>(),
               Arg.Any<Level>(),
               Arg.Any<byte>(),
               Arg.Any<SettingType>()).Returns(results);

            _settingTypeServiceFactory.GetInstance(Arg.Any<SettingType>()).Returns(_settingTypeService);
        }

        [Test]
        public void UpdateMultipleCustomerSetting_Always_CallResolveSettingTypeService()
        {
            //// Arrange
            MultipleUpdateCustomerSettingCommand updateMultipleSettingParam = InstanceFactory.CreateUpdateMultipleSettingParam();
            AssumeUpdateMultipleCustomersSettingActionIsValid(updateMultipleSettingParam);
            SetupSettingTypeServiceStub();

            //// Act
            _settingService.Update(updateMultipleSettingParam);

            //// Assert
            var numOfSettingTypes = updateMultipleSettingParam.Setting.SettingTypes.Count();
            _settingTypeServiceFactory.Received(numOfSettingTypes).GetInstance(Arg.Any<SettingType>());
        }

        [Test]
        public void UpdateMultipleCustomerSetting_Always_CallUpdateMultipleCustomersSettingType()
        {
            //// Arrange
            var updateMultipleSettingParam = InstanceFactory.CreateUpdateMultipleSettingParam();
            AssumeUpdateMultipleCustomersSettingActionIsValid(updateMultipleSettingParam);
            SetupSettingTypeServiceStub();

            //// Act
            _settingService.Update(updateMultipleSettingParam);

            //// Assert
            var numOfRunPostTasks = updateMultipleSettingParam.Setting.SettingTypes.Count();
            _settingTypeService.Received(numOfRunPostTasks)
                .UpdateMultipleCustomerSettingType(
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
            MultipleUpdateCustomerSettingCommand updateMultipleSettingParam = InstanceFactory.CreateUpdateMultipleSettingParam();
            AssumeUpdateMultipleCustomersSettingActionIsValid(updateMultipleSettingParam);
            SetupSettingTypeServiceStub(false);

            //// Act
            var actualResult = _settingService.Update(updateMultipleSettingParam);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [Test]
        public void UpdateMultipleCustomerSetting_Updated_ReturnSuccessResult()
        {
            //// Arrange
            MultipleUpdateCustomerSettingCommand updateMultipleSettingParam = InstanceFactory.CreateUpdateMultipleSettingParam();
            AssumeUpdateMultipleCustomersSettingActionIsValid(updateMultipleSettingParam);
            SetupSettingTypeServiceStub();

            //// Act
            var actualResult = _settingService.Update(updateMultipleSettingParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [Test]
        public void UpdateMultipleCustomerSetting_UpdatedSettingTypes_ReturnResultWithCustomerName()
        {
            //// Arrange
            MultipleUpdateCustomerSettingCommand updateMultipleSettingParam = InstanceFactory.CreateUpdateMultipleSettingParam();
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

            _settingTypeService.UpdateMultipleCustomerSettingType(
               Arg.Any<ISettingContext>(),
               Arg.Any<IEnumerable<int>>(),
               Arg.Any<Level>(),
               Arg.Any<Level>(),
               Arg.Any<byte>(),
               Arg.Any<SettingType>()).Returns(results);

            _settingTypeServiceFactory.GetInstance(Arg.Any<SettingType>()).Returns(_settingTypeService);

            //// Act
            var actualResult = _settingService.Update(updateMultipleSettingParam);

            //// Assert
            var updateSettingResult = actualResult as UpdateSettingResult;
            Assert.IsTrue(
                updateSettingResult.SubResults.Any(settingResult => settingResult.CustomerName == "Username1")
                && updateSettingResult.SubResults.Any(settingResult => settingResult.CustomerName == "Username2"));
        }

        [Test]
        public void UpdateMultipleCustomerSetting_RaisedException_AlwaysCallLogError()
        {
            //// Arrange
            MultipleUpdateCustomerSettingCommand updateMultipleSettingParam = InstanceFactory.CreateUpdateMultipleSettingParam();

            AssumeUpdateMultipleCustomersSettingActionIsValid(updateMultipleSettingParam);

            var expectedException = new DllNotFoundException("Dll not found");
            _settingTypeServiceFactory.GetInstance(Arg.Any<SettingType>()).Returns(x => { throw expectedException; });

            //// Act
            _settingService.Update(updateMultipleSettingParam);

            //// Assert
            _logger.Received().Error(
                Arg.Is<string>(message => message.Contains(expectedException.Message)),
                Arg.Any<CustomerSettingException>());
        }
    }
}