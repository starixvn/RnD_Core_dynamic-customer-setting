namespace Fanex.CustomerSetting.NumberGames2.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;

    internal static class StubHelpers
    {
        public static readonly byte[] SettingTypes = { 11, 3, 9, 1 };

        public static UpdateCustomerSettingCommand CreateUpdateSettingCommand(
            int customerId = 1,
            Level customerLevel = null,
            byte productId = 1,
            Level uplineLevel = null,
            byte[] settingTypes = null)
        {
            var settings = CreateCustomerSetting(
                customerId,
                customerLevel ?? Level.Master,
                productId,
                settingTypes ?? SettingTypes);

            return new UpdateCustomerSettingCommand(settings, uplineLevel);
        }

        public static Setting CreateCustomerSetting(
            int customerId = 1,
            Level customerLevel = null,
            byte productId = 1,
            params byte[] settingTypeIds)
        {
            var settingTypes = settingTypeIds.Select(settingTypeId => CreateSettingType(settingTypeId));

            return new Setting(
                customerId,
                customerLevel ?? Level.Master,
                productId,
                settingTypes);
        }

        public static SettingType CreateSettingType(byte settingTypeId)
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