namespace Fanex.CustomerSetting.Tasks.Product21.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CustomerSetting.Services;
    using Models;
    using Models.Enumerations;
    using Models.Requests;
    using NSubstitute;
    using NUnit.Framework;

    public class RenameBetTypesTaskTests
    {
        private readonly short[] _referencingBaccaratBetTypeIds = { 2401, 2403, 2404, 2405, 2406 };

        private RenameBetTypesTask _task;
        private IResourceService _resourceService;
        private ISettingContext _settingContext;

        [SetUp]
        public void SetUp()
        {
            _resourceService = Substitute.For<IResourceService>();
            _task = new RenameBetTypesTask(_resourceService);
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
            var settingType = CreateSampleSettingType4Data();

            _resourceService.GetBetTypeName(Arg.Any<int>(), Arg.Any<string>()).Returns(callInfo => $"Bet type {callInfo.ArgAt<int>(0)}");

            // Act.
            _task.Invoke(_settingContext, query, settingType);

            // Assert.
            var expectedBetTypeNames = _referencingBaccaratBetTypeIds.Select(betTypeId => $"Bet type {betTypeId}").ToArray();
            var expectedDisplayedName = string.Format(Resources.ReferencingBaccaratBetTypes, expectedBetTypeNames);
            var assertedSettingItem = settingType.SettingItems.FirstOrDefault(
                settingItem => (settingItem as SettingItemDefault).BetTypeId == 2401) as SettingItemDefault;

            Assert.AreEqual(expectedDisplayedName, assertedSettingItem.BetTypeName);
        }

        [Test]
        public void PostQueryCustomerSettingType_SettingTypeIs6_RenameBetType2401()
        {
            // Arrange.
            var query = Substitute.For<CustomerSettingQuery>(1, Level.Agent, (byte)21, null);
            var settingType = CreateSampleSettingType7Data();

            _resourceService.GetBetTypeName(Arg.Any<int>(), Arg.Any<string>()).Returns(callInfo => $"Bet type {callInfo.ArgAt<int>(0)}");

            // Act.
            _task.Invoke(_settingContext, query, settingType);

            // Assert.
            var expectedBetTypeNames = _referencingBaccaratBetTypeIds.Select(betTypeId => $"Bet type {betTypeId}").ToArray();
            var expectedDisplayedName = string.Format(Resources.ReferencingBaccaratBetTypes, expectedBetTypeNames);
            var assertedSettingItem = settingType.SettingItems.FirstOrDefault(
                settingItem => (settingItem as SettingItemDefault).BetTypeId == 2401) as SettingItemDefault;

            Assert.AreEqual(expectedDisplayedName, assertedSettingItem.BetTypeName);
        }

        [Test]
        public void PostQueryCustomerSettingType_SettingTypeIs4_BetType2402NameStaysTheSame()
        {
            // Arrange.
            var query = Substitute.For<CustomerSettingQuery>(1, Level.Agent, (byte)21, null);
            var settingType = CreateSampleSettingType4Data();

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
            var settingType = CreateSampleSettingType7Data();

            // Act.
            _task.Invoke(_settingContext, query, settingType);

            // Assert.
            var assertedSettingItem = settingType.SettingItems.FirstOrDefault(
                settingItem => (settingItem as SettingItemDefault).BetTypeId == 2402) as SettingItemDefault;

            Assert.AreEqual(Resources.OtherDisplayingTextForVipBaccarat, assertedSettingItem.BetTypeName);
        }

        private SettingType CreateSampleSettingType4Data()
        {
            var settingType13 = Substitute.For<SettingType>();
            var betType2401 = Substitute.For<SettingItemDefault>();
            var betType2402 = Substitute.For<SettingItemDefault>();
            var settingItems = new List<SettingItemDefault>();

            settingType13.Id = 13;
            settingType13.SettingItems = settingItems;
            betType2401.BetTypeId = 2401;
            betType2402.BetTypeId = 2402;
            betType2402.BetTypeName = "Bet Type 2402";
            settingItems.Add(betType2401);
            settingItems.Add(betType2402);

            return settingType13;
        }

        private SettingType CreateSampleSettingType7Data()
        {
            var settingType7 = Substitute.For<SettingType>();
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