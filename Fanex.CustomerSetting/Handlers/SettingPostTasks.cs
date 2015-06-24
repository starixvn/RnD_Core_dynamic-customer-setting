namespace Fanex.CustomerSetting.Handlers
{
    using System;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;

    public class SettingPostTasks
    {
        public SettingPostTasks(ISettingContext settingContext)
        {
            SettingContext = settingContext;
        }

        protected ISettingContext SettingContext { get; set; }

        public virtual SettingType PostGetSetting(
            QuerySettingParams queryParams,
            SettingType settingType)
        {
            throw new NotImplementedException("Post-Get Setting is not implemented");
        }

        public virtual IResult PostUpdateSetting(
            Customer customer,
            SettingType settingType,
            CustomerLevel? uplineLevel,
            IResult result)
        {
            throw new NotImplementedException("Post-Update Setting is not implemented");
        }

        public virtual IResult PostUpdateMultipleSetting(
            IEnumerable<Customer> customers,
            SettingType settingType,
            CustomerLevel? uplineLevel,
            IResult result)
        {
            throw new NotImplementedException("Post-Update Multiple Setting is not implemented");
        }
    }
}