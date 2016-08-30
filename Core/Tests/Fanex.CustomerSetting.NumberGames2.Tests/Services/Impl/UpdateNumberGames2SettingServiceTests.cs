namespace Fanex.CustomerSetting.NumberGames2.Tests.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using CustomerSetting.Services;
    using Exceptions;
    using Logging;
    using Models;
    using NSubstitute;
    using NumberGames2.Services;
    using NumberGames2.Services.Impl;
    using NUnit.Framework;
    using ONEbook.NG2.API.Interface;
    using Resources;
    using ApiModels = ONEbook.NG2.API.DTO;

    public class UpdateNumberGames2SettingServiceTests
    {
        private const byte ProductStatusSettingTypeId = 11;

        private ISettingContext _settingContext;
        private IValidationService _validationService;
        private ITaskService _pluggableTaskService;
        private ILogger _logger;
        private ISettingTypeServiceFactory _settingTypeServiceFactory;
        private ICustomerSyncStatusService _customerSyncStatusService;
        private IUpdateSettingService _updateSettingService;
        private IThirdPartyServices _apiService;
        private Dictionary<short, ISettingTypeService> _settingTypeServices;

        [SetUp]
        public void SetUp()
        {
            var actor = new User
            {
                Id = 100,
                Level = Level.Company,
                Name = "Admin"
            };

            _settingContext = Substitute.For<ISettingContext>();
            _validationService = Substitute.For<IValidationService>();
            _pluggableTaskService = Substitute.For<ITaskService>();
            _logger = Substitute.For<ILogger>();
            _settingTypeServiceFactory = Substitute.For<ISettingTypeServiceFactory>();
            _customerSyncStatusService = Substitute.For<ICustomerSyncStatusService>();
            _apiService = Substitute.For<IThirdPartyServices>();
            _settingTypeServices = new Dictionary<short, ISettingTypeService>();

            _settingContext.Actor.Returns(actor);
            _settingContext.IP.Returns("192.168.1.1");
            _settingContext.Domain.Returns("Domain Name");

            foreach (short settingTypeId in StubHelpers.SettingTypes)
            {
                var settingTypeService = Substitute.For<ISettingTypeService>();

                _settingTypeServices.Add(settingTypeId, settingTypeService);
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
                 _customerSyncStatusService,
                 _apiService);
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

            FakeSyncStatusServiceCall(customerId, productId, true, false);

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

            FakeSyncStatusServiceCall(customerId, productId, true, true);

            // Act.
            _updateSettingService.Update(command);

            // Assert.
            Received.InOrder(() =>
            {
                foreach (KeyValuePair<short, ISettingTypeService> settingTypeService in _settingTypeServices)
                {
                    settingTypeService.Value.UpdateCustomerSettingType(
                        Arg.Is(_settingContext),
                        Arg.Is(customerId),
                        Arg.Is(command.Setting.CustomerLevel),
                        Arg.Is(command.UplineLevel),
                        Arg.Is(productId),
                        Arg.Any<SettingType>());
                }
            });
        }

        [Test]
        public void Update_UnsyncedCustomer_CallApiToSyncCustomerInfo()
        {
            // Arrange.
            int customerId = 1;
            Level customerLevel = Level.Super;
            byte productId = 1;
            UpdateCustomerSettingCommand command = StubHelpers.CreateUpdateSettingCommand(
                customerId,
                customerLevel,
                productId);

            FakeSyncStatusServiceCall(customerId, productId);

            // Act.
            _updateSettingService.Update(command);

            // Assert.
            _apiService.Received().SyncCustomers(Arg.Is<ApiModels.Request>(request =>
                request.Call.CustIds.Contains(customerId)
                && request.Call.RoleId == customerLevel.Value));
        }

        [Test]
        public void Update_SyncedCustomerInfoSuccessfully_UpdateProductStatusIfAny()
        {
            // Arrange.
            int customerId = 1;
            Level customerLevel = Level.Super;
            byte productId = 1;
            UpdateCustomerSettingCommand command = StubHelpers.CreateUpdateSettingCommand(
                customerId: customerId,
                customerLevel: customerLevel,
                productId: productId,
                settingTypes: new byte[] { ProductStatusSettingTypeId });
            ISettingTypeService productSettingTypeService = _settingTypeServices[ProductStatusSettingTypeId];

            FakeApiCall(customerId, customerLevel);
            FakeSyncStatusServiceCall(customerId, productId);

            // Act.
            _updateSettingService.Update(command);

            // Assert.
            productSettingTypeService.Received().UpdateCustomerSettingType(
                Arg.Is(_settingContext),
                Arg.Is(customerId),
                Arg.Is(customerLevel),
                Arg.Is<Level>(uplineLevel => uplineLevel == null),
                Arg.Is(productId),
                Arg.Is<SettingType>(settingType => settingType.Id == ProductStatusSettingTypeId));
        }

        [Test]
        public void Update_FailedToSyncCustomerInfoAndProductStatusNeedsToBeUpdated_NotUpdateProductStatus()
        {
            // Arrange.
            int customerId = 1;
            Level customerLevel = Level.Super;
            byte productId = 1;
            UpdateCustomerSettingCommand command = StubHelpers.CreateUpdateSettingCommand(
                customerId: customerId,
                customerLevel: customerLevel,
                productId: productId,
                settingTypes: new byte[] { ProductStatusSettingTypeId });
            ISettingTypeService productSettingTypeService = _settingTypeServices[ProductStatusSettingTypeId];

            FakeApiCall(customerId, customerLevel, false);
            FakeSyncStatusServiceCall(customerId, productId);

            // Act.
            _updateSettingService.Update(command);

            // Assert.
            productSettingTypeService.DidNotReceive();
        }

        [Test]
        public void Update_FailedToSyncCustomerInfoAndProductStatusNeedsToBeUpdated_ReturnError()
        {
            // Arrange.
            int customerId = 1;
            Level customerLevel = Level.Super;
            byte productId = 1;
            UpdateCustomerSettingCommand command = StubHelpers.CreateUpdateSettingCommand(
                customerId: customerId,
                customerLevel: customerLevel,
                productId: productId,
                settingTypes: new byte[] { ProductStatusSettingTypeId });

            FakeApiCall(customerId, customerLevel, false);
            FakeSyncStatusServiceCall(customerId, productId);

            // Act.
            var result = _updateSettingService.Update(command) as UpdateSettingResult;

            // Assert.
            Assert.True(result.IsFailed);
        }

        [Test]
        public void Update_FailedToSyncCustomerInfoAndProductStatusNeedsToBeUpdated_ReturnErrorMessage()
        {
            // Arrange.
            int customerId = 1;
            Level customerLevel = Level.Super;
            byte productId = 1;
            UpdateCustomerSettingCommand command = StubHelpers.CreateUpdateSettingCommand(
                customerId: customerId,
                customerLevel: customerLevel,
                productId: productId,
                settingTypes: new byte[] { ProductStatusSettingTypeId });

            FakeApiCall(customerId, customerLevel, false);
            FakeSyncStatusServiceCall(customerId, productId);

            // Act.
            var result = _updateSettingService.Update(command) as UpdateSettingResult;

            // Assert.
            Assert.That(result.SubResults.Any(subResult => subResult.Message == Messages.UnableToUpdateProductStatus));
        }

        private void FakeSyncStatusServiceCall(
            int customerId,
            byte productId,
            bool isSynced = false,
            bool isSyncedUpline = true)
        {
            _customerSyncStatusService
                .GetCustomerSyncStatus(Arg.Is(customerId), Arg.Is(productId))
                .Returns(new CustomerSyncStatus
                {
                    IsSynced = isSynced,
                    IsSyncedUpline = isSyncedUpline
                });
        }

        private void FakeApiCall(int customerId, Level customerLevel, bool isSuccessful = true)
        {
            var apiResultDetail = Substitute.For<ApiModels.IResultDetail>();
            var apiResponse = Substitute.For<ApiModels.IResponse>();

            apiResultDetail.IsSuccess.Returns(isSuccessful);
            apiResponse.Result = Substitute.For<ApiModels.IResult>();
            apiResponse.Result.ResultDetail = new List<ApiModels.IResultDetail>
            {
                apiResultDetail
            };

            _apiService
                .SyncCustomers(Arg.Is<ApiModels.Request>(request =>
                    request.Call.CustIds.Contains(customerId)
                    && request.Call.RoleId == customerLevel.Value))
                .Returns(apiResponse);
        }
    }
}