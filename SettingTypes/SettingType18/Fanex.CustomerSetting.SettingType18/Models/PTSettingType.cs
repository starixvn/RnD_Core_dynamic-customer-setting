namespace Fanex.CustomerSetting.SettingType18.Models
{
    using System;
    using System.Linq;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;

    [Serializable]
    public class PTSettingType : SettingType
    {
        public bool IsDisableMasterAutoPT { get; set; }

        public bool IsMultiple { get; set; }

        public override bool SatisfyCustomerLevelConstraint(Level actorLevel, Level customerLevel)
        {
            return actorLevel.IsCompanyLevel() || MeetExpectedPTDataType(actorLevel, customerLevel);
        }

        public string ToXml(bool isMinPtOnly)
        {
            var settingItemsXml = string.Join(string.Empty, SettingItems.Where(settingItem => ((PTItem)settingItem).IsUpdateMinPtOnly == isMinPtOnly).Select(settingItem => settingItem.ToXml()));
            return string.Format(SettingTypeXmlFormat, settingItemsXml);
        }

        private bool MeetExpectedPTDataType(Level actorLevel, Level customerLevel)
        {
            return MeetSuperPTDataType(customerLevel)
                || MeetMasterPTDataType(customerLevel)
                || MeetAgentPTDataType(customerLevel)
                || MeetMemberPTDataType(actorLevel, customerLevel);
        }

        private bool MeetSuperPTDataType(Level customerLevel)
        {
            return customerLevel.IsSuperLevel() && SettingItems.All(settingItem => settingItem.GetType() == typeof(SuperPTItem));
        }

        private bool MeetMasterPTDataType(Level customerLevel)
        {
            return customerLevel.IsMasterLevel() && SettingItems.All(settingItem => settingItem.GetType() == typeof(MasterPTItem));
        }

        private bool MeetAgentPTDataType(Level customerLevel)
        {
            return customerLevel.IsAgentLevel() && SettingItems.All(settingItem => settingItem.GetType() == typeof(AgentPTItem));
        }

        private bool MeetMemberPTDataType(Level actorLevel, Level customerLevel)
        {
            if (customerLevel.IsNotMemberLevel())
            {
                return false;
            }

            if (actorLevel.IsSuperLevel())
            {
                return SettingItems.All(settingItem => settingItem.GetType() == typeof(SuperPTOnMemberItem));
            }

            if (actorLevel.IsMasterLevel())
            {
                return SettingItems.Any(settingItem => settingItem.GetType() == typeof(MasterPTOnMemberItem));
            }

            if (actorLevel.IsAgentLevel())
            {
                return SettingItems.Any(settingItem => settingItem.GetType() == typeof(AgentPTOnMemberItem));
            }

            return false;
        }
    }
}