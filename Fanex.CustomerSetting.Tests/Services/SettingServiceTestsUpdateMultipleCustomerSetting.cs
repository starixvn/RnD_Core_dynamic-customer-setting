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
        public void UpdateMultipleCustomerSetting_SettingIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            UpdatingMultipleSettingParam updatingMultipleSettingParam = InstanceFactory.CreateUpdatingMultipleSettingParam();
            updatingMultipleSettingParam.Setting = null;

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
        public void UpdateMultipleCustomerSetting_Always_ExecuteValidateUpdatingMultipleSettingMethodOnValidationService()
        {
            //// Arrange
            UpdatingMultipleSettingParam updatingMultipleSettingParam = InstanceFactory.CreateUpdatingMultipleSettingParam();

            //// Act
            _settingService.UpdateMultipleCustomerSetting(updatingMultipleSettingParam);

            //// Assert
            _validationService.Received().ValidateUpdatingMultipleSetting(updatingMultipleSettingParam);
        }

        private void AssumeUpdatingMultipleCustomersSettingActionPassedValidation()
        {
            _validationService.ValidateUpdatingMultipleSetting(Arg.Any<UpdatingMultipleSettingParam>()).Returns(Result.Success);
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
        public void UpdateMultipleCustomerSetting_SettingTypeHasPreUpdateMultipleCustomersSettingTypeTasks_ExecuteThoseTasks()
        {
            //// Arrange
            var updatingMultipleSettingParam = InstanceFactory.CreateUpdatingMultipleSettingParam();
            AssumeUpdatingMultipleCustomersSettingActionPassedValidation();
            SetupSettingTypeDataAccessStubForReturningDummyResult();

            var settingTypePreTasksMock = Substitute.ForPartsOf<SettingPreTasks>();
            _settingResolver.ResolveSettingPreTasks(Arg.Any<SettingType>()).Returns(settingTypePreTasksMock);

            //// Act
            _settingService.UpdateMultipleCustomerSetting(updatingMultipleSettingParam);

            //// Assert
            var numOfCallToPreTasks = updatingMultipleSettingParam.Setting.SettingTypes.Count();
            settingTypePreTasksMock.Received(numOfCallToPreTasks)
                .PreUpdateMultipleCustomersSettingType(
                    _settingContext,
                    updatingMultipleSettingParam.CustomerIds,
                    updatingMultipleSettingParam.Setting.CustomerLevel,
                    Arg.Any<SettingType>(),
                    updatingMultipleSettingParam.UplineLevel);
        }

        private void AssumePreUpdateMultipleCustomersSettingTypeTaskHadDone(
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
                        CustomerIdsNeedForSingleUpdating = customerIdsNeedForSingleUpdating
                    });

            _settingResolver.ResolveSettingPreTasks(Arg.Any<SettingType>()).Returns(settingTypePreTasksStub);
        }

        private void AssumePostUpdateMultipleCustomersSettingTypeTaskHadDone(bool isTaskSuccessful = true)
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
        public void UpdateMultipleCustomerSetting_Always_ExecuteResolveSettingPreTasksMethodOnSettingResolver()
        {
            //// Arrange
            UpdatingMultipleSettingParam updatingMultipleSettingParam = InstanceFactory.CreateUpdatingMultipleSettingParam();
            AssumeUpdatingMultipleCustomersSettingActionPassedValidation();
            SetupSettingTypeDataAccessStubForReturningDummyResult();
            AssumePreUpdateMultipleCustomersSettingTypeTaskHadDone();

            //// Act
            _settingService.UpdateMultipleCustomerSetting(updatingMultipleSettingParam);

            //// Assert
            var numOfSettingTypes = updatingMultipleSettingParam.Setting.SettingTypes.Count();
            var numOfSettingTypesEachCustomer = numOfSettingTypes * updatingMultipleSettingParam.CustomerIds.Count();
            var numOfCallingToResolveSettingPreTasks = numOfSettingTypes + numOfSettingTypesEachCustomer;
            _settingResolver.Received(numOfCallingToResolveSettingPreTasks).ResolveSettingPreTasks(Arg.Any<SettingType>());
        }

        [Test]
        public void UpdateMultipleCustomerSetting_ExecuteResolveSettingPostTasksMethodOnSettingResolver()
        {
            //// Arrange
            UpdatingMultipleSettingParam updatingMultipleSettingParam = InstanceFactory.CreateUpdatingMultipleSettingParam();
            AssumeUpdatingMultipleCustomersSettingActionPassedValidation();
            SetupSettingTypeDataAccessStubForReturningDummyResult();
            AssumePreUpdateMultipleCustomersSettingTypeTaskHadDone();
            AssumePostUpdateMultipleCustomersSettingTypeTaskHadDone();

            //// Act
            _settingService.UpdateMultipleCustomerSetting(updatingMultipleSettingParam);

            //// Assert
            var numOfSettingTypes = updatingMultipleSettingParam.Setting.SettingTypes.Count();
            var numOfSettingTypesEachCustomer = numOfSettingTypes * updatingMultipleSettingParam.CustomerIds.Count();
            var numOfCallingToResolveSettingPostTasks = numOfSettingTypes + numOfSettingTypesEachCustomer;
            _settingResolver.Received(numOfCallingToResolveSettingPostTasks).ResolveSettingPostTasks(Arg.Any<SettingType>());
        }

        [Test]
        public void UpdateMultipleCustomerSetting_ExecuteUpdateCustomerSettingTypeOnSettingDataAccess()
        {
            //// Arrange
            UpdatingMultipleSettingParam updatingMultipleSettingParam = InstanceFactory.CreateUpdatingMultipleSettingParam();
            AssumeUpdatingMultipleCustomersSettingActionPassedValidation();
            SetupSettingTypeDataAccessStubForReturningDummyResult();
            AssumePreUpdateMultipleCustomersSettingTypeTaskHadDone();
            AssumePostUpdateMultipleCustomersSettingTypeTaskHadDone();

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
        public void UpdateMultipleCustomerSetting_PreTaskFail_ReturnPreTasksResult()
        {
            //// Arrange
            UpdatingMultipleSettingParam updatingMultipleSettingParam = InstanceFactory.CreateUpdatingMultipleSettingParam();
            AssumeUpdatingMultipleCustomersSettingActionPassedValidation();
            SetupSettingTypeDataAccessStubForReturningDummyResult();

            AssumePreUpdateMultipleCustomersSettingTypeTaskHadDone(
                isTaskSuccessful: false,
                isStopUpdatingWhenPreTaskFailed: true,
                isReturnPreTaskResultToUser: true,
                customerIdsNeedForSingleUpdating: null);

            //// Act
            var actualResult = _settingService.UpdateMultipleCustomerSetting(updatingMultipleSettingParam);

            //// Assert
            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [Test]
        public void UpdateMultipleCustomerSetting_Updated_ReturnSuccessUpdatingSettingResult()
        {
            //// Arrange
            UpdatingMultipleSettingParam updatingMultipleSettingParam = InstanceFactory.CreateUpdatingMultipleSettingParam();
            AssumeUpdatingMultipleCustomersSettingActionPassedValidation();
            SetupSettingTypeDataAccessStubForReturningDummyResult();
            AssumePreUpdateMultipleCustomersSettingTypeTaskHadDone();
            AssumePostUpdateMultipleCustomersSettingTypeTaskHadDone();

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
            AssumeUpdatingMultipleCustomersSettingActionPassedValidation();
            SetupSettingTypeDataAccessStubForReturningDummyResult();

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
            AssumeUpdatingMultipleCustomersSettingActionPassedValidation();

            //// Simulate throw exception
            _settingResolver.ResolveSettingPreTasks(Arg.Any<SettingType>()).Returns(x => { throw new Exception(); });

            //// Act
            _settingService.UpdateMultipleCustomerSetting(updatingMultipleSettingParam);

            //// Assert
            _logger.Received().Error(Arg.Any<string>(), Arg.Any<Exception>());
        }
    }
}