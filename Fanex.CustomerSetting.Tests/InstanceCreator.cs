namespace Fanex.CustomerSetting.Tests
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Tests.Models;

    public static class InstanceCreator
    {
        public static UpdatingSettingParam CreateUpdatingSettingParam(
            int customerId = 1,
            CustomerLevel customerLevel = CustomerLevel.Master,
            byte productId = 1,
            CustomerLevel? uplineLevel = null)
        {
            return new UpdatingSettingParam
            {
                Setting = new Setting
                {
                    ProductId = productId,
                    CustomerId = customerId,
                    CustomerLevel = customerLevel,
                    SettingTypes = new List<SettingType>
                    {
                        InstanceCreator.CreateCustomerSettingType(1, 1),
                        InstanceCreator.CreateCustomerSettingType(2, 1),
                        InstanceCreator.CreateCustomerSettingType(3, 1),
                        InstanceCreator.CreateCustomerSettingType(4, 1)
                    }
                },
                UplineLevel = uplineLevel
            };
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
                SettingLevel = 1
            };
        }

        public static UpdatingMultipleSettingParam CreateUpdatingMultipleSettingParam(
            int customerId = 1,
            CustomerLevel customerLevel = CustomerLevel.Master,
            byte productId = 1,
            CustomerLevel? uplineLevel = null)
        {
            return new UpdatingMultipleSettingParam
            {
                CustomerIds = new List<int> { customerId, 2 },
                CustomerNames = new List<string> { "Username1", "Username2" },
                Setting = new Setting
                {
                    ProductId = productId,
                    CustomerId = customerId,
                    CustomerLevel = customerLevel,
                    SettingTypes = new List<SettingType>
                    {
                        InstanceCreator.CreateCustomerSettingType(1, 1),
                        InstanceCreator.CreateCustomerSettingType(2, 1),
                        InstanceCreator.CreateCustomerSettingType(3, 1),
                        InstanceCreator.CreateCustomerSettingType(4, 1)
                    }
                },
                UplineLevel = uplineLevel
            };
        }

        public static QuerySettingParam CreateQuerySettingParam(
            int customerId = 1,
            CustomerLevel customerLevel = CustomerLevel.Member,
            byte productId = 1,
            CustomerLevel? uplineLevel = CustomerLevel.Master)
        {
            return new QuerySettingParam
            {
                CustomerId = customerId,
                CustomerLevel = customerLevel,
                ProductId = productId,
                UplineLevel = uplineLevel
            };
        }

        public static Product CreateProduct(byte productId = 1)
        {
            return new Product
            {
                Id = 1,
                SettingTypes = new List<SettingType>
                {
                    InstanceCreator.CreateProductSettingType(1, 1),
                    InstanceCreator.CreateProductSettingType(2, 1),
                    InstanceCreator.CreateProductSettingType(3, 1),
                    InstanceCreator.CreateProductSettingType(4, 1)
                }
            };
        }
    }
}