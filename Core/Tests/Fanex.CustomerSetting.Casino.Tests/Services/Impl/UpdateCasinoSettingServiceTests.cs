namespace Fanex.CustomerSetting.Casino.Tests.Services.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using Casino.Models;
    using Casino.Services;
    using Casino.Services.Impl;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using CustomerSetting.Services;
    using Logging;
    using Models;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class UpdateCasinoSettingServiceTests
    {
        private const int StatusSettingTypeValue = 4;
        private IUpdateCasinoSettingService _updateCasinoSettingService;
        private ISettingContext _settingContext;
        private IUpdateSettingService _updateSettingService;
        private IValidationService _validationService;
        private ILogger _logger;
        private ISettingTypeServiceFactory _settingTypeServiceFactory;
        private ICustomerProductStatusService _customerProductStatusService;

        [SetUp]
        public void Setup()
        {
            _settingContext = new SettingContextStub();
            _validationService = Substitute.For<IValidationService>();
            _updateSettingService = Substitute.For<IUpdateSettingService>();
            _logger = Substitute.For<ILogger>();
            _settingTypeServiceFactory = Substitute.For<ISettingTypeServiceFactory>();
            _customerProductStatusService = Substitute.For<ICustomerProductStatusService>();

            _updateCasinoSettingService = new UpdateCasinoSettingService(
                _settingContext,
                _updateSettingService,
                _validationService,
                _customerProductStatusService,
                 _settingTypeServiceFactory,
                 _logger);
        }

        private void ArrangeCustomerCasinoStatus(UpdateCustomerSettingCommand command, CustomerProductStatus status)
        {
            _customerProductStatusService
             .GetCustomerStatus(
                 command.Setting.CustomerId,
                 command.Setting.ProductId)
             .ReturnsForAnyArgs(status);
        }

        [Test]
        public void Update_CustomerCasinoStatusAndAllDownlineAreSynced_RunNormalFlow()
        {
            //// Arrange
            var command = StubHelper.CreateUpdateSettingParam();
            var goodStatus = new CustomerProductStatus { IsSynced = true, IsDownlineSynced = true, IsHidden = false };
            ArrangeCustomerCasinoStatus(command, goodStatus);

            //// Act
            _updateCasinoSettingService.Update(command);

            //// Assert
            _updateSettingService.Received().Update(command);
        }

        [TestCase(false, true)]
        [TestCase(true, false)]
        [TestCase(false, false)]
        public void Update_UnsyncedCustomerSetting_UpdateAllSettingTypesSuccessfully_CallUpdateCustomerProductStatusSynced(bool isSynced, bool isDownlineSynced)
        {
            //// Arrange
            var settingTypeValues = new byte[] { StatusSettingTypeValue, 1, 5, 3 };
            var command = StubHelper.CreateUpdateSettingParam(settingTypeValues: settingTypeValues);
            var unsyncedStatus = new CustomerProductStatus { IsSynced = isSynced, IsDownlineSynced = isDownlineSynced, IsHidden = false };
            ArrangeCustomerCasinoStatus(command, unsyncedStatus);
            var services = new List<ISettingTypeService>();
            command.Setting.SettingTypes.ToList().ForEach(settingType =>
            {
                var settingTypeService = Substitute.For<ISettingTypeService>();
                var sucessResult = new SettingResult(Result.Success);

                settingTypeService.UpdateCustomerSettingType(
                    _settingContext,
                    command.Setting.CustomerId,
                    command.Setting.CustomerLevel,
                    command.UplineLevel,
                    command.Setting.ProductId,
                    settingType)
                    .ReturnsForAnyArgs(sucessResult);

                _settingTypeServiceFactory
                    .GetInstance(settingType)
                    .Returns(settingTypeService);

                services.Add(settingTypeService);
            });

            //// Act
            _updateCasinoSettingService.Update(command);

            //// Assert
            _customerProductStatusService.Received().UpdateCustomerStatus(
                command.Setting.CustomerId,
                command.Setting.ProductId,
                _settingContext.Actor.SubAccountId,
                _settingContext.Actor.Id,
                _settingContext.IP);
        }

        [TestCase(false, true)]
        [TestCase(true, false)]
        [TestCase(false, false)]
        public void Update_UnsyncedCustomerSetting_AFailedResultExists_DoNotCallUpdateStatus(bool isSynced, bool isDownlineSynced)
        {
            //// Arrange
            var settingTypeValues = new byte[] { StatusSettingTypeValue, 2, 5, 3 };
            var command = StubHelper.CreateUpdateSettingParam(settingTypeValues: settingTypeValues);
            var unsyncedStatus = new CustomerProductStatus { IsSynced = isSynced, IsDownlineSynced = isDownlineSynced, IsHidden = false };
            ArrangeCustomerCasinoStatus(command, unsyncedStatus);
            var services = new List<ISettingTypeService>();
            command.Setting.SettingTypes.ToList().ForEach(settingType =>
            {
                var settingTypeService = Substitute.For<ISettingTypeService>();

                var sucessResult = new SettingResult(Result.SystemError);

                settingTypeService.UpdateCustomerSettingType(
                    _settingContext,
                    command.Setting.CustomerId,
                    command.Setting.CustomerLevel,
                    command.UplineLevel,
                    command.Setting.ProductId,
                    settingType)
                    .ReturnsForAnyArgs(sucessResult);

                _settingTypeServiceFactory
                    .GetInstance(settingType)
                    .Returns(settingTypeService);

                services.Add(settingTypeService);
            });

            //// Act
            _updateCasinoSettingService.Update(command);

            //// Assert
            services[0]
                .DidNotReceiveWithAnyArgs()
                .UpdateCustomerSettingType(
                            _settingContext,
                            command.Setting.CustomerId,
                            command.Setting.CustomerLevel,
                            command.UplineLevel,
                            command.Setting.ProductId,
                            Arg.Any<SettingType>());
        }

        [TestCase(false, true)]
        [TestCase(true, false)]
        [TestCase(false, false)]
        public void Update_UnsyncedCustomerSetting_AllResultsAreSuccessfull_CallUpdateStatus(bool isSynced, bool isDownlineSynced)
        {
            //// Arrange
            var settingTypeValues = new byte[] { StatusSettingTypeValue, 1, 5, 3 };
            var command = StubHelper.CreateUpdateSettingParam(settingTypeValues: settingTypeValues);
            var unsyncedStatus = new CustomerProductStatus { IsSynced = isSynced, IsDownlineSynced = isDownlineSynced, IsHidden = false };
            ArrangeCustomerCasinoStatus(command, unsyncedStatus);
            var services = new List<ISettingTypeService>();
            command.Setting.SettingTypes.ToList().ForEach(settingType =>
            {
                var settingTypeService = Substitute.For<ISettingTypeService>();
                var sucessResult = new SettingResult(Result.Success);

                settingTypeService.UpdateCustomerSettingType(
                    _settingContext,
                    command.Setting.CustomerId,
                    command.Setting.CustomerLevel,
                    command.UplineLevel,
                    command.Setting.ProductId,
                    settingType)
                    .ReturnsForAnyArgs(sucessResult);

                _settingTypeServiceFactory
                    .GetInstance(settingType)
                    .Returns(settingTypeService);

                services.Add(settingTypeService);
            });

            //// Act
            _updateCasinoSettingService.Update(command);

            //// Assert
            services[0]
                .Received()
                .UpdateCustomerSettingType(
                            _settingContext,
                            command.Setting.CustomerId,
                            command.Setting.CustomerLevel,
                            command.UplineLevel,
                            command.Setting.ProductId,
                            command.Setting
                        .SettingTypes
                        .FirstOrDefault(s => s.Id == StatusSettingTypeValue));
        }
    }
}