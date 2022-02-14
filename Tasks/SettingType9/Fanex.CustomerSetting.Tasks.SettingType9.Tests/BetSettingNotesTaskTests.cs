namespace Fanex.CustomerSetting.Tasks.SettingType9.Tests
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
            byte settingStatus = 0;
            Level customerLevel = Level.Agent;

            _task = new BetSettingNotesTask();
            _settingContext = Substitute.For<ISettingContext>();
            _setting = new Setting();
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

        [TestCase(SystemType.Y7Alpha, 1)]
        [TestCase(SystemType.Y7InternalAlpha, 1)]
        [TestCase(SystemType.Y7ExternalNap, 1)]
        [TestCase(SystemType.Y7ExternalNapBetSetting, 1)]
        public void PostQueryCustomerSetting_SystemIs789Y_IsMember_DoNotShowUnlimitedBetSettingNote(byte system, int customerLevel)
        {
            // Arrange.
            _settingContext.System = system;
            _query.CustomerLevel = this.GetCustomerLevelEnum(customerLevel);
            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            Assert.IsFalse(((UpdateSettingResult)_setting.WarningMessages).SubResults.Any());
        }

        [TestCase(SystemType.Y7Alpha, 4)]
        [TestCase(SystemType.Y7InternalAlpha, 4)]
        [TestCase(SystemType.Y7ExternalNap, 4)]
        [TestCase(SystemType.Y7ExternalNapBetSetting, 4)]
        [TestCase(SystemType.Y7Alpha, 3)]
        [TestCase(SystemType.Y7InternalAlpha, 3)]
        [TestCase(SystemType.Y7ExternalNap, 3)]
        [TestCase(SystemType.Y7ExternalNapBetSetting, 3)]
        [TestCase(SystemType.Y7Alpha, 2)]
        [TestCase(SystemType.Y7InternalAlpha, 2)]
        [TestCase(SystemType.Y7ExternalNap, 2)]
        [TestCase(SystemType.Y7ExternalNapBetSetting, 2)]
        public void PostQueryCustomerSetting_SystemIs789Y_IsNotMember_ShowUnlimitedBetSettingNote(byte system, int customerLevel)
        {
            // Arrange.
            _settingContext.System = system;
            _query.CustomerLevel = this.GetCustomerLevelEnum(customerLevel);
            // Act.
            _task.Invoke(_settingContext, _query, _setting);

            // Assert.
            Assert.AreEqual(((UpdateSettingResult)_setting.WarningMessages).SubResults[0].Message, Resources.Y7SMANote);
        }

        private Level GetCustomerLevelEnum(int customerLevel)
        {
            switch (customerLevel)
            {
                case 1:
                    return Level.Member;
                case 2:
                    return Level.Agent;
                case 3:
                    return Level.Master;
                default:
                    return Level.Super;
            }
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