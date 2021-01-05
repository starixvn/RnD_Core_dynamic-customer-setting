namespace Fanex.CustomerSetting.Tests
{
    using System.Collections.Generic;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using Models;

    public static class InstanceFactory
    {
        public static UpdateCustomerSettingCommand CreateUpdateSettingParam(
            int customerId = 1,
            Level customerLevel = null,
            byte productId = 1,
            Level uplineLevel = null)
        {
            customerLevel = customerLevel ?? Level.Master;
            var setting = CreateCustomerSetting(customerId, customerLevel, productId);
            return new UpdateCustomerSettingCommand(setting, uplineLevel);
        }

        public static Setting CreateCustomerSetting(
            int customerId = 1,
            Level customerLevel = null,
            byte productId = 1)
        {
            customerLevel = customerLevel ?? Level.Master;

            var settingTypes = new List<SettingType>
                    {
                        InstanceFactory.CreateCustomerSettingType(1),
                        InstanceFactory.CreateCustomerSettingType(2),
                        InstanceFactory.CreateCustomerSettingType(3),
                        InstanceFactory.CreateCustomerSettingType(4)
                    };
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
                        InstanceFactory.CreateCustomerSettingType(1),
                        InstanceFactory.CreateCustomerSettingType(2),
                        InstanceFactory.CreateCustomerSettingType(3),
                        InstanceFactory.CreateCustomerSettingType(4)
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
            byte settingStatus = 0,
            Level uplineLevel = null)
        {
            customerLevel = customerLevel ?? Level.Member;

            if (customerLevel.IsMemberLevel())
            {
                uplineLevel = uplineLevel ?? Level.Master;
            }

            return new CustomerSettingQuery(customerId, customerLevel, productId, settingStatus, uplineLevel);
        }

        internal static Product CreateProduct(byte productId = 1)
        {
            return new Product
            {
                Id = productId,
                SettingTypes = new List<SettingType>
                {
                    InstanceFactory.CreateProductSettingType(1),
                    InstanceFactory.CreateProductSettingType(2),
                    InstanceFactory.CreateProductSettingType(3),
                    InstanceFactory.CreateProductSettingType(4)
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