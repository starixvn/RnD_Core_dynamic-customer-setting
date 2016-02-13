namespace Fanex.CustomerSetting.Casino.Tests.Services.Impl
{
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
    using System;
    using System.Collections.Generic;
    using System.Linq;


    [TestFixture]
    public class UpdateCasinoSettingServiceTests
    {
        private IUpdateSettingService _settingService;
        private ISettingContext _settingContext;
        private IValidationService _validationService;
        private IProductService _productService;
        private ITaskService _taskService;
        private ILogger _logger;
        private ISettingTypeServiceFactory _settingTypeServiceFactory;
        private ISettingTypeService _settingTypeService;

        [SetUp]
        public void Setup()
        {
            _settingContext = new SettingContextStub();
            _productService = Substitute.For<IProductService>();
            _validationService = Substitute.For<IValidationService>();
            _taskService = Substitute.For<ITaskService>();
            _logger = Substitute.For<ILogger>();
            _settingTypeServiceFactory = Substitute.For<ISettingTypeServiceFactory>();
            _settingTypeService = Substitute.For<ISettingTypeService>();
            _settingService = new UpdateCasinoSettingService(
                _settingContext,
                _validationService,
                _taskService,
                _logger,
                _settingTypeServiceFactory);
        }

        [Test]
        public void UpdateCustomerSettingTypes_AFailedResultExists_DoNotCallUpdateStatus()
        {
            //// Arrange
            const int statusSettingTypeValue = 1;
            var settingTypeValues = new byte[] { statusSettingTypeValue, 2, 5, 3 };
            var command = StubHelper.CreateUpdateSettingParam(settingTypeValues: settingTypeValues);
            var services = new List<ISettingTypeService>();
            settingTypeValues
                .ToList()
                .ForEach(
                    value =>
                    {
                        var settingType = command
                        .Setting
                        .SettingTypes
                        .FirstOrDefault(s => s.Id == value);
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

            _settingService = new UpdateCasinoSettingService(
                _settingContext,
                _validationService,
                _taskService,
                _logger,
                _settingTypeServiceFactory);

            //// Act
            _settingService.Update(command);

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

        [Test]
        public void UpdateCustomerSettingTypes_AllResultsAreSuccessfull_CallUpdateStatus()
        {
            //// Arrange
            const int statusSettingTypeValue = 1;
            var settingTypeValues = new byte[] { statusSettingTypeValue, 4, 5, 3 };
            var command = StubHelper.CreateUpdateSettingParam(settingTypeValues: settingTypeValues);
            var services = new List<ISettingTypeService>();
            settingTypeValues
                .ToList()
                .ForEach(
                    value =>
                    {
                        var settingType = command
                        .Setting
                        .SettingTypes
                        .FirstOrDefault(s => s.Id == value);
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

            _settingService = new UpdateCasinoSettingService(
                _settingContext,
                _validationService,
                _taskService,
                _logger,
                _settingTypeServiceFactory);

            //// Act
            _settingService.Update(command);

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

    }
}
