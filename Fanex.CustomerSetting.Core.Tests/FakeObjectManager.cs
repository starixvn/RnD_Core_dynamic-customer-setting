namespace Fanex.CustomerSetting.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.CustomerSetting.Core.Dtos.Params;
    using Fanex.CustomerSetting.Core.Enumerations;
    using Fanex.CustomerSetting.Core.Models;
    using Fanex.CustomerSetting.Core.Tests.Models;

    public static class FakeObjectManager
    {
        public static SettingCommand CreateSettingCommandFakeObject(
            int customerId = 1, 
            CustomerLevel customerLevel = CustomerLevel.Master,
            byte productId = 1,
            CustomerLevel? uplineLevel = CustomerLevel.Super)
        {
            return new SettingCommand
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
                        FakeObjectManager.CreateSettingTypeFakeObject(1, 1),
                        FakeObjectManager.CreateSettingTypeFakeObject(2, 1),
                        FakeObjectManager.CreateSettingTypeFakeObject(3, 1),
                        FakeObjectManager.CreateSettingTypeFakeObject(4, 1)
                    }
                },
                UplineLevel = uplineLevel
            };
        }

        public static SettingType CreateSettingTypeFakeObject(byte settingTypeId, byte settingTemplate = 1)
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

        public static MultipleSettingCommand CreateMultipleSettingCommandFakeObject(
            int customerId = 1,
            CustomerLevel customerLevel = CustomerLevel.Master,
            byte productId = 1,
            CustomerLevel? uplineLevel = CustomerLevel.Super)
        {
            return new MultipleSettingCommand
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
                        FakeObjectManager.CreateSettingTypeFakeObject(1, 1),
                        FakeObjectManager.CreateSettingTypeFakeObject(2, 1),
                        FakeObjectManager.CreateSettingTypeFakeObject(3, 1),
                        FakeObjectManager.CreateSettingTypeFakeObject(4, 1)
                    }
                },
                UplineLevel = uplineLevel
            };
        }
    }
}
