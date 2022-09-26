namespace Fanex.CustomerSetting.Tasks.SettingType21.Tests
{
    using System;
    using System.Linq;
    using Models;
    using Models.Enumerations;
    using Models.Requests;
    using Models.Results;
    using NSubstitute;
    using NUnit.Framework;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.SettingType21.Models;

    public class AddSettingType21QueryWarningTaskTest
    {
        private AddSettingType21QueryWarningTask _task;
        private ISettingContext _settingContext;
        private Setting _setting;
        private CustomerSettingQuery _query;

        [SetUp]
        public void SetUp()
        {
            int customerId = 1;
            byte productId = 1;
            byte settingStatus = 0;
            Level customerLevel = Level.Agent;

            _task = new AddSettingType21QueryWarningTask();
            _settingContext = Substitute.For<ISettingContext>();
            _settingContext.Actor = Substitute.For<User>();
            _settingContext.Actor.Level = Level.Master;
            _settingContext.System = SystemType.Alpha;
            _query = Substitute.For<CustomerSettingQuery>(customerId, customerLevel, productId, settingStatus, null);
            _setting = new Setting();
        }

        [Test]
        public void PostQueryCustomerSetting_InputSettingContextIsNull_ThrowArgumentNullException()
        {
            // Arrange.
            _settingContext = null;

            // Act.
            TestDelegate invokeDelegate = () => _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            Assert.Throws(typeof(ArgumentNullException), invokeDelegate);
        }

        [Test]
        public void DisableChildrenProduct21_ParentProductDisable_ReadOnlyChildrenProduct()
        {
            // Arrange.
            var childProductStatusItem = new ProductStatusItem
                { BetTypeId = 4001, ParentBetTypeId = 4001, ParentSportTypeId = 212, SportTypeId = 9904 };
            var parentProductStatusItem = new ProductStatusItem
                { BetTypeId = 4001, ParentBetTypeId = 0, ParentSportTypeId = 0, SportTypeId = 212, IsDisabled = true };
            _setting = new Setting(1176070, Level.Super, null, 22, "Macau Games", new[]
            {
                new SettingType()
                {
                    Id = 21,
                    Name = "Product Status",
                    SettingItems = new List<ProductStatusItem>
                    {
                        childProductStatusItem,
                        parentProductStatusItem
                    }
                }
            });
            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.

            Assert.True(childProductStatusItem.IsReadOnly);
        }  
        
        [Test]
        public void SettingType21Null_DisableChildrenByParentState_NotRun()
        {
            // Arrange.
            var childProductStatusItem = new ProductStatusItem { BetTypeId = 4001, ParentBetTypeId = 4001, ParentSportTypeId = 212, SportTypeId = 9904 };
            _setting = new Setting(1176070, Level.Super, null, 22, "Macau Games",null);
            
            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.

            Assert.False(childProductStatusItem.IsReadOnly);
        }

        [Test]
        public void PostQueryCustomerSetting_InputQueryIsNull_ThrowArgumentNullException()
        {
            // Arrange.
            _query = null;

            // Act.
            TestDelegate invokeDelegate = () => _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            Assert.Throws(typeof(ArgumentNullException), invokeDelegate);
        }

        [Test]
        public void PostQueryCustomerSetting_InputSettingIsNull_ThrowArgumentNullException()
        {
            // Arrange.
            _setting = null;

            // Act.
            TestDelegate invokeDelegate = () => _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            Assert.Throws(typeof(ArgumentNullException), invokeDelegate);
        }

        [Test]
        public void PostQueryCustomerSetting_SystemIsNap_DoNotAddWarningMessage()
        {
            // Arrange.
            _settingContext.System = SystemType.Nap;

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            Assert.Null(_setting.WarningMessages);
        }

        [Test]
        public void PostQueryCustomerSetting_CustomerLevelIsSuper_DoNotAddWarningMessage()
        {
            // Arrange.
            _query.CustomerLevel = Level.Super;

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            Assert.Null(_setting.WarningMessages);
        }

        [Test]
        public void PostQueryCustomerSetting_CustomerLevelIsMaster_AddWarningNoteToUser()
        {
            // Arrange.
            _query.CustomerLevel = Level.Master;

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            var messageContainer = _setting.WarningMessages as UpdateSettingResult;

            Assert.IsTrue(messageContainer?.SubResults.Any(subResult =>
                subResult.Message == Resources.ProductStatusNoteForExternalSites));
        }

        [Test]
        public void PostQueryCustomerSetting_CustomerLevelIsAgent_AddWarningNoteToUser()
        {
            // Arrange.
            _query.CustomerLevel = Level.Agent;

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            var messageContainer = _setting.WarningMessages as UpdateSettingResult;

            Assert.IsTrue(messageContainer?.SubResults.Any(subResult =>
                subResult.Message == Resources.ProductStatusNoteForExternalSites));
        }

        [Test]
        public void PostQueryCustomerSetting_CustomerLevelIsMember_DoNotAddWarningMessage()
        {
            // Arrange.
            _query.CustomerLevel = Level.Member;

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            Assert.Null(_setting.WarningMessages);
        }

        [TestCase(SystemType.Alpha)]
        [TestCase(SystemType.InternalAlpha)]
        [TestCase(SystemType.Y7Alpha)]
        [TestCase(SystemType.Y7InternalAlpha)]
        public void PostQueryCustomerSetting_OnAlpha_AddWarningNoteToUser(byte systemId)
        {
            // Arrange.
            _settingContext.System = systemId;

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            var messageContainer = _setting.WarningMessages as UpdateSettingResult;

            Assert.IsTrue(messageContainer?.SubResults.Any(subResult =>
                subResult.Message == Resources.ProductStatusNoteForExternalSites));
        }

        [TestCase(SystemType.Nap)]
        [TestCase(SystemType.ExternalNap)]
        [TestCase(SystemType.Y7ExternalNap)]
        public void PostQueryCustomerSetting_OnNap_DoNotAddWarningMessage(byte systemId)
        {
            // Arrange.
            _settingContext.System = systemId;

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            Assert.Null(_setting.WarningMessages);
        }
    }
}