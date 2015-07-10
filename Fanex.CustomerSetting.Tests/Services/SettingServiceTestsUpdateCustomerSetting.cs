namespace Fanex.CustomerSetting.Tests.Services
{
    using System;
    using System.Collections.Generic;
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
    using Fanex.Logging;
    using Fanex.NAP.Framework.Data;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SettingServiceTestsUpdateCustomerSetting
    {
        private ISettingService _settingService;
        private IValidationService _validationService;
        private IProductService _productService;
        private ISettingContext _settingContext;
        private IDynamicRepository _dymanicRepository;
        private ISettingResolver _settingResolver;
        private ISettingTypeDataAccess _settingDataAccess;
        private IResourceService _resourceService;
        private ILogger _logger;
        private SettingPreTasks _settingPreTasksHandler;
        private SettingPostTasks _settingPostTasksHandler;

        [SetUp]
        public void Setup()
        {
            _dymanicRepository = Substitute.For<IDynamicRepository>();
            _settingContext = new SettingContextStub();
            _productService = Substitute.For<IProductService>();
            _validationService = Substitute.For<IValidationService>();
            _settingResolver = Substitute.For<ISettingResolver>();
            _settingDataAccess = Substitute.For<ISettingTypeDataAccess>();
            _resourceService = Substitute.For<IResourceService>();
            _settingPreTasksHandler = Substitute.ForPartsOf<SettingPreTasks>();
            _settingPostTasksHandler = Substitute.ForPartsOf<SettingPostTasks>(_resourceService);
            _logger = Substitute.For<ILogger>();

            _settingService = new SettingService(
                _settingContext,
                _validationService,
                _productService,
                _settingResolver,
                _dymanicRepository,
                _resourceService,
                _logger);
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
        public void UpdateCustomerSetting_SettingIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            UpdatingSettingParam updatingParam = InstanceCreator.CreateUpdatingSettingParam();
            updatingParam.Setting = null;

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => _settingService.UpdateCustomerSetting(updatingParam)));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void UpdateCustomerSetting_CustomerIdIsNegativeOrZero_ThrowArgumentOutOfRangeException(int customerId)
        {
            //// Arrange
            UpdatingSettingParam updatingParam = InstanceCreator.CreateUpdatingSettingParam();
            updatingParam.Setting.CustomerId = customerId;

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _settingService.UpdateCustomerSetting(updatingParam)));
        }

        [Test]
        public void UpdateCustomerSetting_SettingTypesIsEmpty_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            UpdatingSettingParam updatingParam = InstanceCreator.CreateUpdatingSettingParam();
            updatingParam.Setting.SettingTypes = new List<SettingType>();

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _settingService.UpdateCustomerSetting(updatingParam)));
        }

        [Test]
        public void UpdateCustomerSetting_ValidationServiceExecutesValidateUpdatingSettingMethod()
        {
            //// Arrange
            UpdatingSettingParam updatingParam = InstanceCreator.CreateUpdatingSettingParam();

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
            UpdatingSettingParam updatingParam = InstanceCreator.CreateUpdatingSettingParam();
            _validationService.ValidateUpdatingSetting(Arg.Any<UpdatingSettingParam>()).Returns(Result.AccessDenied);

            //// Act
            var actualUpdatingResult = _settingService.UpdateCustomerSetting(updatingParam);

            //// Assert
            Assert.AreEqual(Result.AccessDenied.Code, actualUpdatingResult.Code);
        }

        private void AssumeUpdatingCustomerSettingActionPassedValidation()
        {
            _validationService.ValidateUpdatingSetting(Arg.Any<UpdatingSettingParam>()).Returns(Result.Success);
        }

        [Test]
        public void UpdateCustomerSetting_Always_ExecuteResolveSettingPreTasksMethodOnSettingResolver()
        {
            //// Arrange
            UpdatingSettingParam updatingParam = InstanceCreator.CreateUpdatingSettingParam();
            AssumeUpdatingCustomerSettingActionPassedValidation();
            SetupSettingTypeDataAccessStubForReturningDummyResult();

            //// Act
            _settingService.UpdateCustomerSetting(updatingParam);

            //// Assert
            _settingResolver.Received(updatingParam.Setting.SettingTypes.Count()).ResolveSettingPreTasks(Arg.Any<SettingType>());
        }

        private void SetupSettingTypeDataAccessStubForReturningDummyResult(bool isReturnSuccessfulResult = true)
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
        public void UpdateCustomerSetting_SettingTypeHasPreUpdateCustomerSettingTypeTasks_ExecuteThatTasks()
        {
            //// Arrange
            UpdatingSettingParam updatingParam = InstanceCreator.CreateUpdatingSettingParam();
            AssumeUpdatingCustomerSettingActionPassedValidation();
            SetupSettingTypeDataAccessStubForReturningDummyResult();

            var settingTypePreTasksMock = Substitute.ForPartsOf<SettingPreTasks>();
            _settingResolver.ResolveSettingPreTasks(Arg.Any<SettingType>()).Returns(settingTypePreTasksMock);

            //// Act
            _settingService.UpdateCustomerSetting(updatingParam);

            //// Assert
            settingTypePreTasksMock.Received().PreUpdateCustomerSettingType(
                _settingContext,
                updatingParam.Setting.CustomerId,
                Arg.Any<SettingType>(),
                updatingParam.UplineLevel);
        }

        private void AssumePreUpdateCustomerSettingTypeTasksHadDone(bool isTaskSuccessful = true)
        {
            var settingPreTasksStub = Substitute.ForPartsOf<SettingPreTasks>();
            settingPreTasksStub.PreUpdateCustomerSettingType(
                Arg.Any<ISettingContext>(),
                Arg.Any<int>(),
                Arg.Any<SettingType>(),
                Arg.Any<CustomerLevel?>())
                .Returns(new SettingResult(isTaskSuccessful ? Result.Success : Result.DataError));
            _settingResolver.ResolveSettingPreTasks(Arg.Any<SettingType>()).Returns(settingPreTasksStub);
        }

        private void AssumePostUpdateCustomerSettingTypeTasksHadDone(bool isTaskSuccessful = true)
        {
            var settingPostTasksStub = Substitute.ForPartsOf<SettingPostTasks>(_resourceService);
            settingPostTasksStub.PostUpdateCustomerSettingType(
                Arg.Any<ISettingContext>(),
                Arg.Any<int>(),
                Arg.Any<SettingType>(),
                Arg.Any<CustomerLevel?>(),
                Arg.Any<SettingResult>())
                .Returns(new SettingResult(isTaskSuccessful ? Result.Success : Result.DataError));
            _settingResolver.ResolveSettingPostTasks(Arg.Any<SettingType>()).Returns(settingPostTasksStub);
        }

        [Test]
        public void UpdateCustomerSetting_Always_ExecuteUpdateCustomerSettingTypeOnSettingDataAccess()
        {
            //// Arrange
            UpdatingSettingParam updatingParam = InstanceCreator.CreateUpdatingSettingParam();
            AssumeUpdatingCustomerSettingActionPassedValidation();
            SetupSettingTypeDataAccessStubForReturningDummyResult();
            AssumePreUpdateCustomerSettingTypeTasksHadDone();

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
        public void UpdateCustomerSetting_Always_ExecuteResolveSettingPostTasksMethodOnSettingResolver()
        {
            //// Arrange
            UpdatingSettingParam updatingParam = InstanceCreator.CreateUpdatingSettingParam();
            AssumeUpdatingCustomerSettingActionPassedValidation();
            AssumePreUpdateCustomerSettingTypeTasksHadDone();
            SetupSettingTypeDataAccessStubForReturningDummyResult();

            //// Act
            _settingService.UpdateCustomerSetting(updatingParam);

            //// Assert
            _settingResolver.Received(updatingParam.Setting.SettingTypes.Count()).ResolveSettingPostTasks(Arg.Any<SettingType>());
        }

        [Test]
        public void UpdateCustomerSetting_SettingTypeHasPostUpdateCustomerSettingTypeTasks_ExecuteThoseTasks()
        {
            //// Arrange
            UpdatingSettingParam updatingParam = InstanceCreator.CreateUpdatingSettingParam();
            AssumeUpdatingCustomerSettingActionPassedValidation();
            SetupSettingTypeDataAccessStubForReturningDummyResult();

            //// Setting type has no pre-task
            SettingPreTasks settingPreTasks = null;
            _settingResolver.ResolveSettingPreTasks(Arg.Any<SettingType>()).Returns(settingPreTasks);

            //// Setting type has post-tasks
            var settingTypePostTasksMock = Substitute.ForPartsOf<SettingPostTasks>(_resourceService);
            _settingResolver.ResolveSettingPostTasks(Arg.Any<SettingType>()).Returns(settingTypePostTasksMock);

            //// Act
            _settingService.UpdateCustomerSetting(updatingParam);

            //// Assert
            settingTypePostTasksMock.Received().PostUpdateCustomerSettingType(
                _settingContext,
                updatingParam.Setting.CustomerId,
                Arg.Any<SettingType>(),
                updatingParam.UplineLevel,
                Arg.Any<SettingResult>());
        }

        [Test]
        public void UpdateCustomerSetting_RaisedException_ExecuteErrorOnLogger()
        {
            //// Arrange
            UpdatingSettingParam updatingParam = InstanceCreator.CreateUpdatingSettingParam();
            AssumeUpdatingCustomerSettingActionPassedValidation();

            //// Simulate exception
            _settingResolver.ResolveSettingDataAccess(Arg.Any<SettingType>()).Returns(x => { throw new Exception(); });

            //// Act
            _settingService.UpdateCustomerSetting(updatingParam);

            //// Assert
            _logger.Received().Error(Arg.Any<string>(), Arg.Any<Exception>());
        }

        [Test]
        public void UpdateCustomerSetting_PreTasksAreFailed_ReturnFailedResult()
        {
            //// Arrange
            UpdatingSettingParam updatingParam = InstanceCreator.CreateUpdatingSettingParam();
            AssumeUpdatingCustomerSettingActionPassedValidation();
            SetupSettingTypeDataAccessStubForReturningDummyResult();
            AssumePreUpdateCustomerSettingTypeTasksHadDone(false);
            AssumePostUpdateCustomerSettingTypeTasksHadDone();

            //// Act
            var actualUpdatingResult = _settingService.UpdateCustomerSetting(updatingParam);

            //// Assert
            Assert.IsFalse(actualUpdatingResult.IsSuccessful);
        }

        [Test]
        public void UpdateCustomerSetting_PostTasksAreFailed_ReturnFailedResult()
        {
            //// Arrange
            UpdatingSettingParam updatingParam = InstanceCreator.CreateUpdatingSettingParam();
            AssumeUpdatingCustomerSettingActionPassedValidation();
            SetupSettingTypeDataAccessStubForReturningDummyResult();
            AssumePreUpdateCustomerSettingTypeTasksHadDone();
            AssumePostUpdateCustomerSettingTypeTasksHadDone(false);

            //// Act
            var actualUpdatingResult = _settingService.UpdateCustomerSetting(updatingParam);

            //// Assert
            Assert.IsFalse(actualUpdatingResult.IsSuccessful);
        }

        [Test]
        public void UpdateCustomerSetting_UpdatedCustomerSetting_ReturnSuccessResult()
        {
            //// Arrange
            UpdatingSettingParam updatingParam = InstanceCreator.CreateUpdatingSettingParam();
            AssumeUpdatingCustomerSettingActionPassedValidation();
            SetupSettingTypeDataAccessStubForReturningDummyResult();
            AssumePreUpdateCustomerSettingTypeTasksHadDone();
            AssumePostUpdateCustomerSettingTypeTasksHadDone();

            //// Act
            var actualUpdatingResult = _settingService.UpdateCustomerSetting(updatingParam);

            //// Assert
            Assert.IsTrue(actualUpdatingResult.IsSuccessful);
        }
    }
}