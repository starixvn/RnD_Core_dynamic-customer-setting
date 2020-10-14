namespace Fanex.CustomerSetting.NumberGames2.Tests.Services.Impl
{
    using System.Linq;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using CustomerSetting.NumberGames2.Resources;
    using CustomerSetting.Services;
    using CustomerSetting.Tests.Models;
    using Models;
    using NSubstitute;
    using NUnit.Framework;

    public class QueryNumberGames2SettingServiceTests
    {
        private ISettingContext _settingContext;
        private IResourceService _resourceService;
        private ITaskService _pluggableTaskService;
        private IQuerySettingService _querySettingService;
        private CustomerSettingQuery _query;

        [SetUp]
        public void Setup()
        {
            _settingContext = new SettingContextStub();
            _resourceService = Substitute.For<IResourceService>();
            _pluggableTaskService = Substitute.For<ITaskService>();
            _query = new CustomerSettingQuery(123, Level.Super, 23, 0, null);
        }

        [Test]
        public void GetCustomerSetting_SettingTypeHasBetSetting_ShowBetSettingReferenceNote()
        {
            // Arrange.
            _querySettingService = new QueryNumberGames2SettingServiceStub(
                _settingContext,
                _resourceService,
                _pluggableTaskService,
                new short[] { 9 });

            // Act.
            var setting = _querySettingService.GetCustomerSetting(_query);

            // Assert.
            Assert.IsNotNull(setting.WarningMessages);
        }

        [Test]
        public void GetCustomerSetting_SettingTypeHasBetSetting_WarningMessagesPropertyContainsExpectedBetSettingReferenceNote()
        {
            // Arrange.
            var betTypeIdsInTesting = new short[] { 2502, 2523, 2524, 2525, 2526, 2527, 2504, 2528 };

            _resourceService.GetBetTypeName(Arg.Any<int>(), Arg.Any<string>()).Returns(callInfo => $"Bet type {callInfo.ArgAt<int>(0)}");
            _querySettingService = new QueryNumberGames2SettingServiceStub(
                _settingContext,
                _resourceService,
                _pluggableTaskService,
                new short[] { 9 });

            // Act.
            var setting = _querySettingService.GetCustomerSetting(_query);
            var actualMessage = (setting.WarningMessages as UpdateSettingResult).SubResults[0]?.Message;

            // Assert.
            var expectedBetTypeNames = betTypeIdsInTesting.Select(betTypeId => $"Bet type {betTypeId}").ToArray();
            var expectedMessage = string.Format(Messages.BetSettingReferenceNote, expectedBetTypeNames);

            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [Test]
        public void GetCustomerSetting_SettingTypeDoesNotHaveBetSetting_DoNotShowBetSettingReferenceNote()
        {
            // Arrange.
            _querySettingService = new QueryNumberGames2SettingServiceStub(
                _settingContext,
                _resourceService,
                _pluggableTaskService,
                new short[] { 11, 3, 1 });

            // Act.
            var setting = _querySettingService.GetCustomerSetting(_query);

            // Assert.
            Assert.IsNull(setting.WarningMessages);
        }
    }
}