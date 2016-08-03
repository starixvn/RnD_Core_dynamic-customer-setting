namespace Fanex.CustomerSetting.NumberGames2.Tests.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Services;
    using Exceptions;
    using Logging;
    using Models;
    using NSubstitute;
    using NumberGames2.Services;
    using NumberGames2.Services.Impl;
    using NUnit.Framework;

    public class UpdateNumberGames2SettingServiceTests
    {
        private ISettingContext _settingContext;
        private IValidationService _validationService;
        private ITaskService _pluggableTaskService;
        private ILogger _logger;
        private ISettingTypeServiceFactory _settingTypeServiceFactory;
        private ICustomerSyncStatusService _customerSyncStatusService;
        private IUpdateSettingService _updateSettingService;
        private List<ISettingTypeService> _settingTypeServices;

        [SetUp]
        public void SetUp()
        {
            _settingContext = Substitute.For<ISettingContext>();
            _validationService = Substitute.For<IValidationService>();
            _pluggableTaskService = Substitute.For<ITaskService>();
            _logger = Substitute.For<ILogger>();
            _settingTypeServiceFactory = Substitute.For<ISettingTypeServiceFactory>();
            _customerSyncStatusService = Substitute.For<ICustomerSyncStatusService>();
            _settingTypeServices = new List<ISettingTypeService>();

            _settingContext.Actor.Returns(new User
            {
                Id = 100,
                Level = Level.Company,
                Name = "Admin"
            });
            _settingContext.IP.Returns("192.168.1.1");
            _settingContext.Domain.Returns("Domain Name");

            foreach (short settingTypeId in StubHelpers.SettingTypes)
            {
                var settingTypeService = Substitute.For<ISettingTypeService>();

                _settingTypeServices.Add(settingTypeService);
                _settingTypeServiceFactory
                    .GetInstance(Arg.Is<SettingType>(settingType => settingType.Id == settingTypeId))
                    .Returns(settingTypeService);
            }

            _updateSettingService = new UpdateNumberGames2SettingService(
                _settingContext,
                _validationService,
                _pluggableTaskService,
                 _logger,
                 _settingTypeServiceFactory,
                 _customerSyncStatusService);
        }

        [Test]
        public void Update_CommandIsNull_ThrowException()
        {
            // Arrange.
            UpdateCustomerSettingCommand command = null;

            // Act and Assert.
            var ex = Assert.Throws<ArgumentNullException>(() => _updateSettingService.Update(command));

            Assert.AreEqual("command", ex.ParamName);
        }

        [Test]
        public void Update_SettingTypesInCommandIsEmpty_ThrowException()
        {
            // Arrange.
            UpdateCustomerSettingCommand command = StubHelpers.CreateUpdateSettingCommand();

            command.Setting.SettingTypes = Enumerable.Empty<SettingType>();

            // Act and Assert.
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => _updateSettingService.Update(command));

            Assert.AreEqual("command.Setting.SettingTypes.Count", ex.ParamName);
        }

        [Test]
        public void Update_UplineIsUnsynced_ThrowException()
        {
            // Arrange.
            int customerId = 1;
            byte productId = 1;
            UpdateCustomerSettingCommand command = StubHelpers.CreateUpdateSettingCommand(
                customerId: customerId,
                productId: productId);

            _customerSyncStatusService
                .GetCustomerSyncStatus(Arg.Is(customerId), Arg.Is(productId))
                .Returns(new CustomerSyncStatus
                {
                    IsSynced = true,
                    IsUplineSynced = false
                });

            // Act and Assert.
            var ex = Assert.Throws<CustomerSettingException>(() => _updateSettingService.Update(command));

            StringAssert.Contains(
                "Product 1 has not been offered for Upline of Customer 1.",
                ex.Message);
        }

        [Test]
        public void Update_SyncedCustomer_RunNormalFlow()
        {
            // Arrange.
            int customerId = 1;
            byte productId = 1;
            UpdateCustomerSettingCommand command = StubHelpers.CreateUpdateSettingCommand(
                customerId: customerId,
                productId: productId);

            _customerSyncStatusService
                .GetCustomerSyncStatus(Arg.Is(customerId), Arg.Is(productId))
                .Returns(new CustomerSyncStatus
                {
                    IsSynced = true,
                    IsUplineSynced = true
                });

            // Act.
            _updateSettingService.Update(command);

            // Assert.
            Received.InOrder(() =>
            {
                foreach (ISettingTypeService settingTypeService in _settingTypeServices)
                {
                    settingTypeService.UpdateCustomerSettingType(
                        Arg.Is(_settingContext),
                        Arg.Is(customerId),
                        Arg.Is(command.Setting.CustomerLevel),
                        Arg.Is(command.UplineLevel),
                        Arg.Is(productId),
                        Arg.Any<SettingType>());
                }
            });
        }

        /*[Test]
        public void Update_SyncedMemberAndFirstTimeUserGroupEdit_CallApiToSyncUserGroup()
        {
        }

        [Test]
        public void Update_SyncedMemberAndFirstTimeUserGroupEdit_SyncUserGroupBeforeUpdatingSettings()
        {
        }

        [Test]
        public void Update_UnsyncedCustomer_CallApiToSyncCustomerInfo()
        {
        }

        [Test]
        public void Update_SyncedCustomerInfoSuccessfully_UpdateProductStatusIfAny()
        {
        }

        [Test]
        public void Update_FailedToSyncCustomerInfoAndProductStatusNeedsToBeUpdated_NotUpdateProductStatus()
        {
        }

        [Test]
        public void Update_FailedToSyncCustomerInfoAndProductStatusNeedsToBeUpdated_ReturnError()
        {
        }

        [Test]
        public void Update_UnsyncedMemberAndFirstTimeUserGroupEdit_CallApiToSyncUserGroup()
        {
        }

        [Test]
        public void Update_UnsyncedMemberAndFirstTimeUserGroupEdit_SyncUserGroupBeforeUpdatingSettings()
        {
        }

        [Test]
        public void Update_UnsyncedCustomer_UpdateSettingsExceptProductStatus()
        {
        } /**/
    }
}