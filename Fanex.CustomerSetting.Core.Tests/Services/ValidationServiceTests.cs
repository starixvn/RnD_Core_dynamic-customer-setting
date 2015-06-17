namespace Fanex.CustomerSetting.Core.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.CustomerSetting.Core.DataAccesses;
    using Fanex.CustomerSetting.Core.Dtos.Params;
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.CustomerSetting.Core.Enumerations;
    using Fanex.CustomerSetting.Core.Models;
    using Fanex.CustomerSetting.Core.Services;
    using Fanex.CustomerSetting.Core.Services.Impl;
    using Fanex.CustomerSetting.Core.SPObjects.Commands;
    using Fanex.CustomerSetting.Core.SPObjects.Commands.Validation;
    using Fanex.CustomerSetting.Core.Tests.Models;
    using Fanex.NAP.Framework.Data;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class ValidationServiceTests
    {
        private ISettingContext _settingContext;
        private IValidationService _validationService;
        private IValidationDataAccess _validationDataAccess;
        private IProductService _productService;

        [SetUp]
        public void Setup()
        {
            _validationDataAccess = Substitute.For<IValidationDataAccess>();
            _settingContext = new SettingContextStub();
            _productService = Substitute.For<IProductService>();
            _validationService = new ValidationService(_settingContext, _validationDataAccess, _productService);
        }

        [Test]
        public void ValidateSettingQuery_SettingQueryIsNull_ThrownArgumentNullException()
        {
            SettingQuery settingQuery = null;
            
            Assert.Throws<ArgumentNullException>(
                new TestDelegate(
                    () => _validationService.ValidateSettingQuery(settingQuery)
                    ));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void ValidateSettingQuery_CustomerIdIsNegativeOrZero_ThrowArgumentOutOfRangeException(int customerId)
        {
            SettingQuery settingQuery = new SettingQuery
            {
                CustomerId = customerId
            };

            Assert.Throws<ArgumentOutOfRangeException>(
                new TestDelegate(
                    () => _validationService.ValidateSettingQuery(settingQuery)
                    ));
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        [TestCase(CustomerLevel.Agent)]
        [TestCase(CustomerLevel.Member)]
        public void ValidateSettingQuery_ActorIsCompany_CanViewAllCustomerSetting(CustomerLevel customerLevel)
        {
            SettingQuery settingQuery = new SettingQuery
            {
                CustomerId = 1,
                CustomerLevel = customerLevel
            };
            _settingContext.Actor.Level = UserLevel.Company;

            var actualResult = _validationService.ValidateSettingQuery(settingQuery);

            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateSettingQuery_ActorIsSuper_CannotViewOtherSuperSetting()
        {
            SettingQuery settingQuery = new SettingQuery
            {
                CustomerId = 1,
                CustomerLevel = CustomerLevel.Super
            };
            _settingContext.Actor.Level = UserLevel.Super;

            var actualResult = _validationService.ValidateSettingQuery(settingQuery);

            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Master)]
        ////[TestCase(CustomerLevel.Agent)]
        [TestCase(CustomerLevel.Member)]
        public void ValidateSettingQuery_ActorIsSuper_CanViewDownlineSetting(CustomerLevel downlineLevel)
        {
            SettingQuery settingQuery = new SettingQuery
            {
                CustomerId = 1,
                CustomerLevel = downlineLevel
            };
            _settingContext.Actor.Level = UserLevel.Super;
            _validationDataAccess.ValidatePermission(Arg.Any<PermissionValidationCommand>()).Returns(Result.Success);

            var actualResult = _validationService.ValidateSettingQuery(settingQuery);

            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        public void ValidateSettingQuery_ActorIsMaster_CannotViewOtherMasterAndSuperSetting(CustomerLevel customerLevel)
        {
            SettingQuery settingQuery = new SettingQuery
            {
                CustomerId = 1,
                CustomerLevel = customerLevel
            };
            _settingContext.Actor.Level = UserLevel.Master;

            var actualResult = _validationService.ValidateSettingQuery(settingQuery);

            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Agent)]
        [TestCase(CustomerLevel.Member)]
        public void ValidateSettingQuery_ActorIsMaster_CanViewDownlineSetting(CustomerLevel downlineLevel)
        {
            SettingQuery settingQuery = new SettingQuery
            {
                CustomerId = 1,
                CustomerLevel = downlineLevel
            };
            _settingContext.Actor.Level = UserLevel.Master;
            _validationDataAccess.ValidatePermission(Arg.Any<PermissionValidationCommand>()).Returns(Result.Success);

            var actualResult = _validationService.ValidateSettingQuery(settingQuery);

            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [TestCase(CustomerLevel.Super)]
        [TestCase(CustomerLevel.Master)]
        [TestCase(CustomerLevel.Agent)]
        public void ValidateSettingQuery_ActorIsAgent_CannotViewOtherAgentAndUplineSetting(CustomerLevel customerLevel)
        {
            SettingQuery settingQuery = new SettingQuery
            {
                CustomerId = 1,
                CustomerLevel = customerLevel
            };
            _settingContext.Actor.Level = UserLevel.Agent;

            var actualResult = _validationService.ValidateSettingQuery(settingQuery);

            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateSettingQuery_ActorIsAgent_CanViewMemberSetting()
        {
            SettingQuery settingQuery = new SettingQuery
            {
                CustomerId = 1,
                CustomerLevel = CustomerLevel.Member
            };
            _settingContext.Actor.Level = UserLevel.Agent;
            _validationDataAccess.ValidatePermission(Arg.Any<PermissionValidationCommand>()).Returns(Result.Success);

            var actualResult = _validationService.ValidateSettingQuery(settingQuery);

            Assert.IsTrue(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateSettingQuery_ExecuteValidatePerrmissionMethod()
        {
            SettingQuery settingQuery = new SettingQuery
            {
                CustomerId = 1,
                CustomerLevel = CustomerLevel.Member
            };
            _settingContext.Actor.Level = UserLevel.Agent;

            _validationService.ValidateSettingQuery(settingQuery);

            _validationDataAccess.Received().ValidatePermission(
                Arg.Is<PermissionValidationCommand>(
                command => command.CustId == _settingContext.Actor.Id
                    && command.TargetCustId == settingQuery.CustomerId
                    && command.TargetRoleId == (int)settingQuery.CustomerLevel
                ));
        }

        [Test]
        public void ValidateSettingCommand_CustomerSettingTypeDoesNotMatchWithProductSettingType_ReturnFailedResult()
        {
            var settingCommand = FakeObjectManager.CreateSettingCommandFakeObject();
            settingCommand.Setting.SettingTypes = new List<SettingType>
            {
                FakeObjectManager.CreateSettingTypeFakeObject(1)
            };
            _productService.GetProductSettingTypes(settingCommand.Setting.Product.Id)
                .Returns(new List<SettingType> 
                { 
                    FakeObjectManager.CreateSettingTypeFakeObject(2) 
                });

            var actualResult = _validationService.ValidateSettingCommand(settingCommand);

            Assert.IsFalse(actualResult.IsSuccessful);
        }

        [Test]
        public void ValidateSettingCommand_ExecuteValidateMethodOnSettingType()
        {
            var settingCommand = FakeObjectManager.CreateSettingCommandFakeObject();
            _productService.GetProductSettingTypes(settingCommand.Setting.Product.Id).Returns(settingCommand.Setting.SettingTypes);


        }
        
    }
}
