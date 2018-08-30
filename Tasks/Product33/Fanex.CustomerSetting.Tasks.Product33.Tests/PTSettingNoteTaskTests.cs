namespace Fanex.CustomerSetting.Tasks.Product33.Tests
{
    using System;
    using System.Linq;
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
            _settingContext.Actor = Substitute.For<User>();
            _settingContext.Actor.Level = Level.Master;
            _settingContext.System = (byte)SystemType.Alpha;
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
        public void PostQueryCustomerSetting_DoesNotUpdateSettingType1_DoNotAddNote()
        {
            // Arrange.
            _setting.SettingTypes = _setting.SettingTypes.Where(settingType => settingType.Id != 1);

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            Assert.Null(_setting.WarningMessage);
        }

        [Test]
        public void PostQueryCustomerSetting_UpdateSettingType1_AddNoteWithCodeMatchedDefinedCode()
        {
            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            Assert.IsTrue(_setting?.WarningMessage?.Code == PTSettingNoteTask.PTSettingNoteCode);
        }

        [Test]
        public void PostQueryCustomerSetting_UpdateSettingType1_ShowNoteToUser()
        {
            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            Assert.AreEqual(Resources.PTAppliedForInsuranceAndGemsBought, _setting?.WarningMessage?.Message);
        }
    }
}