namespace Fanex.CustomerSetting.Core.Tasks
{
    using System;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Core.Enumerations;
    using Fanex.CustomerSetting.Core.Models;
    using Fanex.CustomerSetting.Core.Models.Params;
    using Fanex.CustomerSetting.Core.Results;

    public class SettingPostTasks
    {
        public virtual void PostGetSetting(
            SettingQuery settingQuery,
            SettingType settingType)
        {
            throw new NotImplementedException("PostGetSetting is not implemented");
        }

        public virtual IResults PostUpdateSetting(
            Customer customer,
            SettingType settingType,
            CustomerLevel? uplineLevel,
            IResults results)
        {
            throw new NotImplementedException("PostUpdateSetting is not implemented");
        }

        public virtual IMultipleResults PostUpdateMultipleSetting(
            IList<Customer> customers,
            SettingType settingType,
            CustomerLevel? uplineLevel,
            IMultipleResults multipleResults)
        {
            throw new NotImplementedException("PostUpdateMultipleSetting is not implemented");
        }
    }
}