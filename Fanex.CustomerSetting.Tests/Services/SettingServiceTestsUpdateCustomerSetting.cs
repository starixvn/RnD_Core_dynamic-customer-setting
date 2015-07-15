namespace Fanex.CustomerSetting.Tests.Services
{
    using System;
    using System.Linq;
    using Fanex.CustomerSetting.DataAccesses;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Handlers;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.Tests.Models;
    using Fanex.NAP.Framework.Data;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SettingServiceTestsUpdateCustomerSetting
    {
        private IUpdateSettingService _settingService;
        private ISettingContext _settingContext;
        private IValidationService _validationService;
        private IProductService _productService;
        private IResourceService _resourceService;
        private ILoggingService _loggingService;
        private IDynamicRepository _dymanicRepository;
        private ISettingResolver _settingResolver;
        private ISettingTypeDataAccess _settingDataAccess;

        [SetUp]
        public void Setup()
        {
            _settingContext = new SettingContextStub();
            _resourceService = Substitute.For<IResourceService>();
            _loggingService = Substitute.For<ILoggingService>();
            _productService = Substitute.For<IProductService>();
            _validationService = Substitute.For<IValidationService>();
            _settingResolver = Substitute.For<ISettingResolver>();
            _dymanicRepository = Substitute.For<IDynamicRepository>();

            _settingDataAccess = Substitute.For<ISettingTypeDataAccess>();

            _settingService = new UpdateSettingService(
                _settingContext,
                _validationService,
                _productService,
                _resourceService,
                _loggingService,
                _settingResolver,
                _dymanicRepository);
        }

        [Test]
        public void UpdateCustomerSetting_UpdatingSettingParamIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            UpdatingSettingParam updatingParam = null;

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => _settingService.UpdateCustomerSetting(updatingParam)));
        }

        [Test]
        public void UpdateCustomerSetting_Always_CallValidateUpdatingSetting()
        {
            //// Arrange
            UpdatingSettingParam updatingParam = InstanceFactory.CreateUpdatingSettingParam();

            //// Act
            _settingService.UpdateCustomerSetting(updatingParam);

            //// Assert
            _validationService.Received().ValidateUpdatingSetting(Arg.Is<UpdatingSettingParam>(
                actualSettingCommand => actualSettingCommand == updatingParam));
        }

        [Test]
        public void UpdateCustomerSetting_ValidationIsFailed_ReturnFailedResult()
        {
            //// Arrange
            UpdatingSettingParam updatingParam = InstanceFactory.CreateUpdatingSettingParam();
            _validationService.ValidateUpdatingSetting(Arg.Any<UpdatingSettingParam>()).Returns(Result.AccessDenied);

            //// Act
            var actualUpdatingResult = _settingService.UpdateCustomerSetting(updatingParam);

            //// Assert
            Assert.AreEqual(Result.AccessDenied.Code, actualUpdatingResult.Code);
        }

        private void AssumeUpdatingCustomerSettingActionIsValid()
        {
            _validationService.ValidateUpdatingSetting(Arg.Any<UpdatingSettingParam>()).Returns(Result.Success);
        }

        [Test]
        public void UpdateCustomerSetting_Always_CallResolveSettingPreTasks()
        {
            //// Arrange
            UpdatingSettingParam updatingParam = InstanceFactory.CreateUpdatingSettingParam();
            AssumeUpdatingCustomerSettingActionIsValid();
            SetupSettingTypeDataAccessStub();

            //// Act
            _settingService.UpdateCustomerSetting(updatingParam);

            //// Assert
            _settingResolver.Received(updatingParam.Setting.SettingTypes.Count()).ResolveSettingPreTasks(Arg.Any<SettingType>());
        }

        private void SetupSettingTypeDataAccessStub(bool isReturnSuccessfulResult = true)
        {
            _settingDataAccess.UpdateCustomerSettingType(
                Arg.Any<ISettingContext>(),
                Arg.Any<int>(),
                Arg.Any<CustomerLevel>(),
                Arg.Any<SettingType>(),
                Arg.Any<byte>(),
                Arg.Any<CustomerLevel?>()).Returns(isReturnSuccessfulResult ? Result.Success : Result.DataError);
            _settingResolver.ResolveSettingDataAccess(Arg.Any<SettingType>()).Returns(_settingDataAccess);
        }

        [Test]
        public void UpdateCustomerSetting_SettingTypeHasPreUpdateCustomerSettingTypeTasks_RunTasks()
        {
            //// Arrange
            UpdatingSettingParam updatingParam = InstanceFactory.CreateUpdatingSettingParam();
            AssumeUpdatingCustomerSettingActionIsValid();
            SetupSettingTypeDataAccessStub();

            var settingTypePreTasksMock = Substitute.ForPartsOf<SettingPreTasks>();
            _settingResolver.ResolveSettingPreTasks(Arg.Any<SettingType>()).Returns(settingTypePreTasksMock);

            //// Act
            _settingService.UpdateCustomerSetting(updatingParam);

            //// Assert
            settingTypePreTasksMock.Received(updatingParam.Setting.SettingTypes.Count()).PreUpdateCustomerSettingType(
                _settingContext,
                updatingParam.Setting.CustomerId,
                updatingParam.Setting.CustomerLevel,
                Arg.Any<SettingType>(),
                updatingParam.UplineLevel);
        }

        private void AssumePreUpdateCustomerSettingTypeTasksRunCompleted(bool isTaskSuccessful = true)
        {
            var settingPreTasksStub = Substitute.ForPartsOf<SettingPreTasks>();
            settingPreTasksStub.PreUpdateCustomerSettingType(
                Arg.Any<ISettingContext>(),
                Arg.Any<int>(),
                Arg.Any<CustomerLevel>(),
                Arg.Any<SettingType>(),
                Arg.Any<CustomerLevel?>())
                .Returns(new SettingResult(isTaskSuccessful ? Result.Success : Result.DataError));
            _settingResolver.ResolveSettingPreTasks(Arg.Any<SettingType>()).Returns(settingPreTasksStub);
        }

        private void AssumePostUpdateCustomerSettingTypeTasksRunCompleted(bool isTaskSuccessful = true)
        {
            var settingPostTasksStub = Substitute.ForPartsOf<SettingPostTasks>(_resourceService);
            settingPostTasksStub.PostUpdateCustomerSettingType(
                Arg.Any<ISettingContext>(),
                Arg.Any<int>(),
                Arg.Any<CustomerLevel>(),
                Arg.Any<SettingType>(),
                Arg.Any<CustomerLevel?>(),
                Arg.Any<SettingResult>())
                .Returns(new SettingResult(isTaskSuccessful ? Result.Success : Result.DataError));
            _settingResolver.ResolveSettingPostTasks(Arg.Any<SettingType>()).Returns(settingPostTasksStub);
        }

        [Test]
        public void UpdateCustomerSetting_Always_CallUpdateCustomerSettingType()
        {
            //// Arrange
            UpdatingSettingParam updatingParam = InstanceFactory.CreateUpdatingSettingParam();
            AssumeUpdatingCustomerSettingActionIsValid();
            SetupSettingTypeDataAccessStub();
            AssumePreUpdateCustomerSettingTypeTasksRunCompleted(isTaskSuccessful: true);

            //// Act
            _settingService.UpdateCustomerSetting(updatingParam);

            //// Assert
            _settingDataAccess.Received(updatingParam.Setting.SettingTypes.Count()).UpdateCustomerSettingType(
                _settingContext,
                updatingParam.Setting.CustomerId,
                updatingParam.Setting.CustomerLevel,
                Arg.Any<SettingType>(),
                updatingParam.Setting.ProductId,
                updatingParam.UplineLevel);
        }

        [Test]
        public void UpdateCustomerSetting_Always_CallResolveSettingPostTasks()
        {
            //// Arrange
            UpdatingSettingParam updatingParam = InstanceFactory.CreateUpdatingSettingParam();
            AssumeUpdatingCustomerSettingActionIsValid();
            AssumePreUpdateCustomerSettingTypeTasksRunCompleted(isTaskSuccessful: true);
            SetupSettingTypeDataAccessStub();

            //// Act
            _settingService.UpdateCustomerSetting(updatingParam);

            //// Assert
            _settingResolver.Received(updatingParam.Setting.SettingTypes.Count()).ResolveSettingPostTasks(Arg.Any<SettingType>());
        }

        [Test]
        public void UpdateCustomerSetting_SettingTypeHasPostUpdateCustomerSettingTypeTasks_RunTasks()
        {
            //// Arrange
            UpdatingSettingParam updatingParam = InstanceFactory.CreateUpdatingSettingParam();
            AssumeUpdatingCustomerSettingActionIsValid();
            SetupSettingTypeDataAccessStub();

            //// Setting type has no pre-task
            SettingPreTasks settingPreTasks = null;
            _settingResolver.ResolveSettingPreTasks(Arg.Any<SettingType>()).Returns(settingPreTasks);

            //// Setting type has post-tasks
            var settingTypePostTasksMock = Substitute.ForPartsOf<SettingPostTasks>(_resourceService);
            _settingResolver.ResolveSettingPostTasks(Arg.Any<SettingType>()).Returns(settingTypePostTasksMock);

            //// Act
            _settingService.UpdateCustomerSetting(updatingParam);

            //// Assert
            settingTypePostTasksMock.Received(updatingParam.Setting.SettingTypes.Count()).PostUpdateCustomerSettingType(
                _settingContext,
                updatingParam.Setting.CustomerId,
                updatingParam.Setting.CustomerLevel,
                Arg.Any<SettingType>(),
                updatingParam.UplineLevel,
                Arg.Any<SettingResult>());
        }

        [Test]
        public void UpdateCustomerSetting_RaisedException_CallLogUpdatingCustomerSettingTypeError()
        {
            //// Arrange
            UpdatingSettingParam updatingParam = InstanceFactory.CreateUpdatingSettingParam();
            AssumeUpdatingCustomerSettingActionIsValid();
            AssumePreUpdateCustomerSettingTypeTasksRunCompleted(isTaskSuccessful: true);

            //// Simulate exception
            var expectedException = new Exception("Exception");
            _settingResolver.ResolveSettingDataAccess(Arg.Any<SettingType>()).Returns(x => { throw expectedException; });

            //// Act
            _settingService.UpdateCustomerSetting(updatingParam);

            //// Assert
            _loggingService.Received().LogUpdatingCustomerSettingTypeError(
                Arg.Is<int>(customerId => customerId == updatingParam.Setting.CustomerId),
                Arg.Is<CustomerLevel?>(uplineLevel => uplineLevel == updatingParam.UplineLevel),
                Arg.Is<byte>(productId => productId == updatingParam.Setting.ProductId),
                Arg.Any<SettingType>(),
                Arg.Any<Exception>());
        }

        [Test]
        public void UpdateCustomerSetting_PreTasksAreFailed_ReturnFailedResult()
        {
            //// Arrange
            UpdatingSettingParam updatingParam = InstanceFactory.CreateUpdatingSettingParam();
            AssumeUpdatingCustomerSettingActionIsValid();
            AssumePreUpdateCustomerSettingTypeTasksRunCompleted(isTaskSuccessful: false);
            AssumePostUpdateCustomerSettingTypeTasksRunCompleted(isTaskSuccessful: true);
            SetupSettingTypeDataAccessStub();

            //// Act
            var actualUpdatingResult = _settingService.UpdateCustomerSetting(updatingParam);

            //// Assert
            Assert.IsFalse(actualUpdatingResult.IsSuccessful);
        }

        [Test]
        public void UpdateCustomerSetting_PostTasksAreFailed_ReturnFailedResult()
        {
            //// Arrange
            UpdatingSettingParam updatingParam = InstanceFactory.CreateUpdatingSettingParam();
            AssumeUpdatingCustomerSettingActionIsValid();
            SetupSettingTypeDataAccessStub();
            AssumePreUpdateCustomerSettingTypeTasksRunCompleted(isTaskSuccessful: true);
            AssumePostUpdateCustomerSettingTypeTasksRunCompleted(isTaskSuccessful: false);

            //// Act
            var actualUpdatingResult = _settingService.UpdateCustomerSetting(updatingParam);

            //// Assert
            Assert.IsFalse(actualUpdatingResult.IsSuccessful);
        }

        [Test]
        public void UpdateCustomerSetting_UpdatedSetting_ReturnSuccessResult()
        {
            //// Arrange
            UpdatingSettingParam updatingParam = InstanceFactory.CreateUpdatingSettingParam();
            AssumeUpdatingCustomerSettingActionIsValid();
            SetupSettingTypeDataAccessStub();
            AssumePreUpdateCustomerSettingTypeTasksRunCompleted(isTaskSuccessful: true);
            AssumePostUpdateCustomerSettingTypeTasksRunCompleted(isTaskSuccessful: true);

            //// Act
            var actualUpdatingResult = _settingService.UpdateCustomerSetting(updatingParam);

            //// Assert
            Assert.IsTrue(actualUpdatingResult.IsSuccessful);
        }
    }
}