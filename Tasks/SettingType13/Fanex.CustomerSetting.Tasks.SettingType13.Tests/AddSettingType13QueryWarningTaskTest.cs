namespace Fanex.CustomerSetting.Tasks.SettingType13.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CustomerSetting.SettingType13.Models;
    using Models;
    using Models.Enumerations;
    using Models.Requests;
    using NSubstitute;
    using NUnit.Framework;

    public class AddSettingType13QueryWarningTaskTest
    {
        private AddSettingType13QueryWarningTask _task;
        private ISettingContext _settingContext;
        private ISetting _setting;
        private CustomerSettingQuery _query;

        [SetUp]
        public void SetUp()
        {
            int customerId = 1;
            byte productId = 1;
            Level customerLevel = Level.Agent;

            _task = new AddSettingType13QueryWarningTask();
            _settingContext = Substitute.For<ISettingContext>();
            _settingContext.Actor = Substitute.For<User>();
            _settingContext.Actor.Level = Level.Master;
            _settingContext.System = (byte)SystemType.Alpha;
            _setting = CreateSampleSettingData();
            _query = Substitute.For<CustomerSettingQuery>(customerId, customerLevel, productId, null);
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
        public void PostQueryCustomerSetting_DoesNotUpdateSettingType13_NoAddWarningMessage()
        {
            // Arrange.
            _setting.SettingTypes = _setting.SettingTypes.Where(settingType => settingType.Id != Constants.SettingType13Id);

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            Assert.Null(_setting.WarningMessage);
        }

        [Test]
        public void PostQueryCustomerSetting_SystemIsNap_DoNotAddWarningMessage()
        {
            // Arrange.
            _settingContext.System = (byte)SystemType.Nap;

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            Assert.Null(_setting.WarningMessage);
        }

        [Test]
        public void PostQueryCustomerSetting_CustomerLevelIsSuper_DoNotAddWarningMessage()
        {
            // Arrange.
            _query.CustomerLevel = Level.Super;

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            Assert.Null(_setting.WarningMessage);
        }

        [Test]
        public void PostQueryCustomerSetting_CustomerLevelIsMaster_AddWarningNoteToUser()
        {
            // Arrange.
            _query.CustomerLevel = Level.Master;

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            Assert.IsTrue(_setting?.WarningMessage?.Message == Resources.ProductStatusNoteForExternalSites);
        }

        [Test]
        public void PostQueryCustomerSetting_CustomerLevelIsMaster_AddWarningMessageWithCodeIsNegative9102()
        {
            // Arrange.
            _query.CustomerLevel = Level.Master;

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            Assert.IsTrue(_setting?.WarningMessage?.Code == AddSettingType13QueryWarningTask.ProductStatusNoteForExternalSitesCode);
        }

        [Test]
        public void PostQueryCustomerSetting_CustomerLevelIsAgent_AddWarningNoteToUser()
        {
            // Arrange.
            _query.CustomerLevel = Level.Agent;

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            Assert.IsTrue(_setting?.WarningMessage?.Message == Resources.ProductStatusNoteForExternalSites);
        }

        [Test]
        public void PostQueryCustomerSetting_CustomerLevelIsAgent_AddWarningMessageWithCodeIsNegative9102()
        {
            // Arrange.
            _query.CustomerLevel = Level.Agent;

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            Assert.IsTrue(_setting?.WarningMessage?.Code == AddSettingType13QueryWarningTask.ProductStatusNoteForExternalSitesCode);
        }

        [Test]
        public void PostQueryCustomerSetting_CustomerLevelIsMember_DoNotAddWarningMessage()
        {
            // Arrange.
            _query.CustomerLevel = Level.Member;

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            Assert.Null(_setting.WarningMessage);
        }

        [TestCase((byte)SystemType.Alpha)]
        [TestCase((byte)SystemType.InternalAlpha)]
        public void PostQueryCustomerSetting_EditSettingType13OnAlpha_AddWarningMessageWithCodeIsNegative9102(byte systemId)
        {
            // Arrange.
            _settingContext.System = systemId;

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            Assert.IsTrue(_setting?.WarningMessage?.Code == AddSettingType13QueryWarningTask.ProductStatusNoteForExternalSitesCode);
        }

        [TestCase((byte)SystemType.Alpha)]
        [TestCase((byte)SystemType.InternalAlpha)]
        public void PostQueryCustomerSetting_EditSettingType13OnAlpha_AddWarningNoteToUser(byte systemId)
        {
            // Arrange.
            _settingContext.System = systemId;

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            Assert.IsTrue(_setting?.WarningMessage?.Message == Resources.ProductStatusNoteForExternalSites);
        }

        [TestCase((byte)SystemType.Nap)]
        [TestCase((byte)SystemType.ExternalNap)]
        public void PostQueryCustomerSetting_EditSettingType13OnNap_DoNotAddWarningMessage(byte systemId)
        {
            // Arrange.
            _settingContext.System = systemId;

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            Assert.Null(_setting?.WarningMessage);
        }

        private static ISetting CreateSampleSettingData()
        {
            ISetting setting = Substitute.For<ISetting>();

            setting.SettingTypes = new List<SettingType>
            {
                new SettingType
                {
                    Id = Constants.SettingType13Id,
                    DisplayMode = 1,
                    SettingItems = new List<ProductStatusItem>
                    {
                        new ProductStatusItem
                        {
                            SportTypeId = 1,
                            BetTypeId = 1,
                            IsDisabled = false,
                            Upline = new ProductStatusItem
                            {
                                IsDisabled = false
                            }
                        }
                    }
                }
            };
            setting.CustomerLevel = Level.Master;
            setting.WarningMessage = null;

            return setting;
        }
    }
}