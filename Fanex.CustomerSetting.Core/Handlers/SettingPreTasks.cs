namespace Fanex.CustomerSetting.Core.Handlers
{
    using System;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Core.Dtos.Params;
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.CustomerSetting.Core.Enumerations;
    using Fanex.CustomerSetting.Core.Models;

    public class SettingPreTasks
    {
        public virtual void PreGetSetting(SettingQuery settingQuery)
        {
            throw new NotImplementedException("PreGetSetting is not implemented");
        }

        public virtual IResult PreUpdateSetting(
            Customer customer,
            SettingType settingType,
            CustomerLevel? uplineLevel)
        {
            throw new NotImplementedException("PreUpdateSetting is not implemented");
        }

        public virtual IMultipleResults PreUpdateMultipleSetting(
            IList<Customer> customers,
            SettingType settingType,
            CustomerLevel? uplineLevel)
        {
            throw new NotImplementedException("PreUpdateMultipleSetting is not implemented");
        }
    }
}