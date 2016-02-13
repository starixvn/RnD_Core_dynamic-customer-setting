namespace Fanex.CustomerSetting.Casino.Tests
{
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Requests;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class StubHelper
    {
        private static readonly byte[] DefaultSettingTypeValues = new byte[] { 1, 2, 3, 4 };
        public static UpdateCustomerSettingCommand CreateUpdateSettingParam(
            int customerId = 1,
            Level customerLevel = null,
            byte productId = 1,
            Level uplineLevel = null, 
            params byte[] settingTypeValues)
        {
            customerLevel = customerLevel ?? Level.Master;
            var setting = (settingTypeValues != null && settingTypeValues.Any()) 
                ? CreateCustomerSetting(customerId, customerLevel, productId, settingTypeValues)
                : CreateCustomerSetting(customerId, customerLevel, productId, DefaultSettingTypeValues);
            return new UpdateCustomerSettingCommand(setting, uplineLevel);
        }
        public static Setting CreateCustomerSetting(
            int customerId = 1,
            Level customerLevel = null,
            byte productId = 1, 
            params byte[] settingTypeValues)
        {
            customerLevel = customerLevel ?? Level.Master;

            var settingTypes = new List<SettingType>();

            settingTypeValues.ToList().ForEach(
                settingTypeValue =>
                settingTypes.Add(StubHelper.CreateCustomerSettingType(settingTypeValue))
                );

            return new Setting(customerId, customerLevel, productId, settingTypes);
        }

        public static SettingType CreateCustomerSettingType(byte settingTypeId)
        {
            return new SettingType
            {
                Id = settingTypeId,
                SettingLevel = 1,
                DisplayMode = 1,
                SettingItems = new List<SettingItemStub>
                {
                   new SettingItemStub
                   {
                       SportTypeId = 1
                   }
                }
            };
        }

        public static SettingType CreateProductSettingType(byte settingTypeId)
        {
            return new SettingType
            {
                Id = settingTypeId,
                SettingLevel = 1,
                Name = "SettingTypeName"
            };
        }

        public static MultipleUpdateCustomerSettingCommand CreateUpdateMultipleSettingParam(
            int customerId = 1,
            Level customerLevel = null,
            byte productId = 1,
            Level uplineLevel = null)
        {
            customerLevel = customerLevel ?? Level.Master;

            var settingTypes = new List<SettingType>
                    {
                        StubHelper.CreateCustomerSettingType(1),
                        StubHelper.CreateCustomerSettingType(2),
                        StubHelper.CreateCustomerSettingType(3),
                        StubHelper.CreateCustomerSettingType(4)
                    };

            var updateMultipleParam = new MultipleUpdateCustomerSettingCommand(
                new Setting(customerId, customerLevel, productId, settingTypes),
                new List<int> { customerId, 2 },
                new List<string> { "Username1", "Username2" },
                uplineLevel);

            return updateMultipleParam;
        }

        public static CustomerSettingQuery CreateQuerySettingParam(
            int customerId = 1,
            Level customerLevel = null,
            byte productId = 1,
            Level uplineLevel = null)
        {
            customerLevel = customerLevel ?? Level.Member;

            if (customerLevel.IsMemberLevel())
            {
                uplineLevel = uplineLevel ?? Level.Master;
            }

            return new CustomerSettingQuery(customerId, customerLevel, productId, uplineLevel);
        }

        internal static Product CreateProduct(byte productId = 1)
        {
            return new Product
            {
                Id = productId,
                SettingTypes = new List<SettingType>
                {
                    StubHelper.CreateProductSettingType(1),
                    StubHelper.CreateProductSettingType(2),
                    StubHelper.CreateProductSettingType(3),
                    StubHelper.CreateProductSettingType(4)
                }
            };
        }

        public static User CreateUser(
            int userId = 1,
            string userName = "Username",
            Level userLevel = null,
            int subAccountId = 0)
        {
            return new User
            {
                Id = userId,
                Name = userName,
                Level = userLevel ?? Level.Company,
                SubAccountId = subAccountId
            };
        }
    }
}