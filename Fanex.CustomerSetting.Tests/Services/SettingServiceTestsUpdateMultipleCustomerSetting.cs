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
    public class SettingServiceTestsUpdateMultipleCustomerSetting
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
        public void UpdateMultipleCustomerSetting_UpdatingMultipleSettingParamIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            UpdatingMultipleSettingParam updatingMultipleSettingParam = null;

            //// Act
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => _settingService.UpdateMultipleCustomerSetting(updatingMultipleSettingParam)));
        }

        [Test]
        public void UpdateMultipleCustomerSetting_NumberOfSettingTypesIsZero_ThrowArgumentOutOfRangeException()
        {
            //// Arrange
            UpdatingMultipleSettingParam updatingMultipleSettingParam = InstanceFactory.CreateUpdatingMultipleSettingParam();
            updatingMultipleSettingParam.Setting.SettingTypes = new List<SettingType>();

            //// Act
            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _settingService.UpdateMultipleCustomerSetting(updatingMultipleSettingParam)));
        }

        [Test]
        public void UpdateMultipleCustomerSetting_Always_CallValidateUpdatingMultipleSetting()
        {
            //// Arrange
            UpdatingMultipleSettingParam updatingMultipleSettingParam = InstanceFactory.CreateUpdatingMultipleSettingParam();

            //// Act
            _settingService.UpdateMultipleCustomerSetting(updatingMultipleSettingParam);

            //// Assert
            _validationService.Received().ValidateUpdatingMultipleSetting(updatingMultipleSettingParam);
        }

        private void AssumeUpdatingMultipleCustomersSettingActionIsValid()
        {
            _validationService.ValidateUpdatingMultipleSetting(Arg.Any<UpdatingMultipleSettingParam>()).Returns(Result.Success);
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
        public void UpdateMultipleCustomerSetting_SettingTypeHasPreUpdateMultipleCustomersSettingTypeTasks_RunTasks()
        {
            //// Arrange
            var updatingMultipleSettingParam = InstanceFactory.CreateUpdatingMultipleSettingParam();
            AssumeUpdatingMultipleCustomersSettingActionIsValid();
            SetupSettingTypeDataAccessStub();

            var settingTypePreTasksMock = Substitute.ForPartsOf<SettingPreTasks>();
            _settingResolver.ResolveSettingPreTasks(Arg.Any<SettingType>()).Returns(settingTypePreTasksMock);

            //// Act
            _settingService.UpdateMultipleCustomerSetting(updatingMultipleSettingParam);

            //// Assert
            var numOfRunPreTasks = updatingMultipleSettingParam.Setting.SettingTypes.Count();
            settingTypePreTasksMock.Received(numOfRunPreTasks)
                .PreUpdateMultipleCustomersSettingType(
                    _settingContext,
                    updatingMultipleSettingParam.CustomerIds,
                    updatingMultipleSettingParam.Setting.CustomerLevel,
                    Arg.Any<SettingType>(),
                    updatingMultipleSettingParam.UplineLevel);
        }

        private void AssumePreUpdateMultipleCustomersSettingTypeTasksRunCompleted(
            bool isTaskSuccessful = true,
            bool isStopUpdatingWhenPreTaskFailed = false,
            bool isReturnPreTaskResultToUser = false,
            IEnumerable<int> customerIdsNeedForSingleUpdating = null)
        {
            var settingTypePreTasksStub = Substitute.ForPartsOf<SettingPreTasks>();

            settingTypePreTasksStub.PreUpdateCustomerSettingType(
               Arg.Any<ISettingContext>(),
               Arg.Any<int>(),
               Arg.Any<SettingType>(),
               Arg.Any<CustomerLevel?>())
               .Returns(new SettingResult(isTaskSuccessful ? Result.Success : Result.DataError));

            settingTypePreTasksStub.PreUpdateMultipleCustomersSettingType(
                    Arg.Any<ISettingContext>(),
                    Arg.Any<IEnumerable<int>>(),
                    Arg.Any<CustomerLevel>(),
                    Arg.Any<SettingType>(),
                    Arg.Any<CustomerLevel?>()).Returns(
                    new PreUpdateMultipleCustomersSettingTypeResult
                    {
                        Result = new UpdatingSettingResult(isTaskSuccessful ? Result.Success : Result.DataError),
                        IsStopUpdating = isStopUpdatingWhenPreTaskFailed,
                        IsReturnResultToUser = isReturnPreTaskResultToUser,
                        CustomerIdsForSingleUpdating = customerIdsNeedForSingleUpdating
                    });

            _settingResolver.ResolveSettingPreTasks(Arg.Any<SettingType>()).Returns(settingTypePreTasksStub);
        }

        private void AssumePostUpdateMultipleCustomersSettingTypeTasksRunCompleted(bool isTaskSuccessful = true)
        {
            var settingTypePostTasksStub = Substitute.ForPartsOf<SettingPostTasks>(_resourceService);

            settingTypePostTasksStub.PostUpdateCustomerSettingType(
                Arg.Any<ISettingContext>(),
                Arg.Any<int>(),
                Arg.Any<SettingType>(),
                Arg.Any<CustomerLevel?>(),
                Arg.Any<SettingResult>())
                .Returns(new SettingResult(isTaskSuccessful ? Result.Success : Result.DataError));

            var subResults = new List<SettingResult> { new SettingResult(isTaskSuccessful ? Result.Success : Result.DataError) };
            var dummyUpdatingResult = new UpdatingSettingResult(subResults);
            settingTypePostTasksStub.PostUpdateMultipleCustomersSettingType(
                    Arg.Any<ISettingContext>(),
                    Arg.Any<IEnumerable<int>>(),
                    Arg.Any<SettingType>(),
                    Arg.Any<CustomerLevel?>(),
                    Arg.Any<UpdatingSettingResult>()).Returns(dummyUpdatingResult);

            _settingResolver.ResolveSettingPostTasks(Arg.Any<SettingType>()).Returns(settingTypePostTasksStub);
        }

        [Test]
        public void UpdateMultipleCustomerSetting_Always_CallResolveSettingPreTasks()
        {
            //// Arrange
            UpdatingMultipleSettingParam updatingMultipleSettingParam = InstanceFactory.CreateUpdatingMultipleSettingParam();
            AssumeUpdatingMultipleCustomersSettingActionIsValid();
            SetupSettingTypeDataAccessStub();
            AssumePreUpdateMultipleCustomersSettingTypeTasksRunCompleted(isTaskSuccessful: true);

            //// Act
            _settingService.UpdateMultipleCustomerSetting(updatingMultipleSettingParam);

            //// Assert
            var numOfSettingTypes = updatingMultipleSettingParam.Setting.SettingTypes.Count();
            var numOfSettingTypesEachCustomer = numOfSettingTypes * updatingMultipleSettingParam.CustomerIds.Count();
            var numOfCallingToResolveSettingPreTasks = numOfSettingTypes + numOfSettingTypesEachCustomer;
            _settingResolver.Received(numOfCallingToResolveSettingPreTasks).ResolveSettingPreTasks(Arg.Any<SettingType>());
        }

        [Test]
        public void UpdateMultipleCustomerSetting_Always_CallResolveSettingPostTasks()
        {
            //// Arrange
            UpdatingMultipleSettingParam updatingMultipleSettingParam = InstanceFactory.CreateUpdatingMultipleSettingParam();
            AssumeUpdatingMultipleCustomersSettingActionIsValid();
            SetupSettingTypeDataAccessStub();
            AssumePreUpdateMultipleCustomersSettingTypeTasksRunCompleted(isTaskSuccessful: true);
            AssumePostUpdateMultipleCustomersSettingTypeTasksRunCompleted(isTaskSuccessful: true);

            //// Act
            _settingService.UpdateMultipleCustomerSetting(updatingMultipleSettingParam);

            //// Assert
            var numOfSettingTypes = updatingMultipleSettingParam.Setting.SettingTypes.Count();
            var numOfSettingTypesEachCustomer = numOfSettingTypes * updatingMultipleSettingParam.CustomerIds.Count();
            var numOfCallingToResolveSettingPostTasks = numOfSettingTypes + numOfSettingTypesEachCustomer;
            _settingResolver.Received(numOfCallingToResolveSettingPostTasks).ResolveSettingPostTasks(Arg.Any<SettingType>());
        }

        [Test]
        public void UpdateMultipleCustomerSetting_SettingTypeHasPostUpdateMultipleCustomersSettingTypeTasks_RunTasks()
        {
            //// Arrange
            var updatingMultipleSettingParam = InstanceFactory.CreateUpdatingMultipleSettingParam();
            AssumeUpdatingMultipleCustomersSettingActionIsValid();
            SetupSettingTypeDataAccessStub();
            AssumePreUpdateMultipleCustomersSettingTypeTasksRunCompleted(isTaskSuccessful: true);

            var settingTypePostTasksMock = Substitute.ForPartsOf<SettingPostTasks>(_resourceService);
            _settingResolver.ResolveSettingPostTasks(Arg.Any<SettingType>()).Returns(settingTypePostTasksMock);

            //// Act
            _settingService.UpdateMultipleCustomerSetting(updatingMultipleSettingParam);

            //// Assert
            var numOfRunPostTasks = updatingMultipleSettingParam.Setting.SettingTypes.Count();
            settingTypePostTasksMock.Received(numOfRunPostTasks)
                .PostUpdateMultipleCustomersSettingType(
                    _settingContext,
                    updatingMultipleSettingParam.CustomerIds,
                    Arg.Any<SettingType>(),
                    updatingMultipleSettingParam.UplineLevel,
                    Arg.Any<UpdatingSettingResult>());
        }

        [Test]
        public void UpdateMultipleCustomerSetting_Always_CallUpdateCustomerSettingType()
        {
            //// Arrange
            UpdatingMultipleSettingParam updatingMultipleSettingParam = InstanceFactory.CreateUpdatingMultipleSettingParam();
            AssumeUpdatingMultipleCustomersSettingActionIsValid();
            SetupSettingTypeDataAccessStub();
            AssumePreUpdateMultipleCustomersSettingTypeTasksRunCompleted(isTaskSuccessful: true);
            AssumePostUpdateMultipleCustomersSettingTypeTasksRunCompleted(isTaskSuccessful: true);

            //// Act
            _settingService.UpdateMultipleCustomerSetting(updatingMultipleSettingParam);

            //// Assert
            var numOfSettingTypes = updatingMultipleSettingParam.Setting.SettingTypes.Count();
            var numOfCallingToUpdateCustomerSettingType = numOfSettingTypes * updatingMultipleSettingParam.CustomerIds.Count();
            _settingDataAccess.Received(numOfCallingToUpdateCustomerSettingType).UpdateCustomerSettingType(
                Arg.Any<ISettingContext>(),
                Arg.Any<int>(),
                Arg.Any<CustomerLevel>(),
                Arg.Any<SettingType>(),
                Arg.Any<byte>(),
                Arg.Any<CustomerLevel?>());
        }

        [Test]
        public void UpdateMultipleCustomerSetting_PreTasksAreFailed_ReturnPreTasksFailedResult()
        {
            //// Arrange
            UpdatingMultipleSettingParam updatingMultipleSettingParam = InstanceFactory.CreateUpdatingMultipleSettingParam();
            AssumeUpdatingMultipleCustomersSettingActionIsValid();
            SetupSettingTypeDataAccessStub();

            AssumePreUpdateMultipleCustomersSettingTypeTasksRunCompleted(
                isTaskSuccessful: false,
                isStopUpdatingWhenPreTaskFailed: true,
                isReturnPreTaskResultToUser: false,
                customerIdsNeedForSingleUpdating: null);

            //// Act
            var actualResult = _settingService.UpdateMultipleCustomerSetting(updatingMultipleSettingParam);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [Test]
        public void UpdateMultipleCustomerSetting_PreTaskAreFailed_ContinueUpdatingSettingAndReturnPreTasksFailedResult()
        {
            //// Arrange
            UpdatingMultipleSettingParam updatingMultipleSettingParam = InstanceFactory.CreateUpdatingMultipleSettingParam();
            AssumeUpdatingMultipleCustomersSettingActionIsValid();
            SetupSettingTypeDataAccessStub();

            AssumePreUpdateMultipleCustomersSettingTypeTasksRunCompleted(
                isTaskSuccessful: false,
                isStopUpdatingWhenPreTaskFailed: false,
                isReturnPreTaskResultToUser: true,
                customerIdsNeedForSingleUpdating: new List<int> { 2 });

            //// Act
            var actualResult = _settingService.UpdateMultipleCustomerSetting(updatingMultipleSettingParam);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [Test]
        public void UpdateMultipleCustomerSetting_Updated_ReturnSuccessResult()
        {
            //// Arrange
            UpdatingMultipleSettingParam updatingMultipleSettingParam = InstanceFactory.CreateUpdatingMultipleSettingParam();
            AssumeUpdatingMultipleCustomersSettingActionIsValid();
            SetupSettingTypeDataAccessStub();
            AssumePreUpdateMultipleCustomersSettingTypeTasksRunCompleted(isTaskSuccessful: true);
            AssumePostUpdateMultipleCustomersSettingTypeTasksRunCompleted(isTaskSuccessful: true);

            //// Act
            var actualResult = _settingService.UpdateMultipleCustomerSetting(updatingMultipleSettingParam);

            //// Assert
            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [Test]
        public void UpdateMultipleCustomerSetting_Updated_ReturnResultWithCustomerName()
        {
            //// Arrange
            UpdatingMultipleSettingParam updatingMultipleSettingParam = InstanceFactory.CreateUpdatingMultipleSettingParam();
            AssumeUpdatingMultipleCustomersSettingActionIsValid();
            SetupSettingTypeDataAccessStub();

            SettingPreTasks settingPreTask = null;
            _settingResolver.ResolveSettingPreTasks(Arg.Any<SettingType>()).Returns(settingPreTask);

            SettingPostTasks settingPostTask = null;
            _settingResolver.ResolveSettingPostTasks(Arg.Any<SettingType>()).Returns(settingPostTask);

            //// Act
            var actualResult = _settingService.UpdateMultipleCustomerSetting(updatingMultipleSettingParam);

            //// Assert
            var updatingSettingResult = actualResult as UpdatingSettingResult;
            Assert.IsTrue(
                updatingSettingResult.SubResults.Any(settingResult => settingResult.CustomerName == "Username1")
                && updatingSettingResult.SubResults.Any(settingResult => settingResult.CustomerName == "Username2"));
        }

        [Test]
        public void UpdateMultipleCustomerSetting_RaisedException_ExecuteErrorOnLogger()
        {
            //// Arrange
            UpdatingMultipleSettingParam updatingMultipleSettingParam = InstanceFactory.CreateUpdatingMultipleSettingParam();
            AssumeUpdatingMultipleCustomersSettingActionIsValid();

            //// Simulate throw exception
            _settingResolver.ResolveSettingPreTasks(Arg.Any<SettingType>()).Returns(x => { throw new Exception(); });

            //// Act
            _settingService.UpdateMultipleCustomerSetting(updatingMultipleSettingParam);

            //// Assert
            _logger.Received().Error(Arg.Any<string>(), Arg.Any<Exception>());
        }
    }
}