namespace Fanex.CustomerSetting.Tests.Services
{
    using CustomerSetting.Models;
    using CustomerSetting.Models.Constants;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using CustomerSetting.Services;
    using CustomerSetting.Services.Impl;
    using Data.Repository;
    using Fanex.CustomerSetting.DataAccesses;
    using Logging;
    using Models;
    using NSubstitute;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
    public class ValidationServiceTestsValidateUpdateSettingAction
    {
        private ISettingContext _settingContext;
        private ValidationService _validationService;
        private IValidationDataAccess _validationDataAccess;
        private IDynamicRepository _dynamicRepository;
        private IProductService _productService;
        private ILogger _logger;

        [SetUp]
        public void Setup()
        {
            _settingContext = new SettingContextStub();
            _productService = Substitute.For<IProductService>();
            _validationDataAccess = Substitute.For<IValidationDataAccess>();
            _dynamicRepository = Substitute.For<IDynamicRepository>();
            _logger = Substitute.For<ILogger>();

            _validationService = Substitute.ForPartsOf<ValidationService>(
                _settingContext,
                _productService,
                _validationDataAccess,
                _dynamicRepository,
                _logger);
        }

        private void AssumeValidateQuerySettingIsSuccessful()
        {
            _validationService.ValidateQuerySettingAction(Arg.Any<CustomerSettingQuery>()).Returns(Result.Success);
        }

        [Test]
        public void ValidateUpdateSetting_Always_CallValidateQuerySetting()
        {
            //// Arrange
            var updateParam = InstanceFactory.CreateUpdateSettingParam();
            _settingContext.Actor.Level = Level.Super;

            //// Act
            _validationService.ValidateUpdateSettingAction(updateParam);

            //// Assert

            _validationService.Received().ValidateQuerySettingAction(
                Arg.Is<CustomerSettingQuery>(query =>
                    query.CustomerId == updateParam.Setting.CustomerId
                    && query.CustomerLevel == updateParam.Setting.CustomerLevel
                    && query.ProductId == updateParam.Setting.ProductId
                    && query.UplineLevel == updateParam.UplineLevel));
        }

        [Test]
        public void ValidateUpdateSetting_Always_CallGetProductSettingTypes()
        {
            //// Arrange
            var updateParam = InstanceFactory.CreateUpdateSettingParam();

            AssumeValidateQuerySettingIsSuccessful();

            //// Act
            _validationService.ValidateUpdateSettingAction(updateParam);

            //// Assert
            _productService.Received().GetProductSettingTypes(
                Arg.Is<byte>(productId => productId == updateParam.Setting.ProductId),
                Arg.Any<byte>(), 
                Arg.Any<byte>());
        }

        [Test]
        public void ValidateUpdateSetting_CustomerSettingTypesDontMatchWithProductSettingTypes_ReturnFailedResult()
        {
            //// Arrange
            var updateParam = InstanceFactory.CreateUpdateSettingParam();

            AssumeValidateQuerySettingIsSuccessful();

            updateParam.Setting.SettingTypes = new List<SettingType>
            {
                InstanceFactory.CreateCustomerSettingType(1)
            };
            _productService.GetProductSettingTypes(updateParam.Setting.ProductId, SystemType.Nap, 0)
                .Returns(new List<SettingType>
                {
                    InstanceFactory.CreateCustomerSettingType(2)
                });

            //// Act
            var actualResult = _validationService.ValidateUpdateSettingAction(updateParam);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateUpdateSetting_CustomerSettingTypesMatchWithProductSettingTypes_ReturnSuccessResult()
        {
            //// Arrange
            var updateParam = InstanceFactory.CreateUpdateSettingParam();

            AssumeValidateQuerySettingIsSuccessful();

            updateParam.Setting.SettingTypes = new List<SettingType>
            {
                InstanceFactory.CreateCustomerSettingType(1)
            };
            _productService.GetProductSettingTypes(updateParam.Setting.ProductId, SystemType.Nap, 0)
                .Returns(new List<SettingType>
                {
                    InstanceFactory.CreateCustomerSettingType(1)
                });

            //// Act
            var actualResult = _validationService.ValidateUpdateSettingAction(updateParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateUpdateSetting_AnySettingTypeIsNotEditable_ReturnFailedResult()
        {
            // Arrange.
            var updateParam = InstanceFactory.CreateUpdateSettingParam();

            updateParam.Setting.SettingTypes.First().DisplayMode = DisplayModes.ReadOnly;
            AssumeValidateQuerySettingIsSuccessful();

            // Act.
            var actualResult = _validationService.ValidateUpdateSettingAction(updateParam);

            // Assert.
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateUpdateSetting_AnySettingTypeDoesNotMeetCustomerLevelConstraint_ReturnFailedResult()
        {
            // Arrange.
            var updateParam = InstanceFactory.CreateUpdateSettingParam(customerLevel: Level.Agent);

            _settingContext.Actor.Level = Level.Super;
            AssumeValidateQuerySettingIsSuccessful();

            // Act.
            var actualResult = _validationService.ValidateUpdateSettingAction(updateParam);

            // Assert.
            Assert.IsFalse(actualResult.IsSuccessful);
        }
    }
}