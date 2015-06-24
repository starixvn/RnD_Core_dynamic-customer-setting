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

        public virtual SettingType PostGetCustomerSettingType(
            QuerySettingParams queryParams,
            SettingType settingType)
        {
            throw new NotImplementedException("Post-Get Customer Setting Type is not implemented");
        }

        public virtual IResult PostUpdateCustomerSettingType(
            Customer customer,
            SettingType settingType,
            CustomerLevel? uplineLevel,
            IResult result)
        {
            throw new NotImplementedException("Post-Update Customer Setting Type is not implemented");
        }

        public virtual IResult PostUpdateMultipleCustomersSettingType(
            IEnumerable<Customer> customers,
            SettingType settingType,
            CustomerLevel? uplineLevel,
            IResult result)
        {
            throw new NotImplementedException("Post-Update Multiple Customers Setting is not implemented");
        }
    }
}