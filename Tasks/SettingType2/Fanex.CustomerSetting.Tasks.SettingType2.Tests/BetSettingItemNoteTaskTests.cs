
namespace Fanex.CustomerSetting.Tasks.SettingType2.Tests
{
    using Fanex.CustomerSetting.SettingType2.Models;
    using Models;
    using Models.Enumerations;
    using Models.Requests;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class BetSettingItemNoteTaskTests
    {
        private BetSettingItemNoteTask _task;
        private ISettingContext _settingContext;
        private SettingType _setting;
        private CustomerSettingQuery _query;

        [SetUp]
        public void SetUp()
        {
            int customerId = 1;
            byte productId = 33;
            byte settingStatus = 0;
            Level customerLevel = Level.Agent;

            _task = new BetSettingItemNoteTask();
            _settingContext = Substitute.For<ISettingContext>();
            _setting = new SettingType();
            _query = Substitute.For<CustomerSettingQuery>(customerId, customerLevel, productId, settingStatus, null);

            _settingContext.System = SystemType.Nap;
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
        public void PostQueryCustomerSetting_NoSupportedSporttype_NoteMessageIsNullAndNotShow()
        {
            _setting = new SettingType();
            _setting.SettingItems = new List<SettingItemDefault>() {
                new SettingItemDefault()
            };

            _task.Invoke(_settingContext, _query, _setting);
            var actualSettingItem = (SettingItemDefault)_setting.SettingItems.ToList()[0];
            Assert.IsTrue(!actualSettingItem.ShowIconTooltipMessage && actualSettingItem.TooltipMessage == null);
        }

        [Test]
        public void PostQueryCustomerSetting_HasSupportedSporttype_ShowNoteMessage()
        {
            _setting = new SettingType();
            _setting.SettingItems = new List<SettingItemDefault>() {
                new SettingItemDefault{ SportTypeId = 169}
            };

            _task.Invoke(_settingContext, _query, _setting);
            var actualSettingItem = (SettingItemDefault)_setting.SettingItems.ToList()[0];
            Assert.IsTrue(actualSettingItem.ShowIconTooltipMessage && actualSettingItem.TooltipMessage == Resources.BetSettingNoteMessage);
        }
    }
}
