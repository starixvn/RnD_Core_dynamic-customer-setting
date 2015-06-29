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
        public virtual void PreGetCustomerSettingType(ISettingContext settingContext, QuerySettingParams queryParams)
        {
            throw new NotImplementedException("Pre-Get Customer Setting Type is not implemented");
        }

        public virtual IResult PreUpdateCustomerSettingType(
            ISettingContext settingContext,
            Customer customer,
            SettingType settingType,
            CustomerLevel? uplineLevel)
        {
            throw new NotImplementedException("Pre-Update Customer Setting Type is not implemented");
        }

        public virtual PreUpdateMultipleCustomersSettingTypeResult PreUpdateMultipleCustomersSettingType(
            ISettingContext settingContext, 
            IEnumerable<Customer> customers,
            CustomerLevel customerLevel,
            SettingType settingType,
            CustomerLevel? uplineLevel)
        {
            throw new NotImplementedException("Pre-Update Multiple Customer Setting Type is not implemented");
        }
    }
}