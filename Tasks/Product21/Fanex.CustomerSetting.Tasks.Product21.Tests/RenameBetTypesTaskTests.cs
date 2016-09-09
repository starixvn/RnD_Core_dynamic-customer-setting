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
        public void PostQueryCustomerSetting_ParametersAreNull_ThrowException()
        {
            // Arrange.
            CustomerSettingQuery query = null;
            ISetting setting = null;

            // Act.
            TestDelegate invokeDelegate = () => _task.Invoke(_settingContext, query, setting);

            // Assert.
            Assert.Throws<ArgumentNullException>(invokeDelegate);
        }

        [Test]
        public void PostQueryCustomerSetting_ParametersAreNotNull_WarningMessageIsNotNull()
        {
            // Arrange.
            var query = Substitute.For<CustomerSettingQuery>(1, Level.Agent, (byte)21, null);
            ISetting setting = CreateSampleSettingData();

            // Act.
            _task.Invoke(_settingContext, query, setting);

            // Assert.
            Assert.NotNull(setting.WarningMessage);
        }

        [Test]
        public void PostQueryCustomerSetting_ParametersAreNotNull_WarningMessageIsTypeOfUpdateSettingResult()
        {
            // Arrange.
            var query = Substitute.For<CustomerSettingQuery>(1, Level.Agent, (byte)21, null);
            ISetting setting = CreateSampleSettingData();

            // Act.
            _task.Invoke(_settingContext, query, setting);

            // Assert.
            Assert.IsInstanceOf<UpdateSettingResult>(setting.WarningMessage);
        }

        [Test]
        public void PostQueryCustomerSetting_ParametersAreNotNull_WarningMessageContainsExpectedContent()
        {
            // Arrange.
            CustomerSettingQuery query = Substitute.For<CustomerSettingQuery>(1, Level.Agent, (byte)21, null);
            ISetting setting = CreateSampleSettingData();

            // Act.
            _task.Invoke(_settingContext, query, setting);

            // Assert.
            var warningMessage = setting.WarningMessage as UpdateSettingResult;

            Assert.IsNotEmpty(warningMessage.SubResults);

            var subMessage = warningMessage.SubResults.ElementAt(0);

            Assert.AreEqual(Resources.MainInfoMessage, subMessage.Message);
            Assert.AreEqual(Resources.NormalGamesSubMessage, subMessage.SubMessages.ElementAt(0));
            Assert.AreEqual(Resources.VipBaccaratSubMessage, subMessage.SubMessages.ElementAt(1));
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

            Assert.AreEqual(Resources.NormalGames, assertedSettingItem.BetTypeName);
        }

        [Test]
        public void PostQueryCustomerSettingType_SettingTypeIs6_RenameBetType2401()
        {
            // Arrange.
            var query = Substitute.For<CustomerSettingQuery>(1, Level.Agent, (byte)21, null);
            SettingType settingType = CreateSampleSettingType4Data();

            // Act.
            _task.Invoke(_settingContext, query, settingType);

            // Assert.
            var assertedSettingItem = settingType.SettingItems.FirstOrDefault(
                settingItem => (settingItem as SettingItemDefault).BetTypeId == 2401) as SettingItemDefault;

            Assert.AreEqual(Resources.NormalGames, assertedSettingItem.BetTypeName);
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
            var settingItems = new List<SettingItemDefault>();

            settingType4.Id = 4;
            settingType4.SettingItems = settingItems;
            betType2401.BetTypeId = 2401;
            settingItems.Add(betType2401);

            return settingType4;
        }

        private SettingType CreateSampleSettingType6Data()
        {
            SettingType settingType6 = Substitute.For<SettingType>();
            var betType2401 = Substitute.For<SettingItemDefault>();
            var settingItems = new List<SettingItemDefault>();

            settingType6.Id = 6;
            settingType6.SettingItems = settingItems;
            betType2401.BetTypeId = 2401;
            settingItems.Add(betType2401);

            return settingType6;
        }
    }
}