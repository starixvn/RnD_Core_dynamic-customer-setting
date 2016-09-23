namespace Fanex.CustomerSetting.Tasks.Product21.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Models;
    using Models.Enumerations;
    using Models.Requests;
    using Models.Results;
    using NSubstitute;
    using NUnit.Framework;

    public class RenameBetTypesTaskTests
    {
        private RenameBetTypesTask _task;
        private ISettingContext _settingContext;

        [SetUp]
        public void SetUp()
        {
            _task = new RenameBetTypesTask();
            _settingContext = Substitute.For<ISettingContext>();
            _settingContext.Actor = Substitute.For<User>();
            _settingContext.Actor.Level = Level.Master;
        }

        [Test]
        public void PostQueryCustomerSettingType_ParametersAreNull_ThrowException()
        {
            // Arrange.
            CustomerSettingQuery query = null;
            SettingType settingType = null;

            // Act.
            TestDelegate invokeDelegate = () => _task.Invoke(_settingContext, query, settingType);

            // Assert.
            Assert.Throws<ArgumentNullException>(invokeDelegate);
        }

        [Test]
        public void PostQueryCustomerSettingType_SettingTypeIs4_RenameBetType2401()
        {
            // Arrange.
            var query = Substitute.For<CustomerSettingQuery>(1, Level.Agent, (byte)21, null);
            SettingType settingType = CreateSampleSettingType4Data();

            // Act.
            _task.Invoke(_settingContext, query, settingType);

            // Assert.
            var assertedSettingItem = settingType.SettingItems.FirstOrDefault(
                settingItem => (settingItem as SettingItemDefault).BetTypeId == 2401) as SettingItemDefault;

            Assert.AreEqual(Resources.OtherDisplayingTextForBaccarat, assertedSettingItem.BetTypeName);
        }

        [Test]
        public void PostQueryCustomerSettingType_SettingTypeIs6_RenameBetType2401()
        {
            // Arrange.
            var query = Substitute.For<CustomerSettingQuery>(1, Level.Agent, (byte)21, null);
            SettingType settingType = CreateSampleSettingType7Data();

            // Act.
            _task.Invoke(_settingContext, query, settingType);

            // Assert.
            var assertedSettingItem = settingType.SettingItems.FirstOrDefault(
                settingItem => (settingItem as SettingItemDefault).BetTypeId == 2401) as SettingItemDefault;

            Assert.AreEqual(Resources.OtherDisplayingTextForBaccarat, assertedSettingItem.BetTypeName);
        }

        [Test]
        public void PostQueryCustomerSettingType_SettingTypeIs4_BetType2402NameStaysTheSame()
        {
            // Arrange.
            var query = Substitute.For<CustomerSettingQuery>(1, Level.Agent, (byte)21, null);
            SettingType settingType = CreateSampleSettingType4Data();

            // Act.
            _task.Invoke(_settingContext, query, settingType);

            // Assert.
            var assertedSettingItem = settingType.SettingItems.FirstOrDefault(
                settingItem => (settingItem as SettingItemDefault).BetTypeId == 2402) as SettingItemDefault;

            Assert.AreEqual("Bet Type 2402", assertedSettingItem.BetTypeName);
        }

        [Test]
        public void PostQueryCustomerSettingType_SettingTypeIs6_RenameBetType2402()
        {
            // Arrange.
            var query = Substitute.For<CustomerSettingQuery>(1, Level.Agent, (byte)21, null);
            SettingType settingType = CreateSampleSettingType7Data();

            // Act.
            _task.Invoke(_settingContext, query, settingType);

            // Assert.
            var assertedSettingItem = settingType.SettingItems.FirstOrDefault(
                settingItem => (settingItem as SettingItemDefault).BetTypeId == 2402) as SettingItemDefault;

            Assert.AreEqual(Resources.OtherDisplayingTextForVipBaccarat, assertedSettingItem.BetTypeName);
        }

        private ISetting CreateSampleSettingData()
        {
            ISetting setting = Substitute.For<ISetting>();
            SettingType settingType1 = Substitute.For<SettingType>();
            SettingType settingType3 = Substitute.For<SettingType>();
            SettingType settingType4 = Substitute.For<SettingType>();
            SettingType settingType6 = Substitute.For<SettingType>();
            SettingType settingType7 = Substitute.For<SettingType>();

            settingType1.Id = 1;
            settingType3.Id = 3;
            settingType4.Id = 4;
            settingType6.Id = 6;
            settingType7.Id = 7;

            setting.SettingTypes = new List<SettingType>
            {
                settingType1,
                settingType3,
                settingType4,
                settingType6,
                settingType7
            };
            setting.WarningMessage = null;
            setting.CustomerLevel = Level.Agent;

            return setting;
        }

        private SettingType CreateSampleSettingType4Data()
        {
            SettingType settingType4 = Substitute.For<SettingType>();
            var betType2401 = Substitute.For<SettingItemDefault>();
            var betType2402 = Substitute.For<SettingItemDefault>();
            var settingItems = new List<SettingItemDefault>();

            settingType4.Id = 4;
            settingType4.SettingItems = settingItems;
            betType2401.BetTypeId = 2401;
            betType2402.BetTypeId = 2402;
            betType2402.BetTypeName = "Bet Type 2402";
            settingItems.Add(betType2401);
            settingItems.Add(betType2402);

            return settingType4;
        }

        private SettingType CreateSampleSettingType7Data()
        {
            SettingType settingType7 = Substitute.For<SettingType>();
            var betType2401 = Substitute.For<SettingItemDefault>();
            var betType2402 = Substitute.For<SettingItemDefault>();
            var settingItems = new List<SettingItemDefault>();

            settingType7.Id = 7;
            settingType7.SettingItems = settingItems;
            betType2401.BetTypeId = 2401;
            betType2402.BetTypeId = 2402;
            betType2402.BetTypeName = "Bet Type 2402";
            settingItems.Add(betType2401);
            settingItems.Add(betType2402);

            return settingType7;
        }
    }
}