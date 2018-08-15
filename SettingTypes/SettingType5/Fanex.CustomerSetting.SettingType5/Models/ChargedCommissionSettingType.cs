namespace Fanex.CustomerSetting.SettingType5.Models
{
    using System;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;

    [Serializable]
    public class ChargedCommissionSettingType : SettingType
    {
        public override bool SatisfyCustomerLevelConstraint(Level actorLevel, Level customerLevel)
        {
            return customerLevel.IsNotMemberLevel();
        }
    }
}