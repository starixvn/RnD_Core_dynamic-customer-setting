namespace Fanex.CustomerSetting.Tasks.SettingType13.Tests
{
    using System;
    using System.Linq;
    using Models;
    using Models.Enumerations;
    using Models.Requests;
    using Models.Results;
    using NSubstitute;
    using NUnit.Framework;

    public class AddSettingType13QueryWarningTaskTest
    {
        private AddSettingType13QueryWarningTask _task;
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

            _task = new AddSettingType13QueryWarningTask();
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

            Assert.IsTrue(messageContainer?.SubResults.Any(subResult => subResult.Message == Resources.ProductStatusNoteForExternalSites));
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

            Assert.IsTrue(messageContainer?.SubResults.Any(subResult => subResult.Message == Resources.ProductStatusNoteForExternalSites));
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

            Assert.IsTrue(messageContainer?.SubResults.Any(subResult => subResult.Message == Resources.ProductStatusNoteForExternalSites));
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