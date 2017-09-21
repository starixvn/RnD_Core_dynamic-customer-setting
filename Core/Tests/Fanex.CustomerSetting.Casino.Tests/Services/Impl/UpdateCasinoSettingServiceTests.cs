namespace Fanex.CustomerSetting.Casino.Tests.Services.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using Casino.Services;
    using Casino.Services.Impl;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
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
        private const int StatusSettingTypeValue = 13;
        private readonly byte[] _settingTypeIds = { StatusSettingTypeValue, 1, 5, 3 };

        private IUpdateCasinoSettingService _updateCasinoSettingService;
        private ISettingContext _settingContext;
        private ISettingTypeServiceFactory _settingTypeServiceFactory;
        private ICustomerProductSyncedStatusService _customerProductStatusService;

        [SetUp]
        public void Setup()
        {
            _settingContext = new SettingContextStub();
            _settingTypeServiceFactory = Substitute.For<ISettingTypeServiceFactory>();
            _customerProductStatusService = Substitute.For<ICustomerProductSyncedStatusService>();

            _updateCasinoSettingService = new UpdateCasinoSettingService(
                _settingContext,
                Substitute.For<IValidationService>(),
                Substitute.For<ITaskService>(),
                Substitute.For<ILogger>(),
                _settingTypeServiceFactory,
                _customerProductStatusService);
        }

        private void ArrangeCustomerCasinoStatus(UpdateCustomerSettingCommand command, bool syncedStatus)
        {
            _customerProductStatusService
                .CheckCustomerProductSyncedStatus(command.Setting.CustomerId, command.Setting.ProductId)
                .ReturnsForAnyArgs(syncedStatus);
        }

        [TestCase((byte)SystemType.Nap)]
        [TestCase((byte)SystemType.InternalAlpha)]
        [TestCase((byte)SystemType.Alpha)]
        [TestCase((byte)SystemType.ExternalNap)]
        public void Update_CustomerIsSynced_RunNormalFlow(byte systemId)
        {
            // Arrange.
            var services = new List<ISettingTypeService>();
            var command = PrepareCommand(services, Result.Success);

            ArrangeCustomerCasinoStatus(command, true);
            _settingContext.System = systemId;

            // Act.
            var result = _updateCasinoSettingService.Update(command) as UpdateSettingResult;

            // Assert.
            byte[] orderOfUpdate = result.SubResults.Select(subResult => (byte)subResult.SettingTypeId).ToArray();

            Assert.AreEqual(_settingTypeIds, orderOfUpdate);
        }

        [TestCase((byte)SystemType.Nap)]
        [TestCase((byte)SystemType.InternalAlpha)]
        [TestCase((byte)SystemType.Alpha)]
        [TestCase((byte)SystemType.ExternalNap)]
        public void Update_CustomerIsSynced_DoNotCallUpdateCustomerProductSyncedStatus(byte systemId)
        {
            // Arrange.
            var services = new List<ISettingTypeService>();
            var command = PrepareCommand(services, Result.Success);

            ArrangeCustomerCasinoStatus(command, true);
            _settingContext.System = systemId;

            // Act.
            _updateCasinoSettingService.Update(command);

            // Assert.
            _customerProductStatusService
                .DidNotReceiveWithAnyArgs()
                .UpdateCustomerProductSyncedStatus(
                    command.Setting.CustomerId,
                    command.Setting.ProductId,
                    _settingContext.Actor.Id,
                    _settingContext.Actor.SubAccountId,
                    _settingContext.IP);
        }

        [Test]
        public void Update_CustomerIsUnsyncedAndUpdateOtherSettingTypesSuccessfully_CallUpdateStatus()
        {
            // Arrange.
            var services = new List<ISettingTypeService>();
            var command = PrepareCommand(services, Result.Success);

            ArrangeCustomerCasinoStatus(command, false);

            // Act.
            _updateCasinoSettingService.Update(command);

            // Assert.
            services[0]
                .Received()
                .UpdateCustomerSettingType(
                    _settingContext,
                    command.Setting.CustomerId,
                    command.Setting.CustomerLevel,
                    command.UplineLevel,
                    command.Setting.ProductId,
                    command.Setting.SettingTypes.FirstOrDefault(s => s.Id == StatusSettingTypeValue));
        }

        [TestCase((byte)SystemType.Nap)]
        [TestCase((byte)SystemType.InternalAlpha)]
        [TestCase((byte)SystemType.Alpha)]
        [TestCase((byte)SystemType.ExternalNap)]
        public void Update_CustomerIsUnsyncedAndUpdateAllSettingTypesSuccessfully_CallUpdateCustomerProductSyncedStatus(byte systemId)
        {
            // Arrange.
            var services = new List<ISettingTypeService>();
            var command = PrepareCommand(services, Result.Success);

            ArrangeCustomerCasinoStatus(command, false);
            _settingContext.System = systemId;

            // Act.
            _updateCasinoSettingService.Update(command);

            // Assert.
            _customerProductStatusService
                .Received()
                .UpdateCustomerProductSyncedStatus(
                    command.Setting.CustomerId,
                    command.Setting.ProductId,
                    _settingContext.Actor.Id,
                    _settingContext.Actor.SubAccountId,
                    _settingContext.IP);
        }

        [Test]
        public void UpdateInExternalNapSetSettingFuction_CustomerIsUnsynced_DoNotCallUpdateCustomerProductStatusSynced()
        {
            // Arrange.
            var services = new List<ISettingTypeService>();
            var command = PrepareCommand(services, Result.Success);

            ArrangeCustomerCasinoStatus(command, false);
            _settingContext.System = (byte)SystemType.ExternalNapBetSetting;

            // Act.
            _updateCasinoSettingService.Update(command);

            // Assert.
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

        [Test]
        public void Update_CustomerIsUnsyncedAndAFailedResultExists_DoNotCallUpdateStatus()
        {
            // Arrange.
            var services = new List<ISettingTypeService>();
            var command = PrepareCommand(services, Result.SystemError);

            ArrangeCustomerCasinoStatus(command, false);

            // Act.
            _updateCasinoSettingService.Update(command);

            // Assert.
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

        private UpdateCustomerSettingCommand PrepareCommand(List<ISettingTypeService> registeredSettingTypeServices, IResult expectedResult)
        {
            var command = StubHelper.CreateUpdateSettingParam(settingTypeValues: _settingTypeIds);

            command.Setting.SettingTypes.ToList().ForEach(settingType =>
            {
                var settingTypeService = Substitute.For<ISettingTypeService>();
                var subResult = new SettingResult(expectedResult, command.Setting.CustomerId, settingType.Id, settingType.Name);

                settingTypeService
                    .UpdateCustomerSettingType(
                        _settingContext,
                        command.Setting.CustomerId,
                        command.Setting.CustomerLevel,
                        command.UplineLevel,
                        command.Setting.ProductId,
                        settingType)
                    .ReturnsForAnyArgs(subResult);

                _settingTypeServiceFactory
                    .GetInstance(settingType)
                    .Returns(settingTypeService);

                registeredSettingTypeServices.Add(settingTypeService);
            });

            return command;
        }
    }
}