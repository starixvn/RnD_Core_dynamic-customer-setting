namespace Fanex.CustomerSetting.Tasks.Product21.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CustomerSetting.Models.Results;
    using CustomerSetting.Tests;
    using Models;
    using Models.Enumerations;
    using Models.Requests;
    using NSubstitute;
    using NUnit.Framework;

    public class AddSettingNoteTaskTests
    {
        private AddSettingNoteTask _task;
        private ISettingContext _settingContext;
        private ISetting _setting;
        private CustomerSettingQuery _query;

        [SetUp]
        public void SetUp()
        {
            int customerId = 1;
            byte productId = 21;
            var customerLevel = Level.Master;

            _task = new AddSettingNoteTask();
            _settingContext = Substitute.For<ISettingContext>();
            _settingContext.Actor = Substitute.For<User>();
            _settingContext.Actor.Level = Level.Super;
            _settingContext.System = (byte)SystemType.Alpha;
            _query = Substitute.For<CustomerSettingQuery>(customerId, customerLevel, productId, null);
            _setting = new Setting(customerId, customerLevel, productId, null);
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

        [TestCase(13)]
        [TestCase(7)]
        public void PostQueryCustomerSetting_EditSettingType13Or7_ShowNotes(byte settingType)
        {
            // Arrange.
            _setting.SettingTypes = new List<SettingType>
            {
                InstanceFactory.CreateCustomerSettingType(settingType)
            };

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            Assert.NotNull(_setting.WarningMessage);
        }

        [TestCase(13)]
        [TestCase(7)]
        public void PostQueryCustomerSetting_EditSettingType13Or7_Show2Notes(byte settingType)
        {
            // Arrange.
            _setting.SettingTypes = new List<SettingType>
            {
                InstanceFactory.CreateCustomerSettingType(settingType)
            };

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            var shownNotes = _setting.WarningMessage as UpdateSettingResult;

            Assert.AreEqual(2, shownNotes?.SubResults.Count());
        }

        [TestCase(13)]
        [TestCase(7)]
        public void PostQueryCustomerSetting_EditSettingType13Or7_FirstNoteIsForGeneralSettings(byte settingType)
        {
            // Arrange.
            _setting.SettingTypes = new List<SettingType>
            {
                InstanceFactory.CreateCustomerSettingType(settingType)
            };

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            var shownNotes = _setting.WarningMessage as UpdateSettingResult;
            var firstNote = shownNotes?.SubResults.ElementAt(0)?.Message;

            Assert.AreEqual(Resources.GeneralSettingNote, firstNote);
        }

        [TestCase(13)]
        [TestCase(7)]
        public void PostQueryCustomerSetting_EditSettingType13Or7_SecondNoteIsForBetSettingAndProductStatus(byte settingType)
        {
            // Arrange.
            _setting.SettingTypes = new List<SettingType>
            {
                InstanceFactory.CreateCustomerSettingType(settingType)
            };

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            var shownNotes = _setting.WarningMessage as UpdateSettingResult;
            var firstNote = shownNotes?.SubResults.ElementAt(1)?.Message;

            Assert.AreEqual(Resources.BetSettingAndProductStatusSettingNote, firstNote);
        }

        [Test]
        public void PostQueryCustomerSetting_DoNotEditSettingType13Nor7_DoNotShowNotes()
        {
            // Arrange.
            _setting.SettingTypes = new List<SettingType>
            {
                InstanceFactory.CreateCustomerSettingType(1)
            };

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            Assert.Null(_setting.WarningMessage);
        }
    }
}