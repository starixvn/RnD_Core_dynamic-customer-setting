namespace Fanex.CustomerSetting.SettingType15.Models
{
    using System;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;

    [Serializable]
    public class WinningShareSettingType : SettingType
    {
        public override bool SatisfyCustomerLevelConstraint(Level actorLevel, Level customerLevel)
        {
            return false;
        }
    }
}