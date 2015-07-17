namespace Fanex.CustomerSetting.Tests
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Tests.Models;

    public static class InstanceFactory
    {
        public static UpdatingSettingParam CreateUpdatingSettingParam(
            int customerId = 1,
            Level customerLevel = null,
            byte productId = 1,
            Level uplineLevel = null)
        {
            customerLevel = customerLevel ?? Level.Master;
            var setting = CreateCustomerSetting(customerId, customerLevel, productId);
            return new UpdatingSettingParam(setting, uplineLevel);
        }

        public static Setting CreateCustomerSetting(
            int customerId = 1,
            Level customerLevel = null,
            byte productId = 1)
        {
            customerLevel = customerLevel ?? Level.Master;

            var settingTypes = new List<SettingType>
                    {
                        InstanceFactory.CreateCustomerSettingType(1, 1),
                        InstanceFactory.CreateCustomerSettingType(2, 1),
                        InstanceFactory.CreateCustomerSettingType(3, 1),
                        InstanceFactory.CreateCustomerSettingType(4, 1)
                    };
            return new Setting(customerId, customerLevel, productId, settingTypes);
        }

        public static SettingType CreateCustomerSettingType(byte settingTypeId, byte settingTemplate = 1)
        {
            return new SettingType
            {
                Id = settingTypeId,
                SettingTemplate = settingTemplate,
                SettingLevel = 1,
                SettingItems = new List<SettingItemStub>
                {
                   new SettingItemStub
                   {
                       SportTypeId = 1
                   }
                }
            };
        }

        public static SettingType CreateProductSettingType(byte settingTypeId, byte settingTemplate = 1)
        {
            return new SettingType
            {
                Id = settingTypeId,
                SettingTemplate = settingTemplate,
                SettingLevel = 1,
                Name = "SettingTypeName"
            };
        }

        public static UpdatingMultipleSettingParam CreateUpdatingMultipleSettingParam(
            int customerId = 1,
            Level customerLevel = null,
            byte productId = 1,
            Level uplineLevel = null)
        {
            customerLevel = customerLevel ?? Level.Master;

            var settingTypes = new List<SettingType>
                    {
                        InstanceFactory.CreateCustomerSettingType(1, 1),
                        InstanceFactory.CreateCustomerSettingType(2, 1),
                        InstanceFactory.CreateCustomerSettingType(3, 1),
                        InstanceFactory.CreateCustomerSettingType(4, 1)
                    };

            var updatingMultipleParam = new UpdatingMultipleSettingParam(
                new Setting(customerId, customerLevel, productId, settingTypes),
                new List<int> { customerId, 2 },
                new List<string> { "Username1", "Username2" },
                uplineLevel);

            return updatingMultipleParam;
        }

        public static QuerySettingParam CreateQuerySettingParam(
            int customerId = 1,
            Level customerLevel = null,
            byte productId = 1,
            Level uplineLevel = null)
        {
            customerLevel = customerLevel ?? Level.Member;
            uplineLevel = uplineLevel ?? Level.Master;
            return new QuerySettingParam(customerId, customerLevel, productId, uplineLevel);
        }

        public static Product CreateProduct(byte productId = 1)
        {
            return new Product
            {
                Id = 1,
                SettingTypes = new List<SettingType>
                {
                    InstanceFactory.CreateProductSettingType(1, 1),
                    InstanceFactory.CreateProductSettingType(2, 1),
                    InstanceFactory.CreateProductSettingType(3, 1),
                    InstanceFactory.CreateProductSettingType(4, 1)
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