namespace Fanex.CustomerSetting.SettingType1.Template1.Handlers
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.CustomerSetting.Core.Enumerations;
    using Fanex.CustomerSetting.Core.Handlers;
    using Fanex.CustomerSetting.Core.Models;

    public class SettingType1PreTasks : SettingPreTasks
    {
        public override PreUpdateMultipleSettingResult PreUpdateMultipleSetting(
            IEnumerable<Customer> customers,
            SettingType settingType,
            CustomerLevel? uplineLevel)
        {
            return base.PreUpdateMultipleSetting(customers, settingType, uplineLevel);
        }
    }
}