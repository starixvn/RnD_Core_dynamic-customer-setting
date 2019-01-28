namespace Fanex.CustomerSetting.Tasks.Product33.Tests
{
    using System;
    using System.Linq;
    using CustomerSetting.Models.Results;
    using CustomerSetting.Tests;
    using Models;
    using Models.Enumerations;
    using Models.Requests;
    using NSubstitute;
    using NUnit.Framework;

    public class PTSettingNoteTaskTests
    {
        private PTSettingNoteTask _task;
        private ISettingContext _settingContext;
        private ISetting _setting;
        private CustomerSettingQuery _query;

        [SetUp]
        public void SetUp()
        {
            int customerId = 1;
            byte productId = 33;
            Level customerLevel = Level.Agent;

            _task = new PTSettingNoteTask();
            _settingContext = Substitute.For<ISettingContext>();
            _setting = InstanceFactory.CreateCustomerSetting(customerId, customerLevel, productId);
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
        public void PostQueryCustomerSetting_DoesNotUpdateSettingType1_DoNotShowPTNote()
        {
            // Arrange.
            _setting.SettingTypes = _setting.SettingTypes.Where(settingType => settingType.Id != 1);

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            Assert.Null(_setting.WarningMessages);
        }

        [Test]
        public void PostQueryCustomerSetting_UpdateSettingType1_ShowPTNote()
        {
            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            var shownNotes = _setting.WarningMessages as UpdateSettingResult;

            Assert.AreEqual(Resources.PTAppliedForInsuranceAndGemsBought, shownNotes.SubResults[0].Message);
        }
    }
}