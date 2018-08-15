namespace Fanex.CustomerSetting.SettingType14.Models
{
    using System;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;

    [Serializable]
    public class TicketCommissionSettingType : SettingType
    {
        public override bool SatisfyCustomerLevelConstraint(Level actorLevel, Level customerLevel)
        {
            return customerLevel.IsNotMemberLevel();
        }
    }
}