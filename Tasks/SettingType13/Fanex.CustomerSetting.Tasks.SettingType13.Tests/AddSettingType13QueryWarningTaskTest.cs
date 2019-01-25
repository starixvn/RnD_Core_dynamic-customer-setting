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
            _query = Substitute.For<CustomerSettingQuery>(customerId, customerLevel, productId, null);
            _setting = Substitute.For<ISetting>();
            _setting.WarningMessage = null;
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
            Assert.IsTrue(_setting.WarningMessage?.Message == Resources.ProductStatusNoteForExternalSites);
        }

        [Test]
        public void PostQueryCustomerSetting_CustomerLevelIsMaster_AddWarningMessageWithCodeIsNegative9102()
        {
            // Arrange.
            _query.CustomerLevel = Level.Master;

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            Assert.IsTrue(_setting.WarningMessage?.Code == AddSettingType13QueryWarningTask.ProductStatusNoteForExternalSitesCode);
        }

        [Test]
        public void PostQueryCustomerSetting_CustomerLevelIsAgent_AddWarningNoteToUser()
        {
            // Arrange.
            _query.CustomerLevel = Level.Agent;

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            Assert.IsTrue(_setting.WarningMessage?.Message == Resources.ProductStatusNoteForExternalSites);
        }

        [Test]
        public void PostQueryCustomerSetting_CustomerLevelIsAgent_AddWarningMessageWithCodeIsNegative9102()
        {
            // Arrange.
            _query.CustomerLevel = Level.Agent;

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            Assert.IsTrue(_setting.WarningMessage?.Code == AddSettingType13QueryWarningTask.ProductStatusNoteForExternalSitesCode);
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
        public void PostQueryCustomerSetting_OnAlpha_AddWarningMessageWithCodeIsNegative9102(byte systemId)
        {
            // Arrange.
            _settingContext.System = systemId;

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            Assert.IsTrue(_setting.WarningMessage?.Code == AddSettingType13QueryWarningTask.ProductStatusNoteForExternalSitesCode);
        }

        [TestCase((byte)SystemType.Alpha)]
        [TestCase((byte)SystemType.InternalAlpha)]
        public void PostQueryCustomerSetting_OnAlpha_AddWarningNoteToUser(byte systemId)
        {
            // Arrange.
            _settingContext.System = systemId;

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            Assert.IsTrue(_setting.WarningMessage?.Message == Resources.ProductStatusNoteForExternalSites);
        }

        [TestCase((byte)SystemType.Nap)]
        [TestCase((byte)SystemType.ExternalNap)]
        public void PostQueryCustomerSetting_OnNap_DoNotAddWarningMessage(byte systemId)
        {
            // Arrange.
            _settingContext.System = systemId;

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            Assert.Null(_setting.WarningMessage);
        }

        [Test]
        public void PostQueryCustomerSetting_AnyWarningMessageExists_AddWarningNodeToTheTop()
        {
            // Arrange.
            _setting.WarningMessage = Substitute.For<IResult>();

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            var allWarningMessages = (_setting.WarningMessage as UpdateSettingResult)?.SubResults;

            Assert.IsTrue(allWarningMessages.ElementAt(0).Message == Resources.ProductStatusNoteForExternalSites);
        }
    }
}