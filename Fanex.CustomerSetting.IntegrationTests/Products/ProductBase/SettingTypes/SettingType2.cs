namespace Fanex.CustomerSetting.IntegrationTests.Products.ProductBase.SettingTypes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.CustomerSetting.IntegrationTests.Models;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.SettingType2.Template1.Models;
    using NUnit.Framework;

    public static class SettingType2
    {
        public static void AssertGotBetLimit(ISetting setting)
        {
            var settingType = setting.SettingTypes.Where(st => st.Id == SettingTypeInfo.BetLimitSettingTypeId).FirstOrDefault();

            var betLimitItem = settingType.SettingItems.ElementAt(0) as BetLimitItem;

            Assert.IsNotNull(betLimitItem);
            Assert.GreaterOrEqual(betLimitItem.MinBet, 0);
            Assert.GreaterOrEqual(betLimitItem.MaxBet, 0);
            Assert.GreaterOrEqual(betLimitItem.MaxPerMatch, 0);
        }

        public static ModifySettingTypeInfo ModifySuperBetLimit(ISetting setting)
        {
            var settingType = setting.SettingTypes.Where(st => st.Id == SettingTypeInfo.BetLimitSettingTypeId).FirstOrDefault();

            var superBetLimitItem = settingType.SettingItems.ElementAt(0) as BetLimitItem;

            superBetLimitItem.MinBet = superBetLimitItem.MinBet == 45 ? 44 : 45;
            superBetLimitItem.MaxBet = superBetLimitItem.MaxBet == 550 ? 551 : 550;
            superBetLimitItem.MaxPerMatch = superBetLimitItem.MaxPerMatch == 550550 ? 550551 : 550550;

            return new ModifySettingTypeInfo
            {
                SettingItem = superBetLimitItem,
                SettingTypeId = SettingTypeInfo.BetLimitSettingTypeId
            };
        }

        public static ModifySettingTypeInfo ModifyMasterBetLimit(ISetting setting)
        {
            var settingType = setting.SettingTypes.Where(st => st.Id == SettingTypeInfo.BetLimitSettingTypeId).FirstOrDefault();

            var masterBetLimitItem = settingType.SettingItems.ElementAt(0) as BetLimitItem;

            masterBetLimitItem.MinBet = masterBetLimitItem.MinBet == 50 ? 51 : 50;
            masterBetLimitItem.MaxBet = masterBetLimitItem.MaxBet == 500 ? 501 : 500;
            masterBetLimitItem.MaxPerMatch = masterBetLimitItem.MaxPerMatch == 450550 ? 450551 : 450550;

            return new ModifySettingTypeInfo
            {
                SettingItem = masterBetLimitItem,
                SettingTypeId = SettingTypeInfo.BetLimitSettingTypeId
            };
        }

        public static ModifySettingTypeInfo ModifyAgentBetLimit(ISetting setting)
        {
            var settingType = setting.SettingTypes.Where(st => st.Id == SettingTypeInfo.BetLimitSettingTypeId).FirstOrDefault();

            var agentBetLimitItem = settingType.SettingItems.ElementAt(0) as BetLimitItem;

            agentBetLimitItem.MinBet = agentBetLimitItem.MinBet == 60 ? 61 : 60;
            agentBetLimitItem.MaxBet = agentBetLimitItem.MaxBet == 400 ? 401 : 400;
            agentBetLimitItem.MaxPerMatch = agentBetLimitItem.MaxPerMatch == 350550 ? 350551 : 350550;

            return new ModifySettingTypeInfo
            {
                SettingItem = agentBetLimitItem,
                SettingTypeId = SettingTypeInfo.BetLimitSettingTypeId
            };
        }

        public static ModifySettingTypeInfo ModifyMemberBetLimit(ISetting setting)
        {
            var settingType = setting.SettingTypes.Where(st => st.Id == SettingTypeInfo.BetLimitSettingTypeId).FirstOrDefault();

            var memberBetLimitItem = settingType.SettingItems.ElementAt(0) as BetLimitItem;

            memberBetLimitItem.MinBet = memberBetLimitItem.MinBet == 70 ? 71 : 70;
            memberBetLimitItem.MaxBet = memberBetLimitItem.MaxBet == 300 ? 301 : 300;
            memberBetLimitItem.MaxPerMatch = memberBetLimitItem.MaxPerMatch == 250550 ? 250551 : 250550;

            return new ModifySettingTypeInfo
            {
                SettingItem = memberBetLimitItem,
                SettingTypeId = SettingTypeInfo.BetLimitSettingTypeId
            };
        }

        public static void AssertUpdatedBetLimit(ISettingItem betLimitItem, ISetting setting)
        {
            var settingType = setting.SettingTypes.Where(st => st.Id == SettingTypeInfo.BetLimitSettingTypeId).FirstOrDefault();

            var newBetLimitItem = settingType.SettingItems.ElementAt(0) as BetLimitItem;

            var expectedBetLimitItem = betLimitItem as BetLimitItem;

            Assert.AreEqual(expectedBetLimitItem.MinBet, newBetLimitItem.MinBet);
            Assert.AreEqual(expectedBetLimitItem.MaxBet, newBetLimitItem.MaxBet);
            Assert.AreEqual(expectedBetLimitItem.MaxPerMatch, newBetLimitItem.MaxPerMatch);
        }
    }
}
