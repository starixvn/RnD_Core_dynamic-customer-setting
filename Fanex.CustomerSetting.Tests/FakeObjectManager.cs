namespace Fanex.CustomerSetting.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Tests.Models;

    public static class FakeObjectManager
    {
        public static UpdatingSettingParams CreateUpdatingSettingParams(
            int customerId = 1, 
            CustomerLevel customerLevel = CustomerLevel.Master,
            byte productId = 1,
            CustomerLevel? uplineLevel = CustomerLevel.Super)
        {
            return new UpdatingSettingParams
            {
                Setting = new Setting
                {
                    Product = new Product
                    {
                        Id = productId
                    },
                    Customer = new Customer
                    {
                        Id = customerId,
                        Level = customerLevel
                    },
                    SettingTypes = new List<SettingType>
                    {
                        FakeObjectManager.CreateSettingType(1, 1),
                        FakeObjectManager.CreateSettingType(2, 1),
                        FakeObjectManager.CreateSettingType(3, 1),
                        FakeObjectManager.CreateSettingType(4, 1)
                    }
                },
                UplineLevel = uplineLevel
            };
        }

        public static SettingType CreateSettingType(byte settingTypeId, byte settingTemplate = 1)
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

        public static UpdatingMultipleSettingParams CreateUpdatingMultipleSettingParams(
            int customerId = 1,
            CustomerLevel customerLevel = CustomerLevel.Master,
            byte productId = 1,
            CustomerLevel? uplineLevel = CustomerLevel.Super)
        {
            return new UpdatingMultipleSettingParams
            {
                Customers = new List<Customer>
                {
                    new Customer
                    {
                        Id = customerId
                    },
                    new Customer
                    {
                        Id = 2
                    }
                },
                Setting = new Setting
                {
                    Product = new Product
                    {
                        Id = productId
                    },
                    Customer = new Customer
                    {
                        Id = customerId,
                        Level = customerLevel
                    },
                    SettingTypes = new List<SettingType>
                    {
                        FakeObjectManager.CreateSettingType(1, 1),
                        FakeObjectManager.CreateSettingType(2, 1),
                        FakeObjectManager.CreateSettingType(3, 1),
                        FakeObjectManager.CreateSettingType(4, 1)
                    }
                },
                UplineLevel = uplineLevel
            };
        }

        public static QuerySettingParams CreateQuerySettingParams(
            int customerId = 1, 
            CustomerLevel customerLevel = CustomerLevel.Member,
            byte productId = 1,
            CustomerLevel? uplineLevel = CustomerLevel.Master)
        {
            return new QuerySettingParams
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
                    FakeObjectManager.CreateSettingType(1, 1),
                    FakeObjectManager.CreateSettingType(2, 1),
                    FakeObjectManager.CreateSettingType(3, 1),
                    FakeObjectManager.CreateSettingType(4, 1)
                }
            };
        }
    }
}
