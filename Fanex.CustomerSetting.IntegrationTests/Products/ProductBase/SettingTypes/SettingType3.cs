namespace Fanex.CustomerSetting.IntegrationTests.Products.ProductBase.SettingTypes
{
    using System.Linq;
    using Fanex.CustomerSetting.IntegrationTests.Models;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.SettingType3.Template1.Models;
    using NUnit.Framework;

    public class SettingType3
    {
        public static void AssertGotCommission(ISetting setting)
        {
            var settingType = setting.SettingTypes.Where(st => st.Id == SettingTypeInfo.CommissionSettingTypeId).FirstOrDefault();

            var commissionItem = settingType.SettingItems.ElementAt(0) as CommissionItem;

            Assert.IsNotNull(commissionItem);
            Assert.GreaterOrEqual(commissionItem.CommRate, 0);
            Assert.LessOrEqual(commissionItem.CommRate, 0.01);
        }

        public static ModifySettingTypeInfo ModifySuperCommission(ISetting setting)
        {
            var settingType = setting.SettingTypes.Where(st => st.Id == SettingTypeInfo.CommissionSettingTypeId).FirstOrDefault();

            var superCommissionItem = settingType.SettingItems.ElementAt(0) as CommissionItem;

            superCommissionItem.CommRate = superCommissionItem.CommRate == 0.0095m ? 0.0096m : 0.0095m;

            return new ModifySettingTypeInfo
            {
                SettingItem = superCommissionItem,
                SettingTypeId = SettingTypeInfo.CommissionSettingTypeId
            };
        }

        public static ModifySettingTypeInfo ModifyMasterCommission(ISetting setting)
        {
            var settingType = setting.SettingTypes.Where(st => st.Id == SettingTypeInfo.CommissionSettingTypeId).FirstOrDefault();

            var masterCommissionItem = settingType.SettingItems.ElementAt(0) as CommissionItem;

            masterCommissionItem.CommRate = masterCommissionItem.CommRate == 0.0080m ? 0.0081m : 0.0080m;

            return new ModifySettingTypeInfo
            {
                SettingItem = masterCommissionItem,
                SettingTypeId = SettingTypeInfo.CommissionSettingTypeId
            };
        }

        public static ModifySettingTypeInfo ModifyAgentCommission(ISetting setting)
        {
            var settingType = setting.SettingTypes.Where(st => st.Id == SettingTypeInfo.CommissionSettingTypeId).FirstOrDefault();

            var agentCommissionItem = settingType.SettingItems.ElementAt(0) as CommissionItem;

            agentCommissionItem.CommRate = agentCommissionItem.CommRate == 0.0055m ? 0.0056m : 0.0055m;

            return new ModifySettingTypeInfo
            {
                SettingItem = agentCommissionItem,
                SettingTypeId = SettingTypeInfo.CommissionSettingTypeId
            };
        }

        public static ModifySettingTypeInfo ModifyMemberCommission(ISetting setting)
        {
            var settingType = setting.SettingTypes.Where(st => st.Id == SettingTypeInfo.CommissionSettingTypeId).FirstOrDefault();

            var memberCommissionItem = settingType.SettingItems.ElementAt(0) as CommissionItem;

            memberCommissionItem.CommRate = memberCommissionItem.CommRate == 0.0033m ? 0.0032m : 0.0033m;

            return new ModifySettingTypeInfo
            {
                SettingItem = memberCommissionItem,
                SettingTypeId = SettingTypeInfo.CommissionSettingTypeId
            };
        }

        public static void AssertUpdatedCommission(ISettingItem commissionItem, ISetting setting)
        {
            var settingType = setting.SettingTypes.Where(st => st.Id == SettingTypeInfo.CommissionSettingTypeId).FirstOrDefault();

            var newCommissionItem = settingType.SettingItems.ElementAt(0) as CommissionItem;

            var expectedCommissionItem = commissionItem as CommissionItem;

            Assert.AreEqual(expectedCommissionItem.CommRate, newCommissionItem.CommRate);
        }
    }
}