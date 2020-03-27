namespace Fanex.CustomerSetting.Tasks.SettingType2.Tests
{
    using System;
    using System.Linq;
    using CustomerSetting.Models.Results;
    using Models;
    using Models.Enumerations;
    using Models.Requests;
    using NSubstitute;
    using NUnit.Framework;

    public class BetSettingNotesTaskTests
    {
        private BetSettingNotesTask _task;
        private ISettingContext _settingContext;
        private ISetting _setting;
        private CustomerSettingQuery _query;

        [SetUp]
        public void SetUp()
        {
            int customerId = 1;
            byte productId = 33;
            Level customerLevel = Level.Agent;

            _task = new BetSettingNotesTask();
            _settingContext = Substitute.For<ISettingContext>();
            _setting = new Setting();
            _query = Substitute.For<CustomerSettingQuery>(customerId, customerLevel, productId, null);

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

        [TestCase(SystemType.Nap)]
        [TestCase(SystemType.Alpha)]
        [TestCase(SystemType.InternalAlpha)]
        [TestCase(SystemType.ExternalNap)]
        [TestCase(SystemType.ExternalNapBetSetting)]
        public void PostQueryCustomerSetting_SystemIsNot789Y_ShowUnlimitedBetSettingNote(byte system)
        {
            // Arrange.
            _settingContext.System = system;

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            Assert.NotNull(_setting.WarningMessages);
        }

        [TestCase(SystemType.Y7Alpha)]
        [TestCase(SystemType.Y7InternalAlpha)]
        [TestCase(SystemType.Y7ExternalNap)]
        [TestCase(SystemType.Y7ExternalNapBetSetting)]
        public void PostQueryCustomerSetting_SystemIs789Y_DoNotShowUnlimitedBetSettingNote(byte system)
        {
            // Arrange.
            _settingContext.System = system;

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            Assert.Null(_setting.WarningMessages);
        }

        [Test]
        public void PostQueryCustomerSetting_CustomerIsMember_ShowMemberSpecificUnlimitedBetSettingNote()
        {
            // Arrange.
            _query.CustomerLevel = Level.Member;

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            var messageContainer = _setting.WarningMessages as UpdateSettingResult;

            Assert.IsTrue(messageContainer?.SubResults.Any(subResult => subResult.Message == Resources.MemberNote));
        }

        [Test]
        public void PostQueryCustomerSetting_CustomerIsSuper_ShowSMASpecificUnlimitedBetSettingNote()
        {
            // Arrange.
            _query.CustomerLevel = Level.Super;

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            var messageContainer = _setting.WarningMessages as UpdateSettingResult;

            Assert.IsTrue(messageContainer?.SubResults.Any(subResult => subResult.Message == Resources.SMANote));
        }

        [Test]
        public void PostQueryCustomerSetting_CustomerIsMaster_ShowSMASpecificUnlimitedBetSettingNote()
        {
            // Arrange.
            _query.CustomerLevel = Level.Master;

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            var messageContainer = _setting.WarningMessages as UpdateSettingResult;

            Assert.IsTrue(messageContainer?.SubResults.Any(subResult => subResult.Message == Resources.SMANote));
        }

        [Test]
        public void PostQueryCustomerSetting_CustomerIsAgent_ShowSMASpecificUnlimitedBetSettingNote()
        {
            // Arrange.
            _query.CustomerLevel = Level.Agent;

            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            var messageContainer = _setting.WarningMessages as UpdateSettingResult;

            Assert.IsTrue(messageContainer?.SubResults.Any(subResult => subResult.Message == Resources.SMANote));
        }
    }
}