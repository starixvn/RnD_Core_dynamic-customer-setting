namespace Fanex.CustomerSetting.Core.Handlers
{
    using System;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Core.Dtos.Params;
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.CustomerSetting.Core.Enumerations;
    using Fanex.CustomerSetting.Core.Models;

    public class SettingPostTasks
    {
        public SettingPostTasks()
            : this(null)
        {
        }

        public SettingPostTasks(ISettingContext settingContext)
        {
            SettingContext = settingContext ?? SettingContextManager.SettingContext;
        }

        protected ISettingContext SettingContext { get; set; }

        public virtual SettingType PostGetSetting(
            SettingQuery settingQuery,
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

        public virtual IMultipleResults PostUpdateMultipleSetting(
            IList<Customer> customers,
            SettingType settingType,
            CustomerLevel? uplineLevel,
            IMultipleResults multipleResults)
        {
            throw new NotImplementedException("Post-Update Multiple Setting is not implemented");
        }
    }
}