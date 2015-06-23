namespace Fanex.CustomerSetting.Handlers
{
    using System;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;

    public class SettingPreTasks
    {
        public SettingPreTasks(ISettingContext settingContext)
        {
            SettingContext = settingContext;
        }

        protected ISettingContext SettingContext { get; set; }

        public virtual void PreGetSetting(SettingQuery settingQuery)
        {
            throw new NotImplementedException("Pre-Get Setting is not implemented");
        }

        public virtual IResult PreUpdateSetting(
            Customer customer,
            SettingType settingType,
            CustomerLevel? uplineLevel)
        {
            throw new NotImplementedException("Pre-Update Setting is not implemented");
        }

        public virtual PreUpdateMultipleSettingResult PreUpdateMultipleSetting(
            IEnumerable<Customer> customers,
            CustomerLevel customerLevel,
            SettingType settingType,
            CustomerLevel? uplineLevel)
        {
            throw new NotImplementedException("Pre-Update Multiple Setting is not implemented");
        }
    }
}