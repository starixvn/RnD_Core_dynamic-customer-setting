namespace Fanex.CustomerSetting.Tasks.SettingType13.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Models;
    using Models.Constants;
    using Models.Enumerations;
    using Models.Requests;
    using Models.Results;
    using NSubstitute;
    using NUnit.Framework;

    public class AddSettingType13QueryWarningTaskTest
    {
        private AddSettingType13QueryWarningTask _task;
        private List<SettingResult> _subResults;
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
            _subResults = new List<SettingResult>();
            _settingContext = Substitute.For<ISettingContext>();
            _settingContext.Actor = Substitute.For<User>();
            _settingContext.Actor.Level = Level.Master;
            _settingContext.System = (byte)SystemType.Alpha;
            _setting = SampleData.CreateSampleSettingData();
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
            _setting.SettingTypes = _setting.SettingTypes.Where(settingType => settingType.Id != AddSettingType13WarningTask.SettingType13Id);

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            Assert.Null(_setting.WarningMessage);
        }

        [Test]
        public void PostQueryCustomerSetting_SystemIsNap_NoAddWarningMessage()
        {
            // Arrange.
            _settingContext.System = (byte)SystemType.Nap;

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            Assert.Null(_setting.WarningMessage);
        }

        [Test]
        public void PostQueryCustomerSetting_DisplayModeIsReadonly_NoAddWarningMessage()
        {
            // Arrange.
            SettingType settingType13 = _setting.SettingTypes.FirstOrDefault(settingType => settingType.Id == AddSettingType13WarningTask.SettingType13Id);

            settingType13.DisplayMode = DisplayMode.ReadOnly;

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            Assert.Null(_setting.WarningMessage);
        }

        [TestCase((byte)SystemType.Alpha)]
        [TestCase((byte)SystemType.InternalAlpha)]
        [TestCase((byte)SystemType.ExternalNap)]
        public void PostQueryCustomerSetting_EditSettingType13OnExternalSites_AddWarningMessageWithCodeIsNegative9102(byte systemId)
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
        [TestCase((byte)SystemType.ExternalNap)]
        public void PostQueryCustomerSetting_EditSettingType13OnExternalSites_AddWarningNoteToUser(byte systemId)
        {
            // Arrange.
            _settingContext.System = systemId;

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            Assert.IsTrue(_setting?.WarningMessage?.Message == Resources.ProductStatusNoteForExternalSites);
        }
    }
}