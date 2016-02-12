namespace Fanex.CustomerSetting.Casino.Tests.Services.Impl
{
    using System;
    using Casino.Services.Impl;
    using Criteria;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Services;
    using Data.Repository;
    using Logging;
    using Models;
    using NSubstitute;
    using NUnit.Framework;

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
        public void UpdateCustomerSettingTypes_Always_StatusSettingTypeIsTheLastOneIsUpdated()
        {
            //// Arrange
            UpdateCustomerSettingCommand updateParam = InstanceFactory.CreateUpdateSettingParam();

            //// Act
            _settingService.Update(updateParam);

            //// Assert

        }
    }
}
