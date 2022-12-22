namespace Fanex.CustomerSetting.Tests.Models.Requests
{
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Requests;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Script.Serialization;

    [TestFixture]
    public class UpdateCustomerSettingCommandTests
    {
        private UpdateCustomerSettingCommand command;
        private readonly byte[] SettingTypes = { 1, 3, 18 };
        private readonly JavaScriptSerializer serializer = new JavaScriptSerializer();

        [SetUp]
        public void SetUp()
        {
            command = CreateUpdateSettingCommand();
        }

        [Test]
        public void Setting_SetToNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => command.Setting = null);
        }

        [Test]
        public void Setting_SetToAnotherSetting_Success()
        {
            var expected = CreateCustomerSetting(2, Level.Super, 2, new byte[] { 4, 5, 6 });
            command.Setting = expected;

            var actual = command.Setting;

            Assert.AreEqual(serializer.Serialize(expected), serializer.Serialize(actual));
        }

        [Test]
        public void UplineLevel_SetUplineLevelToNullWhenIsMember_ThrowArgumentNullException()
        {
            command = CreateUpdateSettingCommand(1, Level.Member, 1, Level.Agent);
            Assert.Throws<ArgumentNullException>(() => command.UplineLevel = null);
        }

        [Test]
        public void UplineLevel_SetUplineLevelToCompanyOrMember_ThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => command.UplineLevel = Level.Member);
            Assert.Throws<ArgumentOutOfRangeException>(() => command.UplineLevel = Level.Company);
        }

        [Test]
        public void UplineLevel_SetUplineLevelToAnyWhenIsNotMember_ThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => command.UplineLevel = Level.Super);
            Assert.Throws<ArgumentOutOfRangeException>(() => command.UplineLevel = Level.Master);
            Assert.Throws<ArgumentOutOfRangeException>(() => command.UplineLevel = Level.Agent);
        }

        private UpdateCustomerSettingCommand CreateUpdateSettingCommand(
            int customerId = 1,
            Level customerLevel = null,
            byte productId = 1,
            Level uplineLevel = null,
            byte settingStatus = 0,
            byte[] settingTypes = null)
        {
            var settings = CreateCustomerSetting(
                customerId,
                customerLevel ?? Level.Master,
                productId,
                settingTypes ?? SettingTypes);

            return new UpdateCustomerSettingCommand(
                settings,
                settingStatus,
                uplineLevel);
        }

        private Setting CreateCustomerSetting(
            int customerId = 1,
            Level customerLevel = null,
            byte productId = 1,
            params byte[] settingTypeIds)
        {
            var settingTypes = settingTypeIds.Select(settingTypeId => CreateSettingType(settingTypeId));

            return new Setting(
                customerId,
                customerLevel ?? Level.Master,
                new CustomerInfo(),
                productId,
                string.Empty,
                settingTypes);
        }

        private SettingType CreateSettingType(byte settingTypeId)
        {
            return new SettingType
            {
                Id = settingTypeId,
                SettingLevel = 1,
                DisplayMode = 1,
                SettingItems = new List<ISettingItem>
                {
                   new SettingItemDefault
                   {
                       SportTypeId = 1,
                       BetTypeId = 1
                   }
                }
            };
        }
    }
}
