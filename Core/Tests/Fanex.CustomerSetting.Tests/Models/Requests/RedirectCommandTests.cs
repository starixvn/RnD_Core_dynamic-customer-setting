namespace Fanex.CustomerSetting.Tests.Models.Requests
{
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Requests;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class RedirectCommandTests
    {
        private RedirectCommand command;

        [SetUp]
        public void SetUp()
        {
            command = new RedirectCommand(
                customerId: 123,
                customerLevel: Level.Member,
                productId: 1,
                uplineLevel: Level.Agent,
                settingTypeId: 1,
                actionId: 0,
                actionType: ActionType.Query,
                jsonParameters: "{\"sportTypeId\":1,\"betTypeId\":1}");
        }

        [Test]
        public void SettingTypeId_SetToLessThanOrEqualsZero_ThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => command.SettingTypeId = 0);
            Assert.Throws<ArgumentOutOfRangeException>(() => command.SettingTypeId = -1);
        }

        [Test]
        public void SettingTypeId_SetToAnyGreaterThanZero_Successs()
        {
            short expected = 1;

            command.SettingTypeId = expected;
            var actual = command.SettingTypeId;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ActionId_SetToAnyGreaterThanZero_Successs()
        {
            byte expected = 1;

            command.ActionId = expected;
            var actual = command.ActionId;
            Assert.AreEqual(expected, actual);
        }
    }
}
