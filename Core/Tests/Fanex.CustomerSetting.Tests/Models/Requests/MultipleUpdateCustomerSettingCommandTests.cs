namespace Fanex.CustomerSetting.Tests.Models.Requests
{
    using Fanex.CustomerSetting.Casino.Tests;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Requests;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Script.Serialization;

    [TestFixture]
    public class MultipleUpdateCustomerSettingCommandTests
    {
        private readonly byte[] SettingTypes = { 1, 3, 18 };
        private MultipleUpdateCustomerSettingCommand command;
        private readonly JavaScriptSerializer serializer = new JavaScriptSerializer();

        [SetUp]
        public void SetUp()
        {
            string customerIds = "1,2,3,4,5";
            string customerNames = "cust1,cust2,cust3,cust4,cust5";
            byte productId = 1;
            command = CreateMultipleUpdateSettingCommand(
                customerIds: customerIds,
                customerNames: customerNames,
                productId: productId);
        }

        [Test]
        public void CustomerIds_SetToNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => command.CustomerIds = null);
        }

        [Test]
        public void CustomerNames_SetToNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => command.CustomerNames = null);
        }

        [Test]
        public void ValidateCustomerIdListWithCustomerNameList_LengthMissmatched_ThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => command.CustomerIds = new List<int> { 1, 2, 3, 4 });
        }

        [Test]
        public void ValidateCustomerIdListWithCustomerNameList_CustomerIdsLengthIsZero_ThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => command.CustomerIds = new List<int>());
        }

        [Test]
        public void ValidateCustomerIdListWithCustomerNameList_CustomerNamesLengthIsZero_ThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => command.CustomerNames = new List<string>());
        }

        [Test]
        public void GetUpdateCustomerSettingCommand_CustomerIdNotExisted_ThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => command.GetUpdateCustomerSettingCommand(10));
        }

        [Test]
        public void GetUpdateCustomerSettingCommand_CustomerIdExisted_ReturnExpectedResult()
        {
            var expected = new UpdateCustomerSettingCommand(command.Setting, 0, command.UplineLevel);
            expected.Setting.CustomerId = 3;
            expected.Setting.CustomerInfo.UserName = "cust3";

            var actual = command.GetUpdateCustomerSettingCommand(3);
            Assert.AreEqual(serializer.Serialize(expected), serializer.Serialize(actual));
        }
        
        private MultipleUpdateCustomerSettingCommand CreateMultipleUpdateSettingCommand(
            string customerIds = null,
            string customerNames = null,
            Level customerLevel = null,
            byte productId = 1,
            Level uplineLevel = null,
            byte[] settingTypes = null)
        {
            var listCustomerIds = customerIds.Split(',').Select(custId => int.Parse(custId.Trim())).ToList();
            var listCustomerNames = customerNames.Split(',').Select(userName => userName.Trim()).ToList();
            var settings = CreateCustomerSetting(
                listCustomerIds[0],
                customerLevel ?? Level.Master,
                productId,
                settingTypes ?? SettingTypes);

            return new MultipleUpdateCustomerSettingCommand(
                settings,
                listCustomerIds,
                listCustomerNames,
                uplineLevel
                );
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
