namespace Fanex.CustomerSetting.Casino.Tests.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Casino.Models;
    using Casino.Services;
    using Casino.Services.Impl;
    using Criteria;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using CustomerSetting.Services;
    using Data.Repository;
    using Logging;
    using Models;
    using NSubstitute;
    using NUnit.Framework;


    [TestFixture]
    public class UpdateCasinoSettingServiceTests
    {
        private const int statusSettingTypeValue = 1;
        private IUpdateCasinoSettingService _updateCasinoSettingService;
        private ISettingContext _settingContext;
        private IUpdateSettingService _updateSettingService;
        private IValidationService _validationService;
        private IProductService _productService;
        private ITaskService _taskService;
        private ILogger _logger;
        private ISettingTypeServiceFactory _settingTypeServiceFactory;
        private ISettingTypeService _settingTypeService;
        private IDynamicRepository _dymanicRepository;

        [SetUp]
        public void Setup()
        {
            _settingContext = new SettingContextStub();
            _validationService = Substitute.For<IValidationService>();
            _updateSettingService = Substitute.For<IUpdateSettingService>();
            _taskService = Substitute.For<ITaskService>();
            _logger = Substitute.For<ILogger>();
            _settingTypeServiceFactory = Substitute.For<ISettingTypeServiceFactory>();
            _settingTypeService = Substitute.For<ISettingTypeService>();
            _dymanicRepository = Substitute.For<IDynamicRepository>();
            _productService = Substitute.For<IProductService>();

            _updateCasinoSettingService = new UpdateCasinoSettingService(
                _settingContext,
                _updateSettingService,
                _validationService,
                _dymanicRepository,
                 _settingTypeServiceFactory,
                 _logger);
        }

        private void ArrangeCustomerCasinoStatus(UpdateCustomerSettingCommand command, CustomerCasinoStatus status)
        {
            _dymanicRepository
             .Get<CustomerCasinoStatus>(new GetCustomerCasinoStatusCriteria
             {
                 ProductId = command.Setting.ProductId,
                 CustId = command.Setting.CustomerId
             })
             .ReturnsForAnyArgs(status);
        }

        [Test]
        public void Update_CustomerCasinoStatusIsNull_ThrowInvalidOperationException()
        {
            //// Arrange
            var command = StubHelper.CreateUpdateSettingParam();
            ArrangeCustomerCasinoStatus(command, status: null);
           
            //// Act & Assert
            Assert.Throws<InvalidOperationException>(
               () => _updateCasinoSettingService.Update(command));
        }

        [Test]
        public void Update_CustomerCasinoStatusIsHidden_ThrowInvalidOperationException()
        {
            //// Arrange
            var command = StubHelper.CreateUpdateSettingParam();
            var hiddenStatus = new CustomerCasinoStatus { IsHidden = true };
            ArrangeCustomerCasinoStatus(command, hiddenStatus);

            //// Act & Assert
            Assert.Throws<InvalidOperationException>(
               () => _updateCasinoSettingService.Update(command));
        }

        [Test]
        public void Update_CustomerCasinoStatusAndAllDownlineAreSynced_RunNormalFlow()
        {
            //// Arrange
            var command = StubHelper.CreateUpdateSettingParam();
            var goodStatus = new CustomerCasinoStatus { IsSynced = true, IsDownlineSynced = true, IsHidden = false};
            ArrangeCustomerCasinoStatus(command, goodStatus);

            //// Act
            _updateCasinoSettingService.Update(command);

            //// Assert
            _updateSettingService.Received().Update(command);
        }

        [Test]
        public void Update_UpdateAllSettingTypesSuccessfully_UpdateSyncStatusOfCustomerToTrue()
        {
            Assert.Inconclusive();
        }

        [TestCase(false, true)]
        [TestCase(true, false)]
        [TestCase(false, false)]
        public void Update_UnsyncedCustomerSetting_AFailedResultExists_DoNotCallUpdateStatus(bool isSynced, bool isDownlineSynced)
        {
            //// Arrange
            var settingTypeValues = new byte[] { statusSettingTypeValue, 2, 5, 3 };
            var command = StubHelper.CreateUpdateSettingParam(settingTypeValues: settingTypeValues);
            var unsyncedStatus = new CustomerCasinoStatus { IsSynced = isSynced, IsDownlineSynced = isDownlineSynced, IsHidden = false };
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
                .UpdateCustomerSettingType(_settingContext,
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
            var settingTypeValues = new byte[] { statusSettingTypeValue, 4, 5, 3 };
            var command = StubHelper.CreateUpdateSettingParam(settingTypeValues: settingTypeValues);
            var unsyncedStatus = new CustomerCasinoStatus { IsSynced = isSynced, IsDownlineSynced = isDownlineSynced, IsHidden = false };
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
                .UpdateCustomerSettingType(_settingContext,
                            command.Setting.CustomerId,
                            command.Setting.CustomerLevel,
                            command.UplineLevel,
                            command.Setting.ProductId,
                            command.Setting
                        .SettingTypes
                        .FirstOrDefault(s => s.Id == statusSettingTypeValue));

        }

        //[Test]
        //public void UpdateSettingTypesForMultipleCustomers_Always_CallUpdateSingleCustomerOneByOne()
        //{
        //    //// Anders questions:
        //    //// Update setting type {status, A, B} for 100 customers
        //    //// Update successfully A for 90 customers, B for 85 customers
        //    //// Will we update customers in intersaction of A & B? or reject all or what?
        //    //// Arrange
        //    const int statusSettingTypeValue = 1;
        //    var customerIds = new List<int> { 1, 100, 1000, 1001 };
        //    var settingTypeValues = new byte[] { statusSettingTypeValue, 2, 5, 3 };
        //    var command = StubHelper.CreateUpdateMultipleSettingParam(
        //        customerIds: customerIds,
        //        customerNames: new List<string> { "username1", "100", "1000", "1001" },
        //        settingTypeValues: settingTypeValues);
        //    var services = new List<ISettingTypeService>();

        //    command.Setting.SettingTypes.ToList().ForEach(settingType =>
        //    {
        //        var settingTypeService = Substitute.For<ISettingTypeService>();
        //        var sucessResults = new List<SettingResult>();

        //        settingTypeService.UpdateMultipleCustomerSettingType(
        //            _settingContext,
        //            command.CustomerIds,
        //            command.Setting.CustomerLevel,
        //            command.UplineLevel,
        //            command.Setting.ProductId,
        //            settingType)
        //           .ReturnsForAnyArgs(sucessResults);

        //        _settingTypeServiceFactory
        //            .GetInstance(settingType)
        //            .Returns(settingTypeService);

        //        services.Add(settingTypeService);
        //    });

        //    _settingService = new UpdateCasinoSettingService(
        //        _settingContext,
        //        _validationService,
        //        _taskService,
        //        _logger,
        //        _settingTypeServiceFactory);

        //    //// Act
        //    _settingService.Update(command);

        //    //// Assert
        //    _settingService.ReceivedWithAnyArgs(customerIds.Count).Update(Arg.Any<UpdateCustomerSettingCommand>());
        //}
    }
}
